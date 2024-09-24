namespace Data.SqlServer.Specific
{
    public class SqlServerConfiguration(string connectionString) : ISqlServerConfiguration
    {
        // TODO: Read the ConnectionString from config.
        public string ConnectionString { get; } = connectionString;
    }
}
