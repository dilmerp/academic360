using Academic360.Application.Alumnos.Interfaces;
using MediatR;

namespace Academic360.Application.Alumnos.Commands.ActualizarEstadoMatricula;

public class ActualizarEstadoMatriculaCommandHandler : IRequestHandler<ActualizarEstadoMatriculaCommand>
{
    private readonly IAlumnoMatriculaRepository _alumnoMatriculaRepository;

    public ActualizarEstadoMatriculaCommandHandler(IAlumnoMatriculaRepository alumnoMatriculaRepository)
    {
        _alumnoMatriculaRepository = alumnoMatriculaRepository;
    }

    public async Task Handle(ActualizarEstadoMatriculaCommand request, CancellationToken cancellationToken)
    {
        // Casteamos explícitamente a int (32 bits) para que coincidan 
        // con los parámetros INT del Stored Procedure de SQL Server
        await _alumnoMatriculaRepository.ActualizarEstadoMatriculaAsync(
            (int)request.IdAlumno,
            (int)request.IdRegistro,
            (int)request.IdMatricula,
            request.EsMatricula);
    }
}