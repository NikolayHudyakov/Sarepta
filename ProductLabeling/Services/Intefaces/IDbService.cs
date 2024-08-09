using ProductLabeling.DataBase.Interfases;

namespace ProductLabeling.Services.Intefaces
{
    internal interface IDbService
    {
        public IDataBase DataBase { get; }
    }
}
