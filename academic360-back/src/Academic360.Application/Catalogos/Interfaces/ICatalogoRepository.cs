using System.Collections.Generic;
using System.Threading.Tasks;
using Academic360.Application.Catalogos.DTOs;

namespace Academic360.Application.Catalogos.Interfaces;

public interface ICatalogoRepository
{
    Task<IEnumerable<SubTipoDto>> ObtenerSubTiposActivosAsync();
    Task<IEnumerable<ProductoDto>> ObtenerProductosPorSubTipoAsync(int idTipo, int idSubTipo);
    Task<IEnumerable<PromocionDto>> ObtenerPromocionesPorProductoAsync(int idProducto);
    Task<IEnumerable<TenorDto>> ObtenerTenorActivosAsync();
    Task<IEnumerable<GrupoDto>> ObtenerGruposPorPromocionAsync(int idPromocion);
    Task<IEnumerable<MedioDto>> ObtenerMediosActivosAsync();
    Task<IEnumerable<MonedaDto>> ObtenerMonedasActivasAsync();
    Task<IEnumerable<PlanDto>> ObtenerPlanesAsync(int idPromocion, int idGrupo, int idSeccion);
    Task<IEnumerable<BecaDto>> ObtenerBecasActivasAsync();
    Task<IEnumerable<CursoDisponibleDto>> ObtenerCursosDisponiblesAsync(int idSubTipo, int idProducto, string texto);
    Task<IEnumerable<ConceptoPrecioDto>> ObtenerConceptosPrecioAsync(int idMoneda, int? idPlan);
}