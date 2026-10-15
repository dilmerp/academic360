using System.Threading.Tasks;
using Dapper;
using Academic360.Application.Common.Interfaces;
using Academic360.Application.MatriculasCursosLibres.Interfaces;

namespace Academic360.Infrastructure.Repositories
{
    public class MatriculaCursoLibreRepository : IMatriculaCursoLibreRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MatriculaCursoLibreRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> RegistrarMatriculaCompletaAsync(string matriculaJson)
        {
            //using var connection = _connectionFactory.CreateConnection();
            using var connection = await _connectionFactory.CreateConnectionAsync();

            // Dapper llamando a Postgres. El casteo ::jsonb es clave para que Postgres lo parsee nativamente.
            var sql = "SELECT fn_ins_matricula_curso_libre_json(@JsonData::jsonb);";

            var parameters = new { JsonData = matriculaJson };

            // ExecuteScalar nos devuelve el ID de la matrícula creada por la función
            var idMatricula = await connection.ExecuteScalarAsync<int>(sql, parameters);

            return idMatricula;
        }
    }
}