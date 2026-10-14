using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Auth.Commands.Login;
using Academic360.Application.Auth.Interfaces;
using Academic360.Application.Common.Exceptions;
using Academic360.Application.Common.Interfaces;
using Dapper;
using Npgsql; // Para capturar excepciones específicas

namespace Academic360.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UsuarioRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<UsuarioAuthResultDto?> ObtenerUsuarioPorLoginAsync(string login, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

                var parameters = new DynamicParameters();
                parameters.Add("@Login", login, DbType.String, size: 20);

                var commandDefinition = new CommandDefinition(
                    commandText: "SELECT * FROM public.up_sel_usuario_validar_xlogin(@Login)",
                    parameters: parameters,
                    commandType: CommandType.Text,
                    cancellationToken: cancellationToken
                );

                return await connection.QueryFirstOrDefaultAsync<UsuarioAuthResultDto>(commandDefinition);
            }
            catch (NpgsqlException ex)
            {
                // Aislamiento: Se enmascara la excepción nativa.
                throw new DatabaseOperationException("Ocurrió un error a nivel de base de datos al validar el usuario.", ex);
            }
        }
    }
}


//using System.Data;
//using System.Threading;
//using System.Threading.Tasks;
//using Academic360.Application.Auth.Commands.Login;
//using Academic360.Application.Auth.Interfaces;
//using Dapper;

//namespace Academic360.Infrastructure.Repositories
//{
//    public class UsuarioRepository : IUsuarioRepository
//    {
//        private readonly IDbConnection _connection;

//        public UsuarioRepository(IDbConnection connection)
//        {
//            _connection = connection;
//        }

//        // CORRECCIÓN: La firma ahora coincide con la interfaz actualizada (sin la clave)
//        public async Task<UsuarioAuthResultDto?> ObtenerUsuarioPorLoginAsync(string login, CancellationToken cancellationToken = default)
//        {
//            var parameters = new DynamicParameters();
//            parameters.Add("@Login", login, DbType.String, size: 20);

//            // Nota histórica: Ya no enviamos la clave a SQL Server (Mitigación OWASP A02).
//            // parameters.Add("@Clave", clave, DbType.String, size: 100);

//            // CORRECCIÓN DE RENDIMIENTO/SEGURIDAD: 
//            // Usamos CommandDefinition para poder inyectar el CancellationToken. 
//            // Si hay un ataque de denegación de servicio (DoS) o el cliente se desconecta, 
//            // Dapper cancelará la consulta en SQL Server liberando recursos inmediatamente.
//            var commandDefinition = new CommandDefinition(
//                commandText: "up_sel_usuario_validar_xlogin", // Mantenemos el nombre original del SP que alteraste
//                parameters: parameters,
//                commandType: CommandType.StoredProcedure,
//                cancellationToken: cancellationToken
//            );

//            return await _connection.QueryFirstOrDefaultAsync<UsuarioAuthResultDto>(commandDefinition);
//        }
//    }
//}