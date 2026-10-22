using MediatR;
using System.Transactions;
using Academic360.Application.Alumnos.Interfaces;

namespace Academic360.Application.Matriculas.Commands.RegistrarMatricula
{
    public class RegistrarMatriculaCommandHandler : IRequestHandler<RegistrarMatriculaCommand, string>
    {
        private readonly IAlumnoMatriculaRepository _repository;

        public RegistrarMatriculaCommandHandler(IAlumnoMatriculaRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(RegistrarMatriculaCommand request, CancellationToken cancellationToken)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                string contrasenaGenerada = Guid.NewGuid().ToString().Substring(0, 8);

                string resultadoActor = await _repository.InsertarOActualizarActorAsync(request, contrasenaGenerada, cancellationToken);
                int idAlumno = int.Parse(resultadoActor.Split('|')[0]);

                int idRegistro = await _repository.InsertarRegistroAsync(idAlumno, request, cancellationToken);

                int idMatricula = await _repository.InsertarMatriculaAsync(idAlumno, idRegistro, request, cancellationToken);

                foreach (var curso in request.Cursos)
                {
                    int idAlumnoCurso = await _repository.InsertarAlumnoCursoAsync(idAlumno, idRegistro, idMatricula, request, curso, cancellationToken);
                    int idAlumnoCurricula = await _repository.InsertarAlumnoCurriculaAsync(idAlumno, idRegistro, curso, request.UsuarioCreacion, cancellationToken);

                    await _repository.InsertarAlumnoCurriculaCursoAsync(idAlumno, idRegistro, idAlumnoCurricula, idAlumnoCurso, cancellationToken);
                }

                int idMovimiento = request.IdMovimiento > 0 ? request.IdMovimiento : 1;

                foreach (var cuota in request.Cuotas)
                {
                    await _repository.InsertarProgramacionAsync(idAlumno, idMovimiento, cuota, request, cancellationToken);
                }

                transaction.Complete();
                return resultadoActor;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al grabar la matrícula: {ex.Message}", ex);
            }
        }
    }
}