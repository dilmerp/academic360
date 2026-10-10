using Academic360.Application.Common.Models;
using Academic360.Application.Alumnos.Interfaces;
using Academic360.Application.Matriculas.DTOs;
using MediatR;

namespace Academic360.Application.Matriculas.Queries.GetMatriculasByPromocion;

public class GetMatriculasByPromocionQueryHandler : IRequestHandler<GetMatriculasByPromocionQuery, PagedResult<AlumnoMatriculaListadoDto>>
{
    private readonly IAlumnoMatriculaRepository _repository;

    public GetMatriculasByPromocionQueryHandler(IAlumnoMatriculaRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AlumnoMatriculaListadoDto>> Handle(GetMatriculasByPromocionQuery request, CancellationToken cancellationToken)
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

        return new PagedResult<AlumnoMatriculaListadoDto>
        {
            Items = pagedItems,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}