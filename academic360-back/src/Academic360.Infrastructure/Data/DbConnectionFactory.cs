using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Academic360.Infrastructure.Data;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        // 1. Leemos el valor del interruptor en el secrets.json (ej. "Local" o "Supabase")
        var activeEnv = configuration["DatabaseSwitch"];

        // 2. Si el interruptor tiene un valor, buscamos esa cadena de conexión. 
        // Si está vacío o no existe, usamos "DefaultConnection" por defecto.
        _connectionString = !string.IsNullOrWhiteSpace(activeEnv)
            ? configuration.GetConnectionString(activeEnv)!
            : configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}

//using System.Data;
//using Academic360.Application.Common.Interfaces;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;

//namespace Academic360.Infrastructure.Data;

//public class DbConnectionFactory : IDbConnectionFactory
//{
//    private readonly IConfiguration _configuration;

//    public DbConnectionFactory(IConfiguration configuration)
//    {
//        _configuration = configuration;
//    }

//    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
//    {
//        var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
//        // Se requiere abrir explícitamente para manejar transacciones inmediatamente después.
//        await connection.OpenAsync(cancellationToken);
//        return connection;
//    }
//}