namespace Dapper.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper.Common.Util;

    public class MysqlQuery<T> : IQueryable<T>
        where T : class
    {
        public IDbContext _dbcontext { get; }

        public MysqlQuery(IDbContext dbcontext = null)
        {
            this._dbcontext = dbcontext;
            this._param = new Dictionary<string, object>();
        }

        public MysqlQuery(Dictionary<string, object> param)
        {
            this._param = param;
        }

        public IQueryable<T> With(string lockType, bool condition = true)
        {
            if (condition)
            {
                this.@lock.Append(lockType);
            }

            return this;
        }

        public IQueryable<T> With(LockType lockType, bool condition = true)
        {
            if (condition)
            {
                if (lockType == LockType.FOR_UPADTE)
                {
                    this.With("FOR UPDATE");
                }
                else if (lockType == LockType.LOCK_IN_SHARE_MODE)
                {
                    this.With("LOCK IN SHARE MODE");
                }
            }

            return this;
        }

        public IQueryable<T> Distinct(bool condition = true)
        {
            if (condition)
            {
                this.distinctBuffer.Append("DISTINCT");
            }

            return this;
        }

        public IQueryable<T> Filter<TResult>(Expression<Func<T, TResult>> columns, bool condition = true)
        {
            if (condition)
            {
                this.filters.AddRange(ExpressionUtil.BuildColumns(columns, this._param).Select(s => s.Value));
            }

            return this;
        }

        public IQueryable<T> GroupBy(string expression, bool condition = true)
        {
            if (condition)
            {
                if (this.groupBuffer.Length > 0)
                {
                    this.groupBuffer.Append(",");
                }

                this.groupBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T> GroupBy<TResult>(Expression<Func<T, TResult>> expression, bool condition = true)
        {
            if (condition)
            {
                this.GroupBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param).Select(s => s.Value)));
            }

            return this;
        }

        public IQueryable<T> Having(string expression, bool condition = true)
        {
            if (condition)
            {
                this.havingBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T> Having(Expression<Func<T, bool>> expression, bool condition = true)
        {
            if (condition)
            {
                this.Having(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param).Select(s => s.Value)));
            }

            return this;
        }

        public IQueryable<T> OrderBy(string orderBy, bool condition = true)
        {
            if (condition)
            {
                if (this.orderBuffer.Length > 0)
                {
                    this.orderBuffer.Append(",");
                }

                this.orderBuffer.Append(orderBy);
            }

            return this;
        }

        public IQueryable<T> OrderBy<TResult>(Expression<Func<T, TResult>> expression, bool condition = true)
        {
            if (condition)
            {
                this.OrderBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param).Select(s => string.Format("{0} ASC", s.Value))));
            }

            return this;
        }

        public IQueryable<T> OrderByDescending<TResult>(Expression<Func<T, TResult>> expression, bool condition = true)
        {
            if (condition)
            {
                this.OrderBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param).Select(s => string.Format("{0} DESC", s.Value))));
            }

            return this;
        }

        public IQueryable<T> Page(int index, int count, out int total, bool condition = true)
        {
            total = 0;
            if (condition)
            {
                this.Skip(count * (index - 1), count);
                total = this.Count();
            }

            return this;
        }

        public IQueryable<T> Set(string expression, Action<Dictionary<string, object>> action = null, bool condition = true)
        {
            if (condition)
            {
                if (this.setBuffer.Length > 0)
                {
                    this.setBuffer.Append(",");
                }

                action?.Invoke(this._param);
                this.setBuffer.AppendFormat(expression);
            }

            return this;
        }

        public IQueryable<T> Set<TResult>(Expression<Func<T, TResult>> column, TResult value, bool condition = true)
        {
            if (condition)
            {
                if (this.setBuffer.Length > 0)
                {
                    this.setBuffer.Append(",");
                }

                var columns = ExpressionUtil.BuildColumn(column, this._param).First();
                var key = string.Format("{0}{1}", columns.Key, this._param.Count);
                this._param.Add(key, value);
                this.setBuffer.AppendFormat("{0} = @{1}", columns.Value, key);
            }

            return this;
        }

        public IQueryable<T> Set<TResult>(Expression<Func<T, TResult>> column, Expression<Func<T, TResult>> value, bool condition = true)
        {
            if (condition)
            {
                if (this.setBuffer.Length > 0)
                {
                    this.setBuffer.Append(",");
                }

                var columnName = ExpressionUtil.BuildColumn(column, this._param).First().Value;
                var expression = ExpressionUtil.BuildExpression(value, this._param);
                this.setBuffer.AppendFormat("{0} = {1}", columnName, expression);
            }

            return this;
        }

        public IQueryable<T> Skip(int index, int count, bool condition = true)
        {
            if (condition)
            {
                this.pageIndex = index;
                this.pageCount = count;
            }

            return this;
        }

        public IQueryable<T> Take(int count)
        {
            this.Skip(0, count);
            return this;
        }

        public IQueryable<T> Where(string expression, Action<Dictionary<string, object>> action = null, bool condition = true)
        {
            if (condition)
            {
                if (this.whereBuffer.Length > 0)
                {
                    this.whereBuffer.AppendFormat(" {0} ", Operator.GetOperator(ExpressionType.AndAlso));
                }

                action?.Invoke(this._param);
                this.whereBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression, bool condition = true)
        {
            if (condition)
            {
                this.Where(ExpressionUtil.BuildExpression(expression, this._param), null);
            }

            return this;
        }

        public int Delete(bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildDelete();
                return this._dbcontext.Execute(sql, this._param, timeout);
            }

            return 0;
        }

        public async Task<int> DeleteAsync(bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildDelete();
                return await this._dbcontext.ExecuteAsync(sql, this._param, timeout);
            }

            return 0;
        }

        public int Insert(T entity, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildInsert();
                return this._dbcontext.Execute(sql, entity, timeout);
            }

            return 0;
        }

        public async Task<int> InsertAsync(T entity, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildInsert();
                return await this._dbcontext.ExecuteAsync(sql, entity, timeout);
            }

            return 0;
        }

        public long InsertReturnId(T entity, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildInsert();
                sql = string.Format("{0};SELECT @@IDENTITY;", sql);
                return this._dbcontext.ExecuteScalar<long>(sql, entity, timeout);
            }

            return 0;
        }

        public async Task<long> InsertReturnIdAsync(T entity, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildInsert();
                sql = string.Format("{0};SELECT @@IDENTITY;", sql);
                return await this._dbcontext.ExecuteScalarAsync<long>(sql, entity, timeout);
            }

            return 0;
        }

        public int Insert(IEnumerable<T> entitys, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildInsert();
                return this._dbcontext.Execute(sql, entitys, timeout);
            }

            return 0;
        }

        public async Task<int> InsertAsync(IEnumerable<T> entitys, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildInsert();
                return await this._dbcontext.ExecuteAsync(sql, entitys, timeout);
            }

            return 0;
        }

        public int Update(bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null && this.setBuffer.Length > 0)
            {
                var sql = this.BuildUpdate(false);
                return this._dbcontext.Execute(sql, this._param, timeout);
            }

            return 0;
        }

        public async Task<int> UpdateAsync(bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null && this.setBuffer.Length > 0)
            {
                var sql = this.BuildUpdate(false);
                return await this._dbcontext.ExecuteAsync(sql, this._param, timeout);
            }

            return 0;
        }

        public int Update(T entity, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildUpdate();
                return this._dbcontext.Execute(sql, entity, timeout);
            }

            return 0;
        }

        public async Task<int> UpdateAsync(T entity, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildUpdate();
                return await this._dbcontext.ExecuteAsync(sql, entity, timeout);
            }

            return 0;
        }

        public int Update(IEnumerable<T> entitys, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildUpdate();
                return this._dbcontext.Execute(sql, entitys, timeout);
            }

            return 0;
        }

        public async Task<int> UpdateAsync(IEnumerable<T> entitys, bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildUpdate();
                return await this._dbcontext.ExecuteAsync(sql, entitys, timeout);
            }

            return 0;
        }

        public T Single(string columns = null, bool buffered = true, int? timeout = null)
        {
            this.Take(1);
            return this.Select(columns, buffered, timeout).SingleOrDefault();
        }

        public async Task<T> SingleAsync(string columns = null, int? timeout = null)
        {
            this.Take(1);
            return (await this.SelectAsync(columns, timeout)).SingleOrDefault();
        }

        public TResult Single<TResult>(string columns = null, bool buffered = true, int? timeout = null)
        {
            this.Take(1);
            return this.Select<TResult>(columns, buffered, timeout).SingleOrDefault();
        }

        public async Task<TResult> SingleAsync<TResult>(string columns = null, int? timeout = null)
        {
            this.Take(1);
            return (await this.SelectAsync<TResult>(columns, timeout)).SingleOrDefault();
        }

        public TResult Single<TResult>(Expression<Func<T, TResult>> columns, bool buffered = true, int? timeout = null)
        {
            var columnstr = string.Join(",",
                ExpressionUtil.BuildColumns(columns, this._param).Select(s => string.Format("{0} AS {1}", s.Value, s.Key)));
            return this.Single<TResult>(columnstr, buffered, timeout);
        }

        public Task<TResult> SingleAsync<TResult>(Expression<Func<T, TResult>> columns, int? timeout = null)
        {
            var columnstr = string.Join(",",
                ExpressionUtil.BuildColumns(columns, this._param).Select(s => string.Format("{0} AS {1}", s.Value, s.Key)));
            return this.SingleAsync<TResult>(columnstr, timeout);
        }

        public IEnumerable<T> Select(string colums = null, bool buffered = true, int? timeout = null)
        {
            if (colums != null)
            {
                this.columnBuffer.Append(colums);
            }

            if (this._dbcontext != null)
            {
                var sql = this.BuildSelect();
                return this._dbcontext.Query<T>(sql, this._param, buffered, timeout);
            }

            return new List<T>();
        }

        public async Task<IEnumerable<T>> SelectAsync(string colums = null, int? timeout = null)
        {
            if (colums != null)
            {
                this.columnBuffer.Append(colums);
            }

            if (this._dbcontext != null)
            {
                var sql = this.BuildSelect();
                return await this._dbcontext.QueryAsync<T>(sql, this._param, timeout);
            }

            return new List<T>();
        }

        public IEnumerable<TResult> Select<TResult>(string columns = null, bool buffered = true, int? timeout = null)
        {
            if (columns != null)
            {
                this.columnBuffer.Append(columns);
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
                this.columnBuffer.Append(columns);
            }

            if (this._dbcontext != null)
            {
                var sql = this.BuildSelect();
                return await this._dbcontext.QueryAsync<TResult>(sql, this._param, timeout);
            }

            return new List<TResult>();
        }

        public IEnumerable<TResult> Select<TResult>(Expression<Func<T, TResult>> columns, bool buffered = true, int? timeout = null)
        {
            var columstr = string.Join(",",
                ExpressionUtil.BuildColumns(columns, this._param).Select(s => string.Format("{0} AS {1}", s.Value, s.Key)));
            return this.Select<TResult>(columstr, buffered, timeout);
        }

        public Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> columns, int? timeout = null)
        {
            var columstr = string.Join(",",
                ExpressionUtil.BuildColumns(columns, this._param).Select(s => string.Format("{0} AS {1}", s.Value, s.Key)));
            return this.SelectAsync<TResult>(columstr, timeout);
        }

        public int Count(string columns = null, bool codition = true, int? timeout = null)
        {
            if (codition)
            {
                if (columns != null)
                {
                    this.columnBuffer.Append(columns);
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
                    this.columnBuffer.Append(columns);
                }

                if (this._dbcontext != null)
                {
                    var sql = this.BuildCount();
                    return await this._dbcontext.ExecuteScalarAsync<long>(sql, this._param, timeout);
                }
            }

            return 0;
        }

        public int Count<TResult>(Expression<Func<T, TResult>> expression, bool condition = true, int? timeout = null)
        {
            return this.Count(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param).Select(s => s.Value)), condition, timeout);
        }

        public Task<long> CountAsync<TResult>(Expression<Func<T, TResult>> expression, bool condition = true, int? timeout = null)
        {
            return this.CountAsync(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param).Select(s => s.Value)), condition, timeout);
        }

        public bool Exists(bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildExists();
                return this._dbcontext.ExecuteScalar<int>(sql, this._param, timeout) > 0;
            }

            return false;
        }

        public async Task<bool> ExistsAsync(bool condition = true, int? timeout = null)
        {
            if (condition && this._dbcontext != null)
            {
                var sql = this.BuildExists();
                return await this._dbcontext.ExecuteScalarAsync<int>(sql, this._param, timeout) > 0;
            }

            return false;
        }

        public TResult Sum<TResult>(Expression<Func<T, TResult>> expression, bool condition = true, int? timeout = null)
        {
            if (condition)
            {
                var column = ExpressionUtil.BuildColumn(expression, this._param).First();
                this.sumBuffer.AppendFormat("{0}", column.Value);
                if (this._dbcontext != null)
                {
                    var sql = this.BuildSum();
                    return this._dbcontext.ExecuteScalar<TResult>(sql, this._param, timeout);
                }
            }

            return default;
        }

        public async Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> expression, bool condition = true, int? timeout = null)
        {
            if (condition)
            {
                var column = ExpressionUtil.BuildColumn(expression, this._param).First();
                this.sumBuffer.AppendFormat("{0}", column.Value);
                if (this._dbcontext != null)
                {
                    var sql = this.BuildSum();
                    return await this._dbcontext.ExecuteScalarAsync<TResult>(sql, this._param, timeout);
                }
            }

            return default;
        }

        public Dictionary<string, object> _param { get; set; }
        public List<string> filters = new List<string>();
        public StringBuilder columnBuffer = new StringBuilder();
        public StringBuilder setBuffer = new StringBuilder();
        public StringBuilder havingBuffer = new StringBuilder();
        public StringBuilder whereBuffer = new StringBuilder();
        public StringBuilder groupBuffer = new StringBuilder();
        public StringBuilder orderBuffer = new StringBuilder();
        public StringBuilder distinctBuffer = new StringBuilder();
        public StringBuilder countBuffer = new StringBuilder();
        public StringBuilder sumBuffer = new StringBuilder();
        public StringBuilder @lock = new StringBuilder();
        public Table table = EntityUtil.GetTable<T>();
        public int? pageIndex = null;
        public int? pageCount = null;

        public string BuildInsert()
        {
            var sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                this.table.TableName,
                string.Join(",", this.table.Columns.FindAll(f => f.Identity == false && !this.filters.Exists(e => e == f.ColumnName)).Select(s => s.ColumnName)),
                string.Join(",", this.table.Columns.FindAll(f => f.Identity == false && !this.filters.Exists(e => e == f.ColumnName)).Select(s => string.Format("@{0}", s.CSharpName))));
            return sql;
        }

        public string BuildUpdate(bool allColumn = true)
        {
            if (allColumn)
            {
                var keyColumn = this.table.Columns.Find(f => f.ColumnKey == ColumnKey.Primary);
                var colums = this.table.Columns.FindAll(f => f.ColumnKey != ColumnKey.Primary && !this.filters.Exists(e => e == f.ColumnName));
                var sql = string.Format("UPDATE {0} SET {1} WHERE {2}",
                    this.table.TableName,
                    string.Join(",", colums.Select(s => string.Format("{0} = @{1}", s.ColumnName, s.CSharpName))),
                    string.Format("{0} = @{1}", keyColumn.ColumnName, keyColumn.CSharpName));
                return sql;
            }
            else
            {
                var sql = string.Format("UPDATE {0} SET {1}{2}",
                    this.table.TableName,
                    this.setBuffer,
                    this.whereBuffer.Length > 0 ? string.Format(" WHERE {0}", this.whereBuffer) : "");
                return sql;
            }
        }

        public string BuildDelete()
        {
            var sql = string.Format("DELETE FROM {0}{1}",
                this.table.TableName,
                this.whereBuffer.Length > 0 ? string.Format(" WHERE {0}", this.whereBuffer) : "");
            return sql;
        }

        public string BuildSelect()
        {
            var sqlBuffer = new StringBuilder("SELECT");
            if (this.distinctBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" {0}", this.distinctBuffer);
            }

            if (this.columnBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" {0}", this.columnBuffer);
            }
            else
            {
                sqlBuffer.AppendFormat(" {0}", string.Join(",", this.table.Columns.FindAll(f => !this.filters.Exists(e => e == f.ColumnName)).Select(s => string.Format("{0} AS {1}", s.ColumnName, s.CSharpName))));
            }

            sqlBuffer.AppendFormat(" FROM {0}", this.table.TableName);
            if (this.whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this.whereBuffer);
            }

            if (this.groupBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" GROUP BY {0}", this.groupBuffer);
            }

            if (this.havingBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" HAVING {0}", this.havingBuffer);
            }

            if (this.orderBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" ORDER BY {0}", this.orderBuffer);
            }

            if (this.pageIndex != null && this.pageCount != null)
            {
                sqlBuffer.AppendFormat(" LIMIT {0},{1}", this.pageIndex, this.pageCount);
            }

            if (this.@lock.Length > 0)
            {
                sqlBuffer.AppendFormat(" {0}", this.@lock);
            }

            var sql = sqlBuffer.ToString();
            return sql;
        }

        public string BuildCount()
        {
            var sqlBuffer = new StringBuilder("SELECT");
            if (this.columnBuffer.Length > 0)
            {
                sqlBuffer.Append(" COUNT(");
                if (this.distinctBuffer.Length > 0)
                {
                    sqlBuffer.AppendFormat("{0} ", this.distinctBuffer);
                }

                sqlBuffer.AppendFormat("{0})", this.columnBuffer);
            }
            else
            {
                if (this.groupBuffer.Length > 0)
                {
                    sqlBuffer.Append(" 1 AS COUNT");
                }
                else
                {
                    sqlBuffer.AppendFormat(" COUNT(1)");
                }
            }

            sqlBuffer.AppendFormat(" FROM {0}", this.table.TableName);
            if (this.whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this.whereBuffer);
            }

            if (this.groupBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" GROUP BY {0}", this.groupBuffer);
            }

            if (this.havingBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" HAVING {0}", this.havingBuffer);
            }

            if (this.groupBuffer.Length > 0)
            {
                return string.Format("SELECT COUNT(1) FROM ({0}) AS T", sqlBuffer);
            }
            else
            {
                return sqlBuffer.ToString();
            }
        }

        public string BuildExists()
        {
            var sqlBuffer = new StringBuilder();

            sqlBuffer.AppendFormat("SELECT 1 FROM {0}", this.table.TableName);
            if (this.whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this.whereBuffer);
            }

            if (this.groupBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" GROUP BY {0}", this.groupBuffer);
            }

            if (this.havingBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" HAVING {0}", this.havingBuffer);
            }

            var sql = string.Format("SELECT EXISTS({0})", sqlBuffer);
            return sql;
        }

        public string BuildSum()
        {
            var sqlBuffer = new StringBuilder();
            sqlBuffer.AppendFormat("SELECT SUM({0}) FROM {1}", this.sumBuffer, this.table.TableName);
            if (this.whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this.whereBuffer);
            }

            return sqlBuffer.ToString();
        }
    }

    public class MysqlQuery<T1, T2> : IQueryable<T1, T2>
        where T1 : class
        where T2 : class
    {
        public IDbContext _dbcontext { get; }

        public MysqlQuery(IDbContext dbcontext = null)
        {
            this._dbcontext = dbcontext;
            this._param = new Dictionary<string, object>();
        }

        public MysqlQuery(Dictionary<string, object> param)
        {
            this._param = param;
        }

        public IQueryable<T1, T2> Distinct(bool condition = true)
        {
            if (condition)
            {
                this.distinctBuffer.Append("DISTINCT");
            }

            return this;
        }

        public IQueryable<T1, T2> GroupBy(string expression, bool condition = true)
        {
            if (condition)
            {
                if (this.groupBuffer.Length > 0)
                {
                    this.groupBuffer.Append(",");
                }

                this.groupBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T1, T2> GroupBy<TResult>(Expression<Func<T1, T2, TResult>> expression, bool condition = true)
        {
            if (condition)
            {
                this.GroupBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => s.Value)));
            }

            return this;
        }

        public IQueryable<T1, T2> Having(string expression, bool condition = true)
        {
            if (condition)
            {
                this.havingBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T1, T2> Having(Expression<Func<T1, T2, bool>> expression, bool condition = true)
        {
            if (condition)
            {
                this.Having(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => s.Value)));
            }

            return this;
        }

        public IQueryable<T1, T2> OrderBy(string orderBy, bool condition = true)
        {
            if (condition)
            {
                if (this.orderBuffer.Length > 0)
                {
                    this.orderBuffer.Append(",");
                }

                this.orderBuffer.Append(orderBy);
            }

            return this;
        }

        public IQueryable<T1, T2> OrderBy<TResult>(Expression<Func<T1, T2, TResult>> expression, bool condition = true)
        {
            if (condition)
            {
                this.OrderBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => string.Format("{0} ASC", s.Value))));
            }

            return this;
        }

        public IQueryable<T1, T2> OrderByDescending<TResult>(Expression<Func<T1, T2, TResult>> expression, bool condition = true)
        {
            if (condition)
            {
                this.OrderBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => string.Format("{0} DESC", s.Value))));
            }

            return this;
        }

        public IQueryable<T1, T2> Page(int index, int count, out int total, bool condition = true)
        {
            total = 0;
            if (condition)
            {
                this.Skip(count * (index - 1), count);
                total = this.Count();
            }

            return this;
        }

        public IQueryable<T1, T2> Skip(int index, int count, bool condition = true)
        {
            if (condition)
            {
                this.pageIndex = index;
                this.pageCount = count;
            }

            return this;
        }

        public IQueryable<T1, T2> Take(int count)
        {
            this.Skip(0, count);
            return this;
        }

        public IQueryable<T1, T2> Where(string expression, Action<Dictionary<string, object>> action = null, bool condition = true)
        {
            if (condition)
            {
                if (this.whereBuffer.Length > 0)
                {
                    this.whereBuffer.AppendFormat(" {0} ", Operator.GetOperator(ExpressionType.AndAlso));
                }

                action?.Invoke(this._param);
                this.whereBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T1, T2> Where(Expression<Func<T1, T2, bool>> expression, bool condition = true)
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
                this.columnBuffer.Append(columns);
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
                this.columnBuffer.Append(columns);
            }

            if (this._dbcontext != null)
            {
                var sql = this.BuildSelect();
                return await this._dbcontext.QueryAsync<TResult>(sql, this._param, timeout);
            }

            return new List<TResult>();
        }

        public IEnumerable<TResult> Select<TResult>(Expression<Func<T1, T2, TResult>> columns, bool buffered = true, int? timeout = null)
        {
            var columstr = string.Join(",",
                ExpressionUtil.BuildColumns(columns, this._param, false).Select(s => string.Format("{0} AS {1}", s.Value, s.Key)));
            return this.Select<TResult>(columstr, buffered, timeout);
        }

        public Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<T1, T2, TResult>> columns, int? timeout = null)
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
                    this.columnBuffer.Append(columns);
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
                    this.columnBuffer.Append(columns);
                }

                if (this._dbcontext != null)
                {
                    var sql = this.BuildCount();
                    return await this._dbcontext.ExecuteScalarAsync<long>(sql, this._param, timeout);
                }
            }

            return 0;
        }

        public IQueryable<T1, T2> Join(string expression)
        {
            if (this.join.Length > 0)
            {
                this.join.Append(" ");
            }

            this.join.Append(expression);
            return this;
        }

        public IQueryable<T1, T2> Join(Expression<Func<T1, T2, bool>> expression, JoinType join = JoinType.Inner)
        {
            var onExpression = ExpressionUtil.BuildExpression(expression, this._param, false);
            var table1Name = EntityUtil.GetTable<T1>().TableName;
            var table2Name = EntityUtil.GetTable<T2>().TableName;
            var joinType = string.Format("{0} JOIN", join.ToString().ToUpper());
            this.Join(string.Format("{0} {1} {2} ON {3}", table1Name, joinType, table2Name, onExpression));
            return this;
        }

        public Dictionary<string, object> _param { get; set; }

        public StringBuilder columnBuffer = new StringBuilder();
        public StringBuilder havingBuffer = new StringBuilder();
        public StringBuilder whereBuffer = new StringBuilder();
        public StringBuilder groupBuffer = new StringBuilder();
        public StringBuilder orderBuffer = new StringBuilder();
        public StringBuilder distinctBuffer = new StringBuilder();
        public StringBuilder countBuffer = new StringBuilder();
        public StringBuilder join = new StringBuilder();
        public int? pageIndex = null;
        public int? pageCount = null;

        public string BuildSelect()
        {
            var sqlBuffer = new StringBuilder("SELECT");
            if (this.distinctBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" {0}", this.distinctBuffer);
            }

            sqlBuffer.AppendFormat(" {0}", this.columnBuffer);
            sqlBuffer.AppendFormat(" FROM {0}", this.join);
            if (this.whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this.whereBuffer);
            }

            if (this.groupBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" GROUP BY {0}", this.groupBuffer);
            }

            if (this.havingBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" HAVING {0}", this.havingBuffer);
            }

            if (this.orderBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" ORDER BY {0}", this.orderBuffer);
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
            if (this.columnBuffer.Length > 0)
            {
                sqlBuffer.Append(" COUNT(");
                if (this.distinctBuffer.Length > 0)
                {
                    sqlBuffer.AppendFormat("{0} ", this.distinctBuffer);
                }

                sqlBuffer.AppendFormat("{0})", this.columnBuffer);
            }
            else
            {
                if (this.groupBuffer.Length > 0)
                {
                    sqlBuffer.Append(" 1 AS COUNT");
                }
                else
                {
                    sqlBuffer.AppendFormat(" COUNT(1)");
                }
            }

            sqlBuffer.AppendFormat(" FROM {0}", this.join);
            if (this.whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this.whereBuffer);
            }

            if (this.groupBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" GROUP BY {0}", this.groupBuffer);
            }

            if (this.havingBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" HAVING {0}", this.havingBuffer);
            }

            if (this.groupBuffer.Length > 0)
            {
                return string.Format("SELECT COUNT(1) FROM ({0}) AS T", sqlBuffer);
            }
            else
            {
                return sqlBuffer.ToString();
            }
        }
    }

    public class MysqlQuery<T1, T2, T3> : IQueryable<T1, T2, T3>
        where T1 : class
        where T2 : class
        where T3 : class
    {
        public IDbContext _dbcontext { get; }

        public MysqlQuery(IDbContext dbcontext = null)
        {
            this._dbcontext = dbcontext;
            this._param = new Dictionary<string, object>();
        }

        public MysqlQuery(Dictionary<string, object> param)
        {
            this._param = param;
        }

        public IQueryable<T1, T2, T3> Distinct(bool condition = true)
        {
            if (condition)
            {
                this.distinctBuffer.Append("DISTINCT");
            }

            return this;
        }

        public IQueryable<T1, T2, T3> GroupBy(string expression, bool condition = true)
        {
            if (condition)
            {
                if (this.groupBuffer.Length > 0)
                {
                    this.groupBuffer.Append(",");
                }

                this.groupBuffer.Append(expression);
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
                this.havingBuffer.Append(expression);
            }

            return this;
        }

        public IQueryable<T1, T2, T3> Having(Expression<Func<T1, T2, T3, bool>> expression, bool condition = true)
        {
            if (condition)
            {
                this.Having(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => s.Value)));
            }

            return this;
        }

        public IQueryable<T1, T2, T3> OrderBy(string orderBy, bool condition = true)
        {
            if (condition)
            {
                if (this.orderBuffer.Length > 0)
                {
                    this.orderBuffer.Append(",");
                }

                this.orderBuffer.Append(orderBy);
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
                this.OrderBy(string.Join(",", ExpressionUtil.BuildColumns(expression, this._param, false).Select(s => string.Format("{0} DESC", s.Value))), condition);
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
                if (this.whereBuffer.Length > 0)
                {
                    this.whereBuffer.AppendFormat(" {0} ", Operator.GetOperator(ExpressionType.AndAlso));
                }

                action?.Invoke(this._param);
                this.whereBuffer.Append(expression);
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
                this.columnBuffer.Append(columns);
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
                this.columnBuffer.Append(columns);
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
                    this.columnBuffer.Append(columns);
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
                    this.columnBuffer.Append(columns);
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
            if (this.join.Length > 0)
            {
                this.join.Append(" ");
            }

            this.join.Append(expression);
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
            if (this.tables.Count == 0)
            {
                this.tables.Add(table1Name);
                this.tables.Add(table2Name);
                this.Join(string.Format("{0} {1} {2} ON {3}", table1Name, joinType, table2Name, onExpression));
            }
            else if (this.tables.Exists(a => table1Name == a))
            {
                this.tables.Add(table2Name);
                this.Join(string.Format("{0} {1} ON {2}", joinType, table2Name, onExpression));
            }
            else
            {
                this.tables.Add(table1Name);
                this.Join(string.Format("{0} {1} ON {2}", joinType, table1Name, onExpression));
            }

            return this;
        }

        public Dictionary<string, object> _param { get; set; }

        public StringBuilder columnBuffer = new StringBuilder();
        public StringBuilder havingBuffer = new StringBuilder();
        public StringBuilder whereBuffer = new StringBuilder();
        public StringBuilder groupBuffer = new StringBuilder();
        public StringBuilder orderBuffer = new StringBuilder();
        public StringBuilder distinctBuffer = new StringBuilder();
        public StringBuilder countBuffer = new StringBuilder();
        public StringBuilder join = new StringBuilder();
        public List<string> tables = new List<string>();
        public int? pageIndex = null;
        public int? pageCount = null;

        public string BuildSelect()
        {
            var sqlBuffer = new StringBuilder("SELECT");
            if (this.distinctBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" {0}", this.distinctBuffer);
            }

            sqlBuffer.AppendFormat(" {0}", this.columnBuffer);
            sqlBuffer.AppendFormat(" FROM {0}", this.join);
            if (this.whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this.whereBuffer);
            }

            if (this.groupBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" GROUP BY {0}", this.groupBuffer);
            }

            if (this.havingBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" HAVING {0}", this.havingBuffer);
            }

            if (this.orderBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" ORDER BY {0}", this.orderBuffer);
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
            if (this.columnBuffer.Length > 0)
            {
                sqlBuffer.Append(" COUNT(");
                if (this.distinctBuffer.Length > 0)
                {
                    sqlBuffer.AppendFormat("{0} ", this.distinctBuffer);
                }

                sqlBuffer.AppendFormat("{0})", this.columnBuffer);
            }
            else
            {
                if (this.groupBuffer.Length > 0)
                {
                    sqlBuffer.Append(" 1 AS COUNT");
                }
                else
                {
                    sqlBuffer.AppendFormat(" COUNT(1)");
                }
            }

            sqlBuffer.AppendFormat(" FROM {0}", this.join);
            if (this.whereBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" WHERE {0}", this.whereBuffer);
            }

            if (this.groupBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" GROUP BY {0}", this.groupBuffer);
            }

            if (this.havingBuffer.Length > 0)
            {
                sqlBuffer.AppendFormat(" HAVING {0}", this.havingBuffer);
            }

            if (this.groupBuffer.Length > 0)
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
