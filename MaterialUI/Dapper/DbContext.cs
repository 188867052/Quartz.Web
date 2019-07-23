namespace Dapper.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;
    using static Dapper.SqlMapper;

    internal class DbContext : IDbContext
    {
        public DataSourceType SourceType { get; private set; }

        public List<Logger> Loggers { get; private set; }

        public IDbTransaction Transaction { get; private set; }

        public IDbConnection Connection { get; }

        public bool? Buffered { get; set; }

        public int? Timeout { get; set; }

        public DbContextState State { get; private set; }

        public DbContext(IDbConnection connection, DataSourceType sourceType)
        {
            this.Connection = connection;
            this.SourceType = sourceType;
            this.State = DbContextState.Closed;
        }

        public void Open(bool beginTransaction, IsolationLevel? level = null)
        {
            if (!beginTransaction)
            {
                this.Connection.Open();
            }
            else
            {
                this.Connection.Open();
                this.Transaction = level == null ? this.Connection.BeginTransaction() : this.Connection.BeginTransaction(level.Value);
            }

            this.State = DbContextState.Open;
        }

        public async Task OpenAsync(bool beginTransaction, IsolationLevel? level = null)
        {
            this.State = DbContextState.Open;
            if (!(this.Connection is DbConnection))
            {
                throw new InvalidOperationException("Async operations require use of a DbConnection or an already-open IDbConnection");
            }

            if (!beginTransaction)
            {
                await (this.Connection as DbConnection).OpenAsync();
            }
            else
            {
                await (this.Connection as DbConnection).OpenAsync();
                this.Transaction = level == null ? this.Connection.BeginTransaction() : this.Connection.BeginTransaction(level.Value);
            }
        }

        public void Close()
        {
            this.Connection.Close();
            this.State = DbContextState.Closed;
        }

        public void Commit()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Commit();
                this.Transaction.Dispose();
                this.State = DbContextState.Commit;
            }
        }

        public void Rollback()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Rollback();
                this.Transaction.Dispose();
                this.State = DbContextState.Rollback;
            }
        }

        public void Dispose()
        {
            this.Close();
        }

        public GridReader QueryMultiple(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            return this.Connection.QueryMultiple(sql, param, this.Transaction, this.Timeout != null ? this.Timeout.Value : commandTimeout, text);
        }

        public Task<GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            return this.Connection.QueryMultipleAsync(sql, param, this.Transaction, this.Timeout != null ? this.Timeout.Value : commandTimeout, text);
        }

        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            return this.Connection.Execute(sql, param, this.Transaction, this.Timeout != null ? this.Timeout.Value : commandTimeout, text);
        }

        public Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            return this.Connection.ExecuteAsync(sql, param, this.Transaction, this.Timeout != null ? this.Timeout.Value : commandTimeout, text);
        }

        public T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            return this.Connection.ExecuteScalar<T>(sql, param, this.Transaction, this.Timeout != null ? this.Timeout.Value : commandTimeout, text);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            return this.Connection.ExecuteScalarAsync<T>(sql, param, this.Transaction, this.Timeout != null ? this.Timeout.Value : commandTimeout, text);
        }

        public IQueryable<T> From<T>()
            where T : class
        {
            switch (this.SourceType)
            {
                case DataSourceType.MysqL:
                    return new MysqlQuery<T>(this);
                case DataSourceType.SqlServer:
                    return new SqlQuery<T>(this);
                case DataSourceType.Sqlite:
                    return new SQLiteQuery<T>(this);
                default:
                    throw new NotImplementedException();
            }
        }

        public IQueryable<T1, T2> From<T1, T2>()
            where T1 : class
            where T2 : class
        {
            if (this.SourceType == DataSourceType.MysqL)
            {
                return new MysqlQuery<T1, T2>(this);
            }
            else if (this.SourceType == DataSourceType.SqlServer)
            {
                return new SqlQuery<T1, T2>(this);
            }
            else if (this.SourceType == DataSourceType.Sqlite)
            {
                return new SQLiteQuery<T1, T2>(this);
            }

            throw new NotImplementedException();
        }

        public IQueryable<T1, T2, T3> From<T1, T2, T3>()
            where T1 : class
            where T2 : class
            where T3 : class
        {
            if (this.SourceType == DataSourceType.MysqL)
            {
                return new MysqlQuery<T1, T2, T3>(this);
            }
            else if (this.SourceType == DataSourceType.SqlServer)
            {
                return new SqlQuery<T1, T2, T3>(this);
            }
            else if (this.SourceType == DataSourceType.Sqlite)
            {
                return new SQLiteQuery<T1, T2, T3>(this);
            }

            throw new NotImplementedException();
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.Connection.Query<T>(sql, param, this.Transaction, this.Buffered != null ? this.Buffered.Value : buffered, this.Timeout != null ? this.Timeout.Value : commandTimeout, commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.Connection.QueryAsync<T>(sql, param, this.Transaction, this.Timeout != null ? this.Timeout.Value : commandTimeout, commandType);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.Connection.Query(sql, param, this.Transaction, this.Buffered != null ? this.Buffered.Value : buffered, this.Timeout != null ? this.Timeout.Value : commandTimeout, commandType);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.Connection.QueryAsync(sql, param, this.Transaction, this.Timeout != null ? this.Timeout.Value : commandTimeout, commandType);
        }
    }
}
