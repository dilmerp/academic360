using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Academic360.Application.MatriculasCursosLibres.Interfaces;

namespace Academic360.Application.MatriculasCursosLibres.Commands.CrearMatricula
{
    public class CrearMatriculaCommandHandler : IRequestHandler<CrearMatriculaCommand, int>
    {
        private readonly IMatriculaCursoLibreRepository _repository;

        public CrearMatriculaCommandHandler(IMatriculaCursoLibreRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CrearMatriculaCommand request, CancellationToken cancellationToken)
        {
            // Serializamos el DTO a JSON para aprovechar la potencia de PostgreSQL JSONB
            var matriculaJson = JsonSerializer.Serialize(request.Matricula);

            // Llamamos a la BD una sola vez
            var idMatricula = await _repository.RegistrarMatriculaCompletaAsync(matriculaJson);

            return idMatricula;
        }
    }
}