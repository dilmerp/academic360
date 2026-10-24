using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Auth.Commands.Login;
using Academic360.Application.Auth.Interfaces;
using Academic360.Application.Common.Exceptions;
using Academic360.Application.Common.Interfaces;
using Dapper;
using Npgsql; 

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
                throw new DatabaseOperationException("Ocurrió un error a nivel de base de datos al validar el usuario.", ex);
            }
        }
    }
}

