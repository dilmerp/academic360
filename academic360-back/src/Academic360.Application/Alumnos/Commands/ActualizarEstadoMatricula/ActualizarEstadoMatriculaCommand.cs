using MediatR;

namespace Academic360.Application.Alumnos.Commands.ActualizarEstadoMatricula;

public class ActualizarEstadoMatriculaCommand : IRequest
{
    public long IdAlumno { get; }
    public long IdRegistro { get; }
    public long IdMatricula { get; }
    public bool EsMatricula { get; }

    public ActualizarEstadoMatriculaCommand(long idAlumno, long idRegistro, long idMatricula, bool esMatricula)
    {
        IdAlumno = idAlumno;
        IdRegistro = idRegistro;
        IdMatricula = idMatricula;
        EsMatricula = esMatricula;
    }
}