﻿namespace Data.SqlServer.Specific
{
    public class HardcodedSqlServerConfiguration : ISqlServerConfiguration
    {
        // TODO: Read the ConnectionString from config.
        public string ConnectionString => "Server=.;Database=CleanArchitecture;Trusted_Connection=True;";
    }
}
