using Academic360.Application.Common.Models;
using Academic360.Application.Alumnos.Interfaces;
using MediatR;
using Academic360.Application.Matriculas.Carrera.DTOs;

namespace Academic360.Application.Matriculas.Carrera.Queries.GetMatriculasByPromocion;

public class ObtenerMatriculasPorPromocionQuery : IRequestHandler<ObtenerMatriculasPorPromocionQueryHandler, PagedResult<MatriculaCarreraListadoDto>>
{
    private readonly IAlumnoMatriculaRepository _repository;

    public ObtenerMatriculasPorPromocionQuery(IAlumnoMatriculaRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<MatriculaCarreraListadoDto>> Handle(ObtenerMatriculasPorPromocionQueryHandler request, CancellationToken cancellationToken)
    {
        // Pasamos explícitamente null a los parámetros de grupo y sección que aún no usamos
        var matriculas = await _repository.ObtenerMatriculasAsync(
            request.IdPromocion,
            request.Estado,
            request.SearchTerm,
            null,
            null);

        var totalCount = matriculas.Count();

        var pagedItems = matriculas
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedResult<MatriculaCarreraListadoDto>
        {
            Items = pagedItems,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}