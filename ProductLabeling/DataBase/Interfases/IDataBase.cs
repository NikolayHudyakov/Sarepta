using System;
using System.Data;
using System.Threading.Tasks;

namespace ProductLabeling.DataBase.Interfases
{
    internal interface IDataBase
    {
        public event Action<bool>? Status;

        public event Action<string>? Error;

        public bool Connected { get; }

        public Task StartAsync();

        public Task StopAsync();

        public int ExecuteSqlRaw(string sql, params object?[] parameters);

        public DataTable FromSqlRaw(string sql, params object?[] parameters);
    }

    interface IDataBase<T> : IDataBase
    {
        protected T? Dto { get; set; }
    }
}
