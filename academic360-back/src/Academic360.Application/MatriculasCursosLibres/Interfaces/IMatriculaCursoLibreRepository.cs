using System.Threading.Tasks;

namespace Academic360.Application.MatriculasCursosLibres.Interfaces
{
    public interface IMatriculaCursoLibreRepository
    {
        // Recibiremos todo el objeto serializado en JSON para inyectarlo a Postgres
        Task<int> RegistrarMatriculaCompletaAsync(string matriculaJson);
    }
}