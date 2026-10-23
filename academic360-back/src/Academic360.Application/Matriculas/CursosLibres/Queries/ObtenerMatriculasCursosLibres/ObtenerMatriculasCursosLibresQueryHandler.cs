using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Academic360.Application.Matriculas.CursosLibres.DTOs;
using Academic360.Application.Matriculas.CursosLibres.Interfaces;

namespace Academic360.Application.Matriculas.CursosLibres.Queries.ObtenerMatriculasCursosLibres
{
    public class ObtenerMatriculasCursosLibresQueryHandler : IRequestHandler<ObtenerMatriculasCursosLibresQuery, IEnumerable<MatriculaCursosLibresListadoDto>>
    {
        private readonly IMatriculaCursosLibresRepository _repository;

        public ObtenerMatriculasCursosLibresQueryHandler(IMatriculaCursosLibresRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MatriculaCursosLibresListadoDto>> Handle(ObtenerMatriculasCursosLibresQuery request, CancellationToken cancellationToken)
        {
            return await _repository.ObtenerListadoAsync(request.Filtro);
        }
    }
}