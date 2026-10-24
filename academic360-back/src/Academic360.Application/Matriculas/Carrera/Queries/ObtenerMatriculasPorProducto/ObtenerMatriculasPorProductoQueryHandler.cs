using Academic360.Application.Common.Models;
using Academic360.Application.Matriculas.Carrera.DTOs;
using Academic360.Application.Matriculas.Carrera.Enums;
using MediatR;

namespace Academic360.Application.Matriculas.Carrera.Queries.ObtenerMatriculasPorProducto;

public class ObtenerMatriculasPorProductoQueryHandler : IRequest<PagedResult<MatriculaCarreraListadoDto>>
{
    public int IdProducto { get; set; }
    public FiltroMatriculaEstado Estado { get; set; }

    public string SearchTerm { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}