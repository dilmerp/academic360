using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Actores.Interfaces;
using Dapper;

namespace Academic360.Infrastructure.Repositories;

public class ActorFotoRepository : IActorFotoRepository
{
    public async Task ActualizarFotoAsync(long idActor, byte[] foto, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@idactor", idActor, DbType.Int32);
        parameters.Add("@profesorfoto", foto, DbType.Binary);

        var command = new CommandDefinition(
            commandText: "SELECT public.up_upd_actorfoto(@idactor, @profesorfoto)",
            parameters: parameters,
            transaction: transaction,
            commandType: CommandType.Text,
            cancellationToken: cancellationToken
        );

        await transaction.Connection!.ExecuteAsync(command);
    }
}

//using System.Data;
//using Academic360.Application.Actores.Interfaces;
//using Dapper;

//namespace Academic360.Infrastructure.Repositories;

//public class ActorFotoRepository : IActorFotoRepository
//{
//    public async Task ActualizarFotoAsync(long idActor, byte[] foto, IDbTransaction transaction, CancellationToken cancellationToken = default)
//    {
//        var parameters = new DynamicParameters();
//        parameters.Add("@idactor", idActor, DbType.Int32);
//        parameters.Add("@profesorfoto", foto, DbType.Binary);

//        await transaction.Connection.ExecuteAsync(
//            "up_upd_actorfoto",
//            parameters,
//            transaction: transaction,
//            commandType: CommandType.StoredProcedure);
//    }
//}