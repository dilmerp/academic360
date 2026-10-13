using Academic360.Application.Catalogos.Interfaces;
using Academic360.Application.Common.Interfaces;
using Dapper;
using System.Data;

namespace Academic360.Infrastructure.Repositories;

public class CatalogoRepository : ICatalogoRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CatalogoRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<SubTipoDto>> ObtenerSubTiposActivosAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        return await connection.QueryAsync<SubTipoDto>(
            "SELECT * FROM public.up_sel_productosubtipo_activos()",
            commandType: CommandType.Text,
            commandTimeout : 60);
    }

    public async Task<IEnumerable<ProductoDto>> ObtenerProductosPorSubTipoAsync(int idTipo, int idSubTipo)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@idtipo", idTipo, DbType.Int32);
        parameters.Add("@idsubtipo", idSubTipo, DbType.Int32);

        return await connection.QueryAsync<ProductoDto>(
            "SELECT * FROM public.up_sel_producto_xidtipoxidsubtipo(@idtipo, @idsubtipo)",
            parameters,
            commandType: CommandType.Text, commandTimeout: 60);
    }

    public async Task<IEnumerable<PromocionDto>> ObtenerPromocionesPorProductoAsync(int idProducto)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@idproducto", idProducto, DbType.Int32);

        return await connection.QueryAsync<PromocionDto>(
            "SELECT * FROM public.up_sel_promocion_xidproducto(@idproducto)",
            parameters,
            commandType: CommandType.Text, commandTimeout: 60);
    }
}

//using Academic360.Application.Catalogos.Interfaces;
//using Academic360.Application.Common.Interfaces;
//using Dapper;
//using System.Data;

//namespace Academic360.Infrastructure.Repositories;

//public class CatalogoRepository : ICatalogoRepository
//{
//    private readonly IDbConnectionFactory _connectionFactory;

//    public CatalogoRepository(IDbConnectionFactory connectionFactory)
//    {
//        _connectionFactory = connectionFactory;
//    }

//    public async Task<IEnumerable<SubTipoDto>> ObtenerSubTiposActivosAsync()
//    {
//        using var connection = await _connectionFactory.CreateConnectionAsync();

//        // Asegúrate de que este sea el nombre exacto de tu SP en SQL Server
//        return await connection.QueryAsync<SubTipoDto>(
//            "up_sel_productosubtipo_activos",
//            commandType: CommandType.StoredProcedure);
//    }

//    public async Task<IEnumerable<ProductoDto>> ObtenerProductosPorSubTipoAsync(int idTipo, int idSubTipo)
//    {
//        using var connection = await _connectionFactory.CreateConnectionAsync();

//        var parameters = new DynamicParameters();
//        // Ojo: Asegúrate de que los nombres de los parámetros (@idtipo, @idsubtipo) 
//        // coincidan con los que espera tu SP real.
//        parameters.Add("@idtipo", idTipo, DbType.Int32);
//        parameters.Add("@idsubtipo", idSubTipo, DbType.Int32);

//        return await connection.QueryAsync<ProductoDto>(
//            "up_sel_producto_xidtipoxidsubtipo", // <--- Reemplaza el nombre aquí
//            parameters,
//            commandType: CommandType.StoredProcedure);
//    }

//    public async Task<IEnumerable<PromocionDto>> ObtenerPromocionesPorProductoAsync(int idProducto)
//    {
//        using var connection = await _connectionFactory.CreateConnectionAsync();

//        var parameters = new DynamicParameters();
//        parameters.Add("@idproducto", idProducto, DbType.Int32);

//        return await connection.QueryAsync<PromocionDto>(
//            "up_sel_promocion_xidproducto",
//            parameters,
//            commandType: CommandType.StoredProcedure);
//    }
//}