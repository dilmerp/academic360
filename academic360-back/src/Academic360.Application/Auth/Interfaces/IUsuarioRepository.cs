using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Auth.Commands.Login;

namespace Academic360.Application.Auth.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<UsuarioAuthResultDto?> ObtenerUsuarioPorLoginAsync(string login, CancellationToken cancellationToken = default);
    }
}