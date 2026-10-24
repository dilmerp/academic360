using Academic360.Application.Matriculas.CursosLibres.DTOs;
using System.Threading.Tasks;

namespace Academic360.Application.Matriculas.CursosLibres.Interfaces
{
    public interface IMatriculaCursosLibresRepository
    {
        Task<int> RegistrarMatriculaCompletaAsync(string matriculaJson);
        Task<IEnumerable<MatriculaCursosLibresListadoDto>> ObtenerListadoAsync(string filtro);
    }
}