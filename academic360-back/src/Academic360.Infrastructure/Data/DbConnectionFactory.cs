using System.Data;
using Academic360.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Academic360.Infrastructure.Data;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        // Se requiere abrir explícitamente para manejar transacciones inmediatamente después.
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}