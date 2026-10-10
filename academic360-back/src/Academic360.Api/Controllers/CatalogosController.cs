using Academic360.Application.Catalogos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}