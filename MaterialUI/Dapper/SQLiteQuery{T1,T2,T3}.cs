namespace Dapper.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper.Common.Util;

    public class SQLiteQuery<T1, T2, T3> : IQueryable<T1, T2, T3>
        where T1 : class
        where T2 : class
        where T3 : class
    {
        public IDbContext _dbcontext { get; }

        public SQLiteQuery(IDbContext dbcontext = null)
        {
            this._dbcontext = dbcontext;
            this._param = new Dictionary<string, object>();
        }

        public SQLiteQuery(Dictionary<string, object> param)
        {
            this._param = param;
        }

        public IQueryable<T1, T2, T3> Distinct(bool condition = true)
        {
            if (condition)
            {
                this._distinctBuffer.Append("DISTINCT");
            }

            return this;
        }

        public IQueryable<T1, T2, T3> GroupBy(string expression, bool condition = true)
        {
            if (condition)
            {
                if (this._groupBuffer.Length > 0)
                {
                    this._groupBuffer.Append(",");
                }

                this._groupBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T1, T2, T3> GroupBy<TResult>(Expression<Func<T1, T2, T3, TResult>> expression, bool condition = true)
        {
            if (condition)
            {
                this.GroupBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => s.Value)));
            }

            return this;
        }

        public IQueryable<T1, T2, T3> Having(string expression, bool condition = true)
        {
            if (condition)
            {
                this._havingBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T1, T2, T3> Having(Expression<Func<T1, T2, T3, bool>> expression, bool condition = true)
        {
            this.Having(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => s.Value)), condition);
            return this;
        }

        public IQueryable<T1, T2, T3> OrderBy(string orderBy, bool condition = true)
        {
            if (condition)
            {
                if (this._orderBuffer.Length > 0)
                {
                    this._orderBuffer.Append(",");
                }

                this._orderBuffer.Append(orderBy);
            }

            return this;
        }

        public IQueryable<T1, T2, T3> OrderBy<TResult>(Expression<Func<T1, T2, T3, TResult>> expression, bool condition = true)
        {
            if (condition)
            {
                this.OrderBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => string.Format("{0} ASC", s.Value))));
            }

            return this;
        }

        public IQueryable<T1, T2, T3> OrderByDescending<TResult>(Expression<Func<T1, T2, T3, TResult>> expression, bool condition = true)
        {
            if (condition)
            {
                this.OrderBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => string.Format("{0} DESC", s.Value))));
            }

            return this;
        }

        public IQueryable<T1, T2, T3> Page(int index, int count, out int total, bool condition = true)
        {
            total = 0;
            if (condition)
            {
                this.Skip(count * (index - 1), count);
                total = this.Count();
            }

            return this;
        }

        public IQueryable<T1, T2, T3> Skip(int index, int count, bool condition = true)
        {
            if (condition)
            {
                this.pageIndex = index;
                this.pageCount = count;
            }

            return this;
        }

        public IQueryable<T1, T2, T3> Take(int count)
        {
            this.Skip(0, count);
            return this;
        }

        public IQueryable<T1, T2, T3> Where(string expression, Action<Dictionary<string, object>> action = null, bool condition = true)
        {
            if (condition)
            {
                if (this._whereBuffer.Length > 0)
                {
                    this._whereBuffer.AppendFormat(" {0} ", Operator.GetOperator(ExpressionType.AndAlso));
                }

                action?.Invoke(this._param);
                this._whereBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> expression, bool condition = true)
        {
            if (condition)
            {
                this.Where(ExpressionUtil.BuildExpression(expression, this._param, false), null);
            }

            return this;
        }

        public IEnumerable<TResult> Select<TResult>(string columns = null, bool buffered = true, int? timeout = null)
        {
            if (columns != null)
            {
                this._columnBuffer.Append(columns);
            }

            if (this._dbcontext != null)
            {
                var sql = this.BuildSelect();
                return this._dbcontext.Query<TResult>(sql, this._param, buffered, timeout);
            }

            return new List<TResult>();
        }

        public async Task<IEnumerable<TResult>> SelectAsync<TResult>(string columns = null, int? timeout = null)
        {
            if (columns != null)
            {
                this._columnBuffer.Append(columns);
            }

            if (this._dbcontext != null)
            {
                var sql = this.BuildSelect();
                return await this._dbcontext.QueryAsync<TResult>(sql, this._param, timeout);
            }

            return new List<TResult>();
        }

        public IEnumerable<TResult> Select<TResult>(Expression<Func<T1, T2, T3, TResult>> columns, bool buffered = true, int? timeout = null)
        {
            var columstr = string.Join(",",
                ExpressionUtil.BuildColumns(columns, this._param, false).Select(s => string.Format("{0} AS {1}", s.Value, s.Key)));
            return this.Select<TResult>(columstr, buffered, timeout);
        }

        public Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<T1, T2, T3, TResult>> columns, int? timeout = null)
        {
            var columstr = string.Join(",",
                ExpressionUtil.BuildColumns(columns, this._param, false).Select(s => string.Format("{0} AS {1}", s.Value, s.Key)));
            return this.SelectAsync<TResult>(columstr, timeout);
        }

        public int Count(string columns = null, bool codition = true, int? timeout = null)
        {
            if (codition)
            {
                if (columns != null)
                {
                    this._columnBuffer.Append(columns);
                }

                if (this._dbcontext != null)
                {
                    var sql = this.BuildCount();
                    return this._dbcontext.ExecuteScalar<int>(sql, this._param, timeout);
                }
            }

            return 0;
        }

        public async Task<long> CountAsync(string columns = null, bool codition = true, int? timeout = null)
        {
            if (codition)
            {
                if (columns != null)
                {
                    this._columnBuffer.Append(columns);
                }

                if (this._dbcontext != null)
                {
                    var sql = this.BuildCount();
                    return await this._dbcontext.ExecuteScalarAsync<long>(sql, this._param, timeout);
                }
            }

            return 0;
        }

        public IQueryable<T1, T2, T3> Join(string expression)
        {
            if (this._join.Length > 0)
            {
                this._join.Append(" ");
            }

            this._join.Append(expression);
            return this;
        }

        public IQueryable<T1, T2, T3> Join<E1, E2>(Expression<Func<E1, E2, bool>> expression, JoinType join = JoinType.Inner)
            where E1 : class
            where E2 : class
        {
            var onExpression = ExpressionUtil.BuildExpression(expression, this._param, false);
            var table1Name = EntityUtil.GetTable<E1>().TableName;
            var table2Name = EntityUtil.GetTable<E2>().TableName;
            var joinType = string.Format("{0} JOIN", join.ToString().ToUpper());
            if (this._tables.Count == 0)
            {
                this._tables.Add(table1Name);
                this._tables.Add(table2Name);
                this.Join(string.Format("{0} {1} {2} ON {3}", table1Name, joinType, table2Name, onExpression));
            }
            else if (this._tables.Exists(a => table1Name == a))
            {
                this._tables.Add(table2Name);
                this.Join(string.Format("{0} {1} ON {2}", joinType, table2Name, onExpression));
            }
            else
            {
                this._tables.Add(table1Name);
                this.Join(string.Format("{0} {1} ON {2}", joinType, table1Name, onExpression));
            }

            return this;
        }

        public Dictionary<string, object> _param { get; set; }

        public StringBuilder _columnBuffer = new StringBuilder();
        public StringBuilder _havingBuffer = new StringBuilder();
        public StringBuilder _whereBuffer = new StringBuilder();
        public StringBuilder _groupBuffer = new StringBuilder();
        public StringBuilder _orderBuffer = new StringBuilder();
        public StringBuilder _distinctBuffer = new StringBuilder();
        public StringBuilder _countBuffer = new StringBuilder();
        public StringBuilder _join = new StringBuilder();
        public List<string> _tables = new List<string>();
        public int? pageIndex = null;
        public int? pageCount = null;

        public string BuildSelect()
        {
            var sqlBuffer = new StringBuilder("SELECT");
            if (this._distinctBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" {0}", this._distinctBuffer);
            }

            sqlBuffer.AppendFormat(" {0}", this._columnBuffer);
            sqlBuffer.AppendFormat(" FROM {0}", this._join);
            if (this._whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this._whereBuffer);
            }

            if (this._groupBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" GROUP BY {0}", this._groupBuffer);
            }

            if (this._havingBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" HAVING {0}", this._havingBuffer);
            }

            if (this._orderBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" ORDER BY {0}", this._orderBuffer);
            }

            if (this.pageIndex != null && this.pageCount != null)
            {
                sqlBuffer.AppendFormat(" LIMIT {0},{1}", this.pageIndex, this.pageCount);
            }

            var sql = sqlBuffer.ToString();
            return sql;
        }

        public string BuildCount()
        {
            var sqlBuffer = new StringBuilder("SELECT");
            if (this._columnBuffer.Length > 0)
            {
                sqlBuffer.Append(" COUNT(");
                if (this._distinctBuffer.Length > 0)
                {
                    sqlBuffer.AppendFormat("{0} ", this._distinctBuffer);
                }

                sqlBuffer.AppendFormat("{0})", this._columnBuffer);
            }
            else
            {
                if (this._groupBuffer.Length > 0)
                {
                    sqlBuffer.Append(" 1 AS COUNT");
                }
                else
                {
                    sqlBuffer.AppendFormat(" COUNT(1)");
                }
            }

            sqlBuffer.AppendFormat(" FROM {0}", this._join);
            if (this._whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this._whereBuffer);
            }

            if (this._groupBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" GROUP BY {0}", this._groupBuffer);
            }

            if (this._havingBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" HAVING {0}", this._havingBuffer);
            }

            if (this._groupBuffer.Length > 0)
            {
                return string.Format("SELECT COUNT(1) FROM ({0}) AS T", sqlBuffer);
            }
            else
            {
                return sqlBuffer.ToString();
            }
        }
    }
}
