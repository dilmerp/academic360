using Academic360.Application.Alumnos.DTOs;
using MediatR;

namespace Academic360.Application.Alumnos.Queries.ObtenerAlumnoPorId;

public class ObtenerAlumnoPorIdQuery : IRequest<AlumnoResponseDto>
{
    public long IdActor { get; }

    public ObtenerAlumnoPorIdQuery(long idActor)
    {
        IdActor = idActor;
    }
}