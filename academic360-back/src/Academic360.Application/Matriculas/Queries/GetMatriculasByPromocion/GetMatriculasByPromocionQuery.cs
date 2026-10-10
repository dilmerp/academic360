using Academic360.Application.Common.Models;
using Academic360.Application.Matriculas.DTOs;
using Academic360.Application.Matriculas.Enums;
using MediatR;

namespace Academic360.Application.Matriculas.Queries.GetMatriculasByPromocion;

// Aquí está la clave: heredar de IRequest indicando el tipo de retorno exacto
public class GetMatriculasByPromocionQuery : IRequest<PagedResult<AlumnoMatriculaListadoDto>>
{
    public int IdPromocion { get; set; }
    public FiltroMatriculaEstado Estado { get; set; }

    // Paginación y búsqueda global
    public string SearchTerm { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}