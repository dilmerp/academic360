using MediatR;

namespace Academic360.Application.Matriculas.Carrera.Commands.ActualizarEstadoMatricula;

public class ActualizarEstadoMatriculaCommand : IRequest<bool>
{
    public int IdAlumno { get; set; }
    public int IdRegistro { get; set; }
    public int IdMatricula { get; set; }
    public bool EsMatricula { get; set; } // true (1) para Matricular, false (0) para Deshacer
}