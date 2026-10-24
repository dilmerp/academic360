using Academic360.Application.Common.Models;
using MediatR;
using Academic360.Application.Matriculas.Carrera.DTOs;
using Academic360.Application.Matriculas.Carrera.Interfaces;

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