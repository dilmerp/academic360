using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Academic360.Application.Common.Interfaces;
using Academic360.Application.Matriculas.CursosLibres.DTOs;
using Academic360.Application.Matriculas.CursosLibres.Interfaces;

namespace Academic360.Infrastructure.Repositories
{
    public class MatriculaCursoLibreRepository : IMatriculaCursosLibresRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MatriculaCursoLibreRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> RegistrarMatriculaCompletaAsync(string matriculaJson)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT fn_ins_matricula_curso_libre_json(@JsonData::jsonb);";
            var parameters = new { JsonData = matriculaJson };
            var idMatricula = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return idMatricula;
        }

        public async Task<IEnumerable<MatriculaCursosLibresListadoDto>> ObtenerListadoAsync(string filtro)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            // Se reemplaza la consulta inyectada por el llamado a la función PostgreSQL
            var sql = "SELECT * FROM public.up_sel_matriculacursolibre_listado(@Filtro)";

            var parameters = new DynamicParameters();
            parameters.Add("@Filtro", filtro ?? string.Empty, DbType.String);

            var resultado = await connection.QueryAsync<MatriculaCursosLibresListadoDto>(sql, parameters);
            return resultado;
        }

   
    }
}