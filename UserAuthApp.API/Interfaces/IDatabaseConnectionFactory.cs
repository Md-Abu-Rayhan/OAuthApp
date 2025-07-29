using System.Data;

namespace UserAuthApp.API.Interfaces;

public interface IDatabaseConnectionFactory
{
    IDbConnection CreateConnection();
}