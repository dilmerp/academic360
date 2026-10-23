using MediatR;
using Academic360.Application.Matriculas.CursosLibres.DTOs;

namespace Academic360.Application.Matriculas.CursosLibres.Commands.CrearMatricula
{
    public class RegistrarMatriculaCommand : IRequest<int> // Retorna el ID de la matrícula generada
    {
        public RegistrarMatriculaRequestDto Matricula { get; set; }

        public RegistrarMatriculaCommand(RegistrarMatriculaRequestDto matricula)
        {
            Matricula = matricula;
        }
    }
}