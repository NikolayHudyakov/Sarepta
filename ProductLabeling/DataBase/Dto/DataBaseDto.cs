namespace ProductLabeling.DataBase.Dto
{
    internal class DataBaseDto
    {
        public Dbms DbmsType { get; set; }
        public PostgreSqlDto PostgreSql { get; set; } = new();
        public MySqlDto MySql { get; set; } = new();
    }

    public enum Dbms
    {
        PostgreSql,
        MySql,
        //MsSql
    }
}
