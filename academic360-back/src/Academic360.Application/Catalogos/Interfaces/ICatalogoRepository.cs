using System.Collections.Generic;
using System.Threading.Tasks;

namespace Academic360.Application.Catalogos.Interfaces;

public class SubTipoDto
{
    public int IdTipo { get; set; }
    public int IdSubTipo { get; set; }
    public string SubTipoNombre { get; set; } = string.Empty;
}

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class PromocionDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Dirigido { get; set; } = string.Empty;
}

public class TenorDto
{
    public int idtenor { get; set; }
    public string TenorCodigo { get; set; } = string.Empty;
    public string TenorNombre { get; set; } = string.Empty;
}

public class GrupoDto
{
    public int IdGrupo { get; set; }
    public string GrupoCodigo { get; set; } = string.Empty;
    public string GrupoNombre { get; set; } = string.Empty;
    public int IdTipo { get; set; }
    public int IdSubTipo { get; set; }
    public int IdProducto { get; set; }
    public int IdModulo { get; set; }
}

public class MedioDto
{
    public int IdMedio { get; set; }
    public string MedioNombre { get; set; } = string.Empty;
}

public class MonedaDto
{
    public int IdMoneda { get; set; }
    public string MonedaCodigo { get; set; } = string.Empty;
    public string MonedaNombre { get; set; } = string.Empty;
}

public class PlanDto
{
    public int IdPlan { get; set; }
    public string PlanNombre { get; set; } = string.Empty;
    public int IdMoneda { get; set; }
    public string MonedaNombre { get; set; } = string.Empty;
}

public class BecaDto
{
    public int IdBeneficio { get; set; }
    public string BeneficioNombre { get; set; } = string.Empty;
    public decimal BeneficioDescuento { get; set; }
}

public class CursoDisponibleDto
{
    public int IdModulo { get; set; }
    public string ModuloNombre { get; set; } = string.Empty;
    public int IdCurso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int IdSeccion { get; set; }
    public string Codigo { get; set; } = string.Empty;
}

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
}