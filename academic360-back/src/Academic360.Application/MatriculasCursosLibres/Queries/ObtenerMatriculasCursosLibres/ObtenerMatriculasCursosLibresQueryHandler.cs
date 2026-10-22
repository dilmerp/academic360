using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Academic360.Application.MatriculasCursosLibres.DTOs;
using Academic360.Application.MatriculasCursosLibres.Interfaces;

namespace Academic360.Application.MatriculasCursosLibres.Queries.ObtenerMatriculasCursosLibres
{
    public class ObtenerMatriculasCursosLibresQueryHandler : IRequestHandler<ObtenerMatriculasCursosLibresQuery, IEnumerable<MatriculaLibreListDto>>
    {
        private readonly IMatriculaCursoLibreRepository _repository;

        public ObtenerMatriculasCursosLibresQueryHandler(IMatriculaCursoLibreRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MatriculaLibreListDto>> Handle(ObtenerMatriculasCursosLibresQuery request, CancellationToken cancellationToken)
        {
            return await _repository.ObtenerListadoAsync(request.Filtro);
        }
    }
}