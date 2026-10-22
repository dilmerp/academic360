using MediatR;
using Academic360.Application.Alumnos.DTOs;
using System.Collections.Generic;

namespace Academic360.Application.Alumnos.Queries.BuscarAlumnos
{
    public class BuscarAlumnosQuery : IRequest<IEnumerable<AlumnoBusquedaDto>>
    {
        public string Filtro { get; set; } = string.Empty;
    }
}