using System.Collections.Generic;
using Academic360.Application.Common.Models;
using MediatR;

namespace Academic360.Application.Alumnos.Queries.ObtenerAlumnos;

public class ObtenerAlumnosPorCriterioQuery : IRequest<PagedResult<AlumnoListadoDto>>
{
    public string FiltroGlobal { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}