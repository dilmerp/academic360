using System.Data;

namespace Academic360.Application.Actores.Interfaces;

public interface IActorFotoRepository
{
    Task ActualizarFotoAsync(long idActor, byte[] foto, IDbTransaction transaction, CancellationToken cancellationToken = default);
}