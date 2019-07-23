namespace Dapper.Common.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;

    public class ExpressionUtil : ExpressionVisitor
    {
        private StringBuilder build = new StringBuilder();
        private Dictionary<string, object> param;
        private string paramName = "Name";
        private string operatorMethod;
        private string @operator;
        private bool singleTable;

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
            {
                this.SetName(node);
            }
            else
            {
                this.SetValue(node);
            }

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Operator))
            {
                this.build.Append("(");
                if (node.Arguments.Count == 1)
                {
                    this.Visit(node.Arguments[0]);
                    this.build.AppendFormat(" {0} ", Operator.GetOperator(node.Method.Name));
                }
                else if (node.Arguments.Count == 2)
                {
                    this.Visit(node.Arguments[0]);
                    this.@operator = Operator.GetOperator(node.Method.Name);
                    this.operatorMethod = node.Method.Name;
                    this.build.AppendFormat(" {0} ", this.@operator);
                    this.Visit(node.Arguments[1]);
                }
                else
                {
                    this.@operator = Operator.GetOperator(node.Method.Name);
                    this.Visit(node.Arguments[0]);
                    this.build.AppendFormat(" {0} ", this.@operator);
                    this.Visit(node.Arguments[1]);
                    this.build.AppendFormat(" {0} ", Operator.GetOperator(ExpressionType.AndAlso));
                    this.Visit(node.Arguments[2]);
                }

                this.build.Append(")");
            }
            else if (node.Method.GetCustomAttributes(typeof(FunctionAttribute), true).Length > 0)
            {
                this.build.AppendFormat("{0}(", node.Method.Name.ToUpper());
                var parameters = node.Method.GetParameters();
                for (int i = 0; i < node.Arguments.Count; i++)
                {
                    if (parameters[i].GetCustomAttributes(typeof(ParameterAttribute), true).Length > 0)
                    {
                        this.build.Append((node.Arguments[i] as ConstantExpression).Value);
                        continue;
                    }

                    this.Visit(node.Arguments[i]);
                    if (i + 1 != node.Arguments.Count)
                    {
                        this.build.Append(",");
                    }
                }

                this.build.Append(")");
            }
            else
            {
                this.SetValue(node);
            }

            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            this.build.Append("(");
            this.Visit(node.Left);
            if (node.Right is ConstantExpression && (node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual) && (node.Right as ConstantExpression).Value == null)
            {
                this.@operator = node.NodeType == ExpressionType.Equal ? Operator.GetOperator(nameof(Operator.IsNull)) : Operator.GetOperator(nameof(Operator.IsNotNull));
                this.build.AppendFormat(" {0}", this.@operator);
            }
            else
            {
                this.@operator = Operator.GetOperator(node.NodeType);
                this.build.AppendFormat(" {0} ", this.@operator);
                this.Visit(node.Right);
            }

            this.build.Append(")");
            return node;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            this.SetValue(node);
            return node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            this.SetValue(node);
            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                this.build.AppendFormat("{0}", Operator.GetOperator(ExpressionType.Not));
            }

            this.Visit(node.Operand);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value == null)
            {
                this.build.AppendFormat("NULL");
            }
            else if (this.@operator == "LIKE" || this.@operator == "NOT LIKE")
            {
                if (this.operatorMethod == nameof(Operator.LikeLeft) || this.operatorMethod == nameof(Operator.NotLikeLeft))
                {
                    this.build.AppendFormat("'%{0}'", node.Value);
                }
                else if (this.operatorMethod == nameof(Operator.NotLikeRight) || this.operatorMethod == nameof(Operator.LikeRight))
                {
                    this.build.AppendFormat("'{0}%'", node.Value);
                }
                else
                {
                    this.build.AppendFormat("'%{0}%'", node.Value);
                }
            }
            else if (node.Value is string)
            {
                this.build.AppendFormat("'{0}'", node.Value);
            }
            else if(node.Value is Enum)
            {
                this.build.AppendFormat("'{0}'", Convert.ToInt32(node.Value));
            }
            else
            {
                this.build.AppendFormat("{0}", node.Value);
            }

            return node;
        }

        public void SetName(MemberExpression expression)
        {
            var name = expression.Member.Name;
            var columnName = EntityUtil.GetColumn(expression.Expression.Type, f => f.CSharpName == name)?.ColumnName ?? name;
            if (!this.singleTable)
            {
                var tableName = EntityUtil.GetTable(expression.Expression.Type).TableName;
                columnName = string.Format("{0}.{1}", tableName, columnName);
            }

            this.build.Append(columnName);
            this.paramName = name;
        }

        public void SetValue(Expression expression)
        {
            var value = GetValue(expression);
            if (this.@operator == "LIKE" || this.@operator == "NOT LIKE")
            {
                if (this.operatorMethod == nameof(Operator.LikeLeft) || this.operatorMethod == nameof(Operator.NotLikeLeft))
                {
                    value = string.Format("%{0}", value);
                }
                else if (this.operatorMethod == nameof(Operator.NotLikeRight) || this.operatorMethod == nameof(Operator.LikeRight))
                {
                    value = string.Format("{0}%", value);
                }
                else
                {
                    value = string.Format("%{0}%", value);
                }
            }
            else if(value is Enum)
            {
                value = Convert.ToInt32(value);
            }

            var key = string.Format("@{0}{1}", this.paramName, this.param.Count);
            this.param.Add(key, value);
            this.build.Append(key);
        }

        public static string BuildExpression(Expression expression, Dictionary<string, object> param, bool singleTable = true)
        {
            var visitor = new ExpressionUtil
            {
                param = param,
                singleTable = singleTable,
            };
            visitor.Visit(expression);
            return visitor.build.ToString();
        }

        public static Dictionary<string, string> BuildColumns(Expression expression, Dictionary<string, object> param, bool singleTable = true)
        {
            var columns = new Dictionary<string, string>();
            if (expression is LambdaExpression)
            {
                expression = (expression as LambdaExpression).Body;
            }

            if (expression is MemberInitExpression)
            {
                var initExpression = expression as MemberInitExpression;
                for (int i = 0; i < initExpression.Bindings.Count; i++)
                {
                    var column = string.Empty;
                    Expression argument = (initExpression.Bindings[i] as MemberAssignment).Expression;
                    if (argument is UnaryExpression)
                    {
                        argument = (argument as UnaryExpression).Operand;
                    }

                    if (argument is MethodCallExpression && (argument as MethodCallExpression).Method.DeclaringType == typeof(Convert))
                    {
                        column = GetValue((argument as MethodCallExpression).Arguments[0]).ToString();
                    }
                    else if (argument is ConstantExpression || (argument is MemberExpression && (argument as MemberExpression).Expression is ConstantExpression))
                    {
                        column = GetValue(argument).ToString();
                    }
                    else
                    {
                        column = BuildExpression(argument, param, singleTable);
                    }

                    var name = initExpression.Bindings[i].Member.Name;
                    columns.Add(name, column);
                }
            }
            else if (expression is NewExpression)
            {
                var newExpression = (expression as NewExpression);
                for (int i = 0; i < newExpression.Arguments.Count; i++)
                {
                    var columnName = string.Empty;
                    var argument = newExpression.Arguments[i];
                    if (argument is MethodCallExpression && (argument as MethodCallExpression).Method.DeclaringType == typeof(Convert))
                    {
                        columnName = GetValue((argument as MethodCallExpression).Arguments[0]).ToString();
                    }
                    else if (argument is ConstantExpression || (argument is MemberExpression && (argument as MemberExpression).Expression is ConstantExpression))
                    {
                        columnName = GetValue(argument).ToString();
                    }
                    else
                    {
                        columnName = BuildExpression(argument, param, singleTable);
                    }

                    var name = newExpression.Members[i].Name;
                    columns.Add(name, columnName);
                }
            }
            else if (expression is MemberExpression)
            {
                var memberExpression = (expression as MemberExpression);
                var name = memberExpression.Member.Name;
                var columnName = EntityUtil.GetColumn(memberExpression.Expression.Type, f => f.CSharpName == name)?.ColumnName ?? name;
                if (!singleTable)
                {
                    var tableName = EntityUtil.GetTable(memberExpression.Expression.Type).TableName;
                    columnName = string.Format("{0}.{1}", tableName, columnName);
                }

                columns.Add(name, columnName);
            }
            else
            {
                var name = string.Format("COLUMN0");
                var columnName = BuildExpression(expression, param, singleTable);
                columns.Add(name, columnName);
            }

            return columns;
        }

        public static Dictionary<string, string> BuildColumn(Expression expression, Dictionary<string, object> param, bool singleTable = true)
        {
            if (expression is LambdaExpression)
            {
                expression = (expression as LambdaExpression).Body;
            }

            var column = new Dictionary<string, string>();
            if (expression is MemberExpression)
            {
                var memberExpression = (expression as MemberExpression);
                var name = memberExpression.Member.Name;
                var columnName = EntityUtil.GetColumn(memberExpression.Expression.Type, f => f.CSharpName == name)?.ColumnName ?? name;
                if (!singleTable)
                {
                    var tableName = EntityUtil.GetTable(memberExpression.Expression.Type).TableName;
                    columnName = string.Format("{0}.{1}", tableName, columnName);
                }

                column.Add(name, columnName);
                return column;
            }
            else
            {
                var name = string.Format("COLUMN0");
                var build = BuildExpression(expression, param, singleTable);
                column.Add(name, build);
                return column;
            }
        }

        public static object GetValue(Expression expression)
        {
            var names = new Stack<string>();
            var exps = new Stack<Expression>();
            var mifs = new Stack<System.Reflection.MemberInfo>();
            var tempExpression = expression;
            while (tempExpression is MemberExpression)
            {
                var member = tempExpression as MemberExpression;
                names.Push(member.Member.Name);
                exps.Push(member.Expression);
                mifs.Push(member.Member);
                tempExpression = member.Expression;
            }

            if (names.Count > 0)
            {
                object value = null;
                foreach (var name in names)
                {
                    var exp = exps.Pop();
                    var mif = mifs.Pop();
                    if (exp is ConstantExpression cex)
                    {
                        value = cex.Value;
                    }

                    if ((mif is System.Reflection.PropertyInfo pif))
                    {
                        value = pif.GetValue(value);
                    }
                    else if ((mif is System.Reflection.FieldInfo fif))
                    {
                        value = fif.GetValue(value);
                    }
                }

                return value;
            }
            else if (expression is ConstantExpression)
            {
                return (tempExpression as ConstantExpression).Value;
            }
            else
            {
                return Expression.Lambda(expression).Compile().DynamicInvoke();
            }
        }
    }
}
