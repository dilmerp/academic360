using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Auth.Commands.Login;

namespace Academic360.Application.Auth.Interfaces
{
    public interface IUsuarioRepository
    {
        // CORRECCIÓN: Eliminamos el parámetro 'clave'. 
        // El repositorio ahora tiene la única responsabilidad de traer los datos del usuario.
        Task<UsuarioAuthResultDto?> ObtenerUsuarioPorLoginAsync(string login, CancellationToken cancellationToken = default);
    }
}