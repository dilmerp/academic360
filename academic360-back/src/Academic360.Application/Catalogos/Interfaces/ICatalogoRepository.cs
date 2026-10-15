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

public interface ICatalogoRepository
{
    Task<IEnumerable<SubTipoDto>> ObtenerSubTiposActivosAsync();
    Task<IEnumerable<ProductoDto>> ObtenerProductosPorSubTipoAsync(int idTipo, int idSubTipo);
    Task<IEnumerable<PromocionDto>> ObtenerPromocionesPorProductoAsync(int idProducto);
    Task<IEnumerable<TenorDto>> ObtenerTenorActivosAsync();
}