using System;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Alumnos.Interfaces;
using MediatR;

namespace Academic360.Application.Alumnos.Commands.EliminarAlumno;

public class EliminarAlumnoCommandHandler : IRequestHandler<EliminarAlumnoCommand, bool>
{
    private readonly IAlumnoRepository _alumnoRepository;

    public EliminarAlumnoCommandHandler(IAlumnoRepository alumnoRepository)
    {
        _alumnoRepository = alumnoRepository;
    }

    public async Task<bool> Handle(EliminarAlumnoCommand request, CancellationToken cancellationToken)
    {
        var retval = await _alumnoRepository.EliminarAlumnoAsync(request.IdActor, cancellationToken);

        // Si el SP devuelve un valor entre 1 y 5, significa que el alumno tiene registros dependientes
        if (retval > 0)
        {
            throw new InvalidOperationException("No se puede eliminar el alumno porque está siendo usado en matrículas, registros o notas.");
        }

        return true;
    }
}