using System.Collections.Generic;
using MediatR;
using Academic360.Application.MatriculasCursosLibres.DTOs;

namespace Academic360.Application.MatriculasCursosLibres.Queries.ObtenerMatriculasCursosLibres
{
    public record ObtenerMatriculasCursosLibresQuery(string Filtro) : IRequest<IEnumerable<MatriculaLibreListDto>>;
}