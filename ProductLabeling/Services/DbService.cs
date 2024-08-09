using ProductLabeling.DataBase;
using ProductLabeling.DataBase.Dto;
using ProductLabeling.DataBase.Interfases;
using ProductLabeling.Models;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System;

namespace ProductLabeling.Services
{
    internal class DbService : AppSettingsProvider, IDbService
    {
        private readonly IDataBase _dataBase;
        private readonly DataBaseDto _dto;

        public DbService(ISerializerService<AppSettingsDto> appSettings) : base(appSettings)
        {
            _dto = dto.DataBase;
            _dataBase = GetDataBase();
        }

        public IDataBase DataBase => _dataBase;
        private IDataBase GetDataBase()
        {
            return _dto.DbmsType switch
            {
                Dbms.PostgreSql => new PostgreSql() { Dto = _dto.PostgreSql },
                Dbms.MySql => new DataBase.MySql() { Dto = _dto.MySql },
                _ => throw new InvalidOperationException("СУБД не поддерживается"),
            };
        }
    }
}
