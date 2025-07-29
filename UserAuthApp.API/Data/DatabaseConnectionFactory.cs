using Microsoft.Data.SqlClient;
using System.Data;
using UserAuthApp.API.Interfaces;

namespace UserAuthApp.API.Data;

public class DatabaseConnectionFactory : IDatabaseConnectionFactory
{
    private readonly string _connectionString;

    public DatabaseConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}