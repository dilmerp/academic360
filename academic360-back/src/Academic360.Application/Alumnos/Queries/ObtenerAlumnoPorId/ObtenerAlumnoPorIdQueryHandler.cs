using Academic360.Application.Alumnos.DTOs;
using Academic360.Application.Alumnos.Interfaces;
using MediatR;

namespace Academic360.Application.Alumnos.Queries.ObtenerAlumnoPorId;

public class ObtenerAlumnoPorIdQueryHandler : IRequestHandler<ObtenerAlumnoPorIdQuery, AlumnoResponseDto>
{
    private readonly IAlumnoRepository _alumnoRepository;

    public ObtenerAlumnoPorIdQueryHandler(IAlumnoRepository alumnoRepository)
    {
        _alumnoRepository = alumnoRepository;
    }

    public async Task<AlumnoResponseDto> Handle(ObtenerAlumnoPorIdQuery request, CancellationToken cancellationToken)
    {
        var alumno = await _alumnoRepository.ObtenerPorIdAsync(request.IdActor, cancellationToken);

        if (alumno == null)
        {
            throw new KeyNotFoundException($"No se encontró ningún alumno con el Id {request.IdActor}.");
        }

        return alumno;
    }
}