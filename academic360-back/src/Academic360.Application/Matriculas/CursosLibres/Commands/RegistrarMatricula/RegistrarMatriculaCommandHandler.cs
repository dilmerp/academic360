using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Academic360.Application.Matriculas.CursosLibres.Interfaces;

namespace Academic360.Application.Matriculas.CursosLibres.Commands.CrearMatricula
{
    public class RegistrarMatriculaCommandHandler : IRequestHandler<RegistrarMatriculaCommand, int>
    {
        private readonly IMatriculaCursosLibresRepository _repository;

        public RegistrarMatriculaCommandHandler(IMatriculaCursosLibresRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(RegistrarMatriculaCommand request, CancellationToken cancellationToken)
        {
            // Serializar el DTO a JSON (PostgreSQL JSONB)
            var matriculaJson = JsonSerializer.Serialize(request.Matricula);

            var idMatricula = await _repository.RegistrarMatriculaCompletaAsync(matriculaJson);

            return idMatricula;
        }
    }
}