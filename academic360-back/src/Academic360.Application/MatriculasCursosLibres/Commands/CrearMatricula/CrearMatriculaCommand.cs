using MediatR;
using Academic360.Application.MatriculasCursosLibres.DTOs;

namespace Academic360.Application.MatriculasCursosLibres.Commands.CrearMatricula
{
    public class CrearMatriculaCommand : IRequest<int> // Retorna el ID de la matrícula generada
    {
        public CrearMatriculaRequestDto Matricula { get; set; }

        public CrearMatriculaCommand(CrearMatriculaRequestDto matricula)
        {
            Matricula = matricula;
        }
    }
}