using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Auth.Commands.Login;
using Academic360.Application.Auth.Interfaces;
using Dapper;

namespace Academic360.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _connection;

        public UsuarioRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        // CORRECCIÓN: La firma ahora coincide con la interfaz actualizada (sin la clave)
        public async Task<UsuarioAuthResultDto?> ObtenerUsuarioPorLoginAsync(string login, CancellationToken cancellationToken = default)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Login", login, DbType.String, size: 20);

            // Nota histórica: Ya no enviamos la clave a SQL Server (Mitigación OWASP A02).
            // parameters.Add("@Clave", clave, DbType.String, size: 100);

            // CORRECCIÓN DE RENDIMIENTO/SEGURIDAD: 
            // Usamos CommandDefinition para poder inyectar el CancellationToken. 
            // Si hay un ataque de denegación de servicio (DoS) o el cliente se desconecta, 
            // Dapper cancelará la consulta en SQL Server liberando recursos inmediatamente.
            var commandDefinition = new CommandDefinition(
                commandText: "up_sel_usuario_validar_xlogin", // Mantenemos el nombre original del SP que alteraste
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            return await _connection.QueryFirstOrDefaultAsync<UsuarioAuthResultDto>(commandDefinition);
        }
    }
}