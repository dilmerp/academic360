using Academic360.Application.Matriculas.CursosLibres.DTOs;
using System.Threading.Tasks;

namespace Academic360.Application.Matriculas.CursosLibres.Interfaces
{
    public interface IMatriculaCursosLibresRepository
    {
        // Recibiremos todo el objeto serializado en JSON para inyectarlo a Postgres
        Task<int> RegistrarMatriculaCompletaAsync(string matriculaJson);
        Task<IEnumerable<MatriculaCursosLibresListadoDto>> ObtenerListadoAsync(string filtro);
    }
}