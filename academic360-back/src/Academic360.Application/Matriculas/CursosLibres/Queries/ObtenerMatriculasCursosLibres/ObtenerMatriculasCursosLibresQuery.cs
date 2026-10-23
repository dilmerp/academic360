using System.Collections.Generic;
using MediatR;
using Academic360.Application.Matriculas.CursosLibres.DTOs;

namespace Academic360.Application.Matriculas.CursosLibres.Queries.ObtenerMatriculasCursosLibres
{
    public record ObtenerMatriculasCursosLibresQuery(string Filtro) : IRequest<IEnumerable<MatriculaCursosLibresListadoDto>>;
}