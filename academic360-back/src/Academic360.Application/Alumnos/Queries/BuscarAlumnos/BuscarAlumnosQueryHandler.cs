using MediatR;
using Academic360.Application.Alumnos.DTOs;
using Academic360.Application.Alumnos.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Academic360.Application.Alumnos.Queries.BuscarAlumnos
{
    public class BuscarAlumnosQueryHandler : IRequestHandler<BuscarAlumnosQuery, IEnumerable<AlumnoBusquedaDto>>
    {
        private readonly IAlumnoRepository _alumnoRepository;

        public BuscarAlumnosQueryHandler(IAlumnoRepository alumnoRepository)
        {
            _alumnoRepository = alumnoRepository;
        }

        public async Task<IEnumerable<AlumnoBusquedaDto>> Handle(BuscarAlumnosQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Filtro) || request.Filtro.Length < 3)
            {
                return new List<AlumnoBusquedaDto>();
            }

            return await _alumnoRepository.BuscarAlumnosAsync(request.Filtro);
        }
    }
}