using Academic360.Application.Matriculas.Carrera.Commands.RegistrarMatricula;
using Academic360.Application.Matriculas.Carrera.DTOs;
using Academic360.Application.Matriculas.Carrera.Enums;

namespace Academic360.Application.Alumnos.Interfaces;

public interface IAlumnoMatriculaRepository
{
    // Firma limpia sin valores por defecto para evitar confusiones del compilador
   // Task<IEnumerable<MatriculaCarreraListadoDto>> ObtenerMatriculasAsync(int idPromocion, FiltroMatriculaEstado estado, string? searchTerm, int? idGrupo, int? idSeccion);
    // Firma limpia para obtener matrículas por promoción
    Task<IEnumerable<MatriculaCarreraListadoDto>> ObtenerMatriculasAsync(int idPromocion, FiltroMatriculaEstado estado, string? searchTerm, int? idGrupo, int? idSeccion);

    // Nueva firma para obtener matrículas directamente por producto (carrera)
    Task<IEnumerable<MatriculaCarreraListadoDto>> ObtenerMatriculasPorProductoAsync(int idProducto, FiltroMatriculaEstado estado, string? searchTerm, int? idGrupo, int? idSeccion);

    Task<bool> ActualizarEstadoMatriculaAsync(int idAlumno, int idRegistro, int idMatricula, bool esMatricula);

    Task<string> InsertarOActualizarActorAsync(RegistrarMatriculaCommand request, string contrasena, CancellationToken cancellationToken);
    Task<int> InsertarRegistroAsync(int idAlumno, RegistrarMatriculaCommand request, CancellationToken cancellationToken);
    Task<int> InsertarMatriculaAsync(int idAlumno, int idRegistro, RegistrarMatriculaCommand request, CancellationToken cancellationToken);
    Task<int> InsertarAlumnoCursoAsync(int idAlumno, int idRegistro, int idMatricula, RegistrarMatriculaCommand request, CursoMatriculaDto curso, CancellationToken cancellationToken);
    Task<int> InsertarAlumnoCurriculaAsync(int idAlumno, int idRegistro, CursoMatriculaDto curso, int usuarioCreacion, CancellationToken cancellationToken);
    Task InsertarAlumnoCurriculaCursoAsync(int idAlumno, int idRegistro, int idCurricula, int idAlumnoCurso, CancellationToken cancellationToken);
    Task InsertarProgramacionAsync(int idAlumno, int idMovimiento, CuotaProgramacionDto cuota, RegistrarMatriculaCommand request, CancellationToken cancellationToken);
}