using Academic360.Application.Catalogos.Interfaces;
using Academic360.Application.Common.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Academic360.Infrastructure.Repositories
{
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
                commandTimeout: 60);
        }

        public async Task<IEnumerable<TenorDto>> ObtenerTenorActivosAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<TenorDto>(
                "SELECT * FROM public.up_sel_tenor_xactivo()",
                commandType: CommandType.Text,
                commandTimeout: 60);
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
                commandType: CommandType.Text,
                commandTimeout: 60);
        }

        public async Task<IEnumerable<PromocionDto>> ObtenerPromocionesPorProductoAsync(int idProducto)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var parameters = new DynamicParameters();
            parameters.Add("@idproducto", idProducto, DbType.Int32);

            return await connection.QueryAsync<PromocionDto>(
                "SELECT * FROM public.up_sel_promocion_xidproducto(@idproducto)",
                parameters,
                commandType: CommandType.Text,
                commandTimeout: 60);
        }

        public async Task<IEnumerable<GrupoDto>> ObtenerGruposPorPromocionAsync(int idPromocion)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var parameters = new DynamicParameters();
            parameters.Add("@idpromocion", idPromocion, DbType.Int32);

            return await connection.QueryAsync<GrupoDto>(
                "SELECT * FROM public.up_sel_grupo_xidpromocion(@idpromocion)",
                parameters,
                commandType: CommandType.Text,
                commandTimeout: 60);
        }

        public async Task<IEnumerable<MedioDto>> ObtenerMediosActivosAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MedioDto>(
                "SELECT * FROM public.up_sel_mediocomunicacion_xactivo()",
                commandType: CommandType.Text,
                commandTimeout: 60);
        }

        public async Task<IEnumerable<MonedaDto>> ObtenerMonedasActivasAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MonedaDto>(
                "SELECT * FROM public.up_sel_moneda_xactivo()",
                commandType: CommandType.Text,
                commandTimeout: 60);
        }

        public async Task<IEnumerable<PlanDto>> ObtenerPlanesAsync(int idPromocion, int idGrupo, int idSeccion)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var parameters = new DynamicParameters();
            parameters.Add("@idpromocion", idPromocion, DbType.Int32);
            parameters.Add("@idgrupo", idGrupo, DbType.Int32);
            parameters.Add("@idseccion", idSeccion, DbType.Int32);

            return await connection.QueryAsync<PlanDto>(
                "SELECT * FROM public.up_sel_planpago_xidpromocion_xidgrupo_xidseccion(@idpromocion, @idgrupo, @idseccion)",
                parameters,
                commandType: CommandType.Text,
                commandTimeout: 60);
        }

        public async Task<IEnumerable<BecaDto>> ObtenerBecasActivasAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<BecaDto>(
                "SELECT * FROM public.up_sel_becabeneficio_xactivo()",
                commandType: CommandType.Text,
                commandTimeout: 60);
        }

        public async Task<IEnumerable<CursoDisponibleDto>> ObtenerCursosDisponiblesAsync(int idSubTipo, int idProducto, string texto)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var parameters = new DynamicParameters();
            parameters.Add("@p_idsubtipo", idSubTipo, DbType.Int32);
            parameters.Add("@p_idproducto", idProducto, DbType.Int32);
            parameters.Add("@p_texto", texto ?? string.Empty, DbType.String);

            // Mapeo forzado con comillas dobles para coincidir exactamente con el DTO en PascalCase
            return await connection.QueryAsync<CursoDisponibleDto>(
                @"SELECT 
                    idmodulo AS ""IdModulo"", 
                    modulonombre AS ""ModuloNombre"", 
                    idcurso AS ""IdCurso"", 
                    nombre AS ""Nombre"", 
                    idseccion AS ""IdSeccion"", 
                    codigo AS ""Codigo"" 
                  FROM public.up_sel_promogrupo_promogruposec_promocursec_xidprod(@p_idsubtipo, @p_idproducto, @p_texto)",
                parameters,
                commandType: CommandType.Text,
                commandTimeout: 60);
        }
    }
}