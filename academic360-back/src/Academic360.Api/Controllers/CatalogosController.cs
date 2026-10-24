using Academic360.Application.Catalogos.Interfaces;
using Academic360.Application.Catalogos.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Academic360.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CatalogosController : ControllerBase
{
    private readonly ICatalogoRepository _repository;

    public CatalogosController(ICatalogoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("subtipos")]
    public async Task<ActionResult<IEnumerable<SubTipoDto>>> GetSubTipos()
    {
        var result = await _repository.ObtenerSubTiposActivosAsync();
        return Ok(result);
    }

    [HttpGet("productos")]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos([FromQuery] int idTipo, [FromQuery] int idSubTipo)
    {
        var result = await _repository.ObtenerProductosPorSubTipoAsync(idTipo, idSubTipo);
        return Ok(result);
    }

    [HttpGet("promociones/{idProducto}")]
    public async Task<ActionResult<IEnumerable<PromocionDto>>> GetPromociones(int idProducto)
    {
        var result = await _repository.ObtenerPromocionesPorProductoAsync(idProducto);
        return Ok(result);
    }

    [HttpGet("tenor")]
    public async Task<ActionResult<IEnumerable<TenorDto>>> GetTenor()
    {
        var result = await _repository.ObtenerTenorActivosAsync();
        return Ok(result);
    }

    [HttpGet("grupos/{idPromocion}")]
    public async Task<ActionResult<IEnumerable<GrupoDto>>> GetGrupos(int idPromocion)
    {
        var result = await _repository.ObtenerGruposPorPromocionAsync(idPromocion);
        return Ok(result);
    }

    [HttpGet("medios-comunicacion")]
    public async Task<ActionResult<IEnumerable<MedioDto>>> GetMediosComunicacion()
    {
        var result = await _repository.ObtenerMediosActivosAsync();
        return Ok(result);
    }

    [HttpGet("monedas")]
    public async Task<ActionResult<IEnumerable<MonedaDto>>> GetMonedas()
    {
        var result = await _repository.ObtenerMonedasActivasAsync();
        return Ok(result);
    }

    [HttpGet("planes")]
    public async Task<ActionResult<IEnumerable<PlanDto>>> GetPlanes([FromQuery] int idPromocion, [FromQuery] int idGrupo, [FromQuery] int idSeccion = -1)
    {
        var result = await _repository.ObtenerPlanesAsync(idPromocion, idGrupo, idSeccion);
        return Ok(result);
    }

    [HttpGet("becas/{idPromocion}")]
    public async Task<ActionResult<IEnumerable<BecaDto>>> GetBecas(int idPromocion)
    {
        var result = await _repository.ObtenerBecasActivasAsync();
        return Ok(result);
    }

    [HttpGet("cursos-disponibles")]
    public async Task<ActionResult<IEnumerable<CursoDisponibleDto>>> GetCursosDisponibles([FromQuery] int idSubTipo, [FromQuery] int idProducto, [FromQuery] string texto = "")
    {
        var result = await _repository.ObtenerCursosDisponiblesAsync(idSubTipo, idProducto, texto);
        return Ok(result);
    }

    [HttpGet("conceptos-precio")]
    public async Task<ActionResult<IEnumerable<ConceptoPrecioDto>>> GetConceptosPrecio([FromQuery] int idMoneda, [FromQuery] int? idPlan)
    {
        var result = await _repository.ObtenerConceptosPrecioAsync(idMoneda, idPlan);
        return Ok(result);
    }
}