namespace Academic360.Application.Catalogos.DTOs;

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

public class ConceptoPrecioDto
{
    public int IdConcepto { get; set; }
    public int IdPrecio { get; set; }
    public string PrecioDescripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
}