using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Academic360.Application.Common.Interfaces;
using Academic360.Application.MatriculasCursosLibres.DTOs;
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
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT fn_ins_matricula_curso_libre_json(@JsonData::jsonb);";
            var parameters = new { JsonData = matriculaJson };
            var idMatricula = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return idMatricula;
        }

        public async Task<IEnumerable<MatriculaLibreListDto>> ObtenerListadoAsync(string filtro)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var sql = @"
                SELECT 
                    am.idregistro AS ""IdRegistro"",
                    am.idmatricula AS ""IdMatricula"",
                    a.codigo AS ""Codigo"",
                    (a.paterno || ' ' || COALESCE(a.materno, '') || ' ' || a.nombres) AS ""Alumno"",
                    COALESCE(c.cursonombre, p.promocionnombre) AS ""Curso"",
                    TO_CHAR(am.matriculafecha, 'DD/MM/YYYY') AS ""Fecha""
                FROM public.alumnomatricula am
                INNER JOIN public.actor a ON am.idalumno = a.idactor
                INNER JOIN public.promocion p ON am.idpromocion = p.idpromocion
                LEFT JOIN public.alumnocurso ac ON am.idalumno = ac.idalumno AND am.idregistro = ac.idregistro
                LEFT JOIN public.curso c ON ac.idcurso = c.idcurso
                WHERE EXTRACT(YEAR FROM am.matriculafecha) = 2026
                  AND (@Filtro = '' OR a.codigo ILIKE '%' || @Filtro || '%' OR (a.paterno || ' ' || a.nombres) ILIKE '%' || @Filtro || '%')
                ORDER BY am.matriculafecha DESC;
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@Filtro", filtro ?? string.Empty, DbType.String);

            var resultado = await connection.QueryAsync<MatriculaLibreListDto>(sql, parameters);
            return resultado;
        }
    }
}