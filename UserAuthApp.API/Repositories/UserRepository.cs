using Dapper;
using UserAuthApp.API.Interfaces;
using UserAuthApp.API.Models;

namespace UserAuthApp.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public UserRepository(IDatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Users WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Users WHERE Email = @Email";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetByProviderIdAsync(string providerId, string provider)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Users WHERE ProviderId = @ProviderId AND Provider = @Provider";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { ProviderId = providerId, Provider = provider });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Users ORDER BY CreatedAt DESC";
        return await connection.QueryAsync<User>(sql);
    }

    public async Task<int> CreateAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO Users (Email, Name, ProviderId, Provider, CreatedAt, UpdatedAt)
            VALUES (@Email, @Name, @ProviderId, @Provider, @CreatedAt, @UpdatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int);";
        
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        
        return await connection.QuerySingleAsync<int>(sql, user);
    }

    public async Task<bool> UpdateAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            UPDATE Users 
            SET Email = @Email, Name = @Name, ProviderId = @ProviderId, 
                Provider = @Provider, UpdatedAt = @UpdatedAt
            WHERE Id = @Id";
        
        user.UpdatedAt = DateTime.UtcNow;
        
        var rowsAffected = await connection.ExecuteAsync(sql, user);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "DELETE FROM Users WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}