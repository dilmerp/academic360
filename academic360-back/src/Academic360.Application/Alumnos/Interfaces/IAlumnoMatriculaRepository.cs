using Academic360.Application.Matriculas.DTOs;
using Academic360.Application.Matriculas.Enums;

namespace Academic360.Application.Alumnos.Interfaces;

public interface IAlumnoMatriculaRepository
{
    // Firma limpia sin valores por defecto para evitar confusiones del compilador
    Task<IEnumerable<AlumnoMatriculaListadoDto>> ObtenerMatriculasAsync(int idPromocion, FiltroMatriculaEstado estado, string? searchTerm, int? idGrupo, int? idSeccion);

    Task<bool> ActualizarEstadoMatriculaAsync(int idAlumno, int idRegistro, int idMatricula, bool esMatricula);
}