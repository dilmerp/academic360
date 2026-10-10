using MediatR;

namespace Academic360.Application.Alumnos.Commands.EliminarAlumno;

public class EliminarAlumnoCommand : IRequest<bool>
{
    public long IdActor { get; set; }
}