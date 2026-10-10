using Academic360.Application.Alumnos.Interfaces;
using Academic360.Application.Common.Interfaces;
using Academic360.Application.Matriculas.DTOs;
using Academic360.Application.Matriculas.Enums;
using Dapper;
using System.Data;

namespace Academic360.Infrastructure.Repositories;

public class AlumnoMatriculaRepository : IAlumnoMatriculaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AlumnoMatriculaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<AlumnoMatriculaListadoDto>> ObtenerMatriculasAsync(int idPromocion, FiltroMatriculaEstado estado, string? searchTerm, int? idGrupo, int? idSeccion)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@idpromocion", idPromocion, DbType.Int32);
        parameters.Add("@estado", (int)estado, DbType.Int32);

        if (idGrupo.HasValue)
        {
            parameters.Add("@idgrupo", idGrupo.Value, DbType.Int32);
        }

        if (idSeccion.HasValue)
        {
            parameters.Add("@idseccion", idSeccion.Value, DbType.Int32);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            parameters.Add("@nombrecompleto", searchTerm, DbType.String);
        }

        var result = await connection.QueryAsync<AlumnoMatriculaListadoDto>(
            "up_sel_alumnomatricula_listados_dinamico",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<bool> ActualizarEstadoMatriculaAsync(int idAlumno, int idRegistro, int idMatricula, bool esMatricula)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@idalumno", idAlumno, DbType.Int32);
        parameters.Add("@idregistro", idRegistro, DbType.Int32);
        parameters.Add("@idmatricula", idMatricula, DbType.Int32);
        parameters.Add("@esmatricula", esMatricula, DbType.Boolean);

        await connection.ExecuteAsync(
            "up_upd_alumnomatricula_esmatricula",
            parameters,
            commandType: CommandType.StoredProcedure);

        return true;
    }
}