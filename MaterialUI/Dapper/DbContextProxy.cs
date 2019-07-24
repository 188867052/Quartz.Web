namespace Dapper.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using static Dapper.SqlMapper;

    internal class DbContextProxy : IDbContext
    {
        private readonly IDbContext target = null;

        public DbContextProxy(IDbContext target)
        {
            this.target = target;
            this.Loggers = new List<Logger>();
        }

        public List<Logger> Loggers { get; private set; }

        public IDbConnection Connection => this.target.Connection;

        public IDbTransaction Transaction => this.target.Transaction;

        public DbContextState State => this.target.State;

        public DataSourceType SourceType => this.target.SourceType;

        public bool? Buffered { get => this.target.Buffered; set => this.target.Buffered = value; }

        public int? Timeout { get => this.target.Timeout; set => this.target.Timeout = value; }

        public void Close()
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                this.target.Close();
                watch.Stop();
            }
            finally
            {
                this.Loggers.Add(new Logger()
                {
                    Value = null,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    Text = nameof(DbContextProxy.Close),
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public void Commit()
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                this.target.Commit();
                watch.Stop();
            }
            finally
            {
                this.Loggers.Add(new Logger()
                {
                    Value = null,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    Text = nameof(this.Commit),
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public void Dispose()
        {
            this.target.Dispose();
        }

        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.Execute(sql, param, commandTimeout, text);
            }
            catch (Exception ex)
            {
                return 1;
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.ExecuteAsync(sql, param, commandTimeout, text);
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.ExecuteScalar<T>(sql, param, commandTimeout, text);
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.ExecuteScalarAsync<T>(sql, param, commandTimeout, text);
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
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

            if (this.SourceType == DataSourceType.SqlServer)
            {
                return new SqlQuery<T1, T2, T3>(this);
            }
            else if (this.SourceType == DataSourceType.Sqlite)
            {
                return new SQLiteQuery<T1, T2, T3>(this);
            }

            throw new NotImplementedException();
        }

        public void Open(bool beginTransaction, IsolationLevel? level = null)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                this.target.Open(beginTransaction, level);
                watch.Stop();
            }
            finally
            {
                this.Loggers.Add(new Logger()
                {
                    Value = null,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    Text = string.Format("{0}:Transaction:{1}", nameof(this.Open), beginTransaction ? "ON" : "OFF"),
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public async Task OpenAsync(bool beginTransaction, IsolationLevel? level = null)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                await this.target.OpenAsync(beginTransaction, level);
                watch.Stop();
            }
            finally
            {
                this.Loggers.Add(new Logger()
                {
                    Value = null,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    Text = string.Format("{0}:Transaction:{1}", nameof(this.Open), beginTransaction ? "ON" : "OFF"),
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.Query<T>(sql, param, buffered, commandTimeout, commandType);
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.QueryAsync<T>(sql, param, commandTimeout, commandType);
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.Query(sql, param, buffered, commandTimeout, commandType);
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.QueryAsync(sql, param, commandTimeout, commandType);
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public GridReader QueryMultiple(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.QueryMultiple(sql, param, commandTimeout, text);
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public Task<GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType text = CommandType.Text)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                return this.target.QueryMultipleAsync(sql, param, commandTimeout, text);
            }
            finally
            {
                watch.Stop();
                this.Loggers.Add(new Logger()
                {
                    Value = param,
                    Text = sql,
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }

        public void Rollback()
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                this.target.Rollback();
                watch.Stop();
            }
            finally
            {
                this.Loggers.Add(new Logger()
                {
                    Value = null,
                    Text = nameof(this.Rollback),
                    Buffered = this.Buffered,
                    Timeout = this.Timeout,
                    ExecuteTime = watch.ElapsedMilliseconds,
                });
            }
        }
    }
}
