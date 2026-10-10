using Academic360.Application.Alumnos.DTOs;
using MediatR;

namespace Academic360.Application.Alumnos.Commands.CrearAlumno;

public class CrearAlumnoCommand : IRequest<long>
{
    public CrearAlumnoRequestDto Datos { get; }

    public CrearAlumnoCommand(CrearAlumnoRequestDto datos)
    {
        Datos = datos;
    }
}