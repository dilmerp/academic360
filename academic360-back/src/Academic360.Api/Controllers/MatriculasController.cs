using Academic360.Application.Common.Models;
using Academic360.Application.Matriculas.Commands.ActualizarEstadoMatricula;
using Academic360.Application.Matriculas.Commands.RegistrarMatricula;
using Academic360.Application.Matriculas.DTOs;
using Academic360.Application.Matriculas.Enums;
using Academic360.Application.Matriculas.Queries.GetMatriculasByPromocion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Academic360.Api.Controllers;

[Authorize] // Asegura que solo usuarios autenticados con JWT puedan acceder
[ApiController]
[Route("api/[controller]")]
public class MatriculasController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatriculasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene el listado paginado de alumnos matriculados o por matricular según la promoción.
    /// </summary>
    /// <param name="idPromocion">ID de la promoción a consultar</param>
    /// <param name="estado">0: Nuevos, 1: Matriculados, 2: Todos</param>
    /// <param name="searchTerm">Texto para búsqueda global (opcional)</param>
    /// <param name="pageNumber">Número de página actual</param>
    /// <param name="pageSize">Cantidad de registros por página</param>
    /// <returns>Resultado paginado de alumnos</returns>
    [HttpGet("promocion/{idPromocion}")]
    public async Task<ActionResult<PagedResult<AlumnoMatriculaListadoDto>>> ObtenerPorPromocion(
        int idPromocion,
        [FromQuery] FiltroMatriculaEstado estado = FiltroMatriculaEstado.Nuevos,
        [FromQuery] string searchTerm = "",
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetMatriculasByPromocionQuery
        {
            IdPromocion = idPromocion,
            Estado = estado,
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Confirma o deshace la matrícula de un alumno.
    /// </summary>
    /// <param name="command">Comando con los IDs y el nuevo estado (EsMatricula: true/false)</param>
    /// <returns>Ok si la operación fue exitosa</returns>
    [HttpPut("estado")]
    public async Task<ActionResult> ActualizarEstado([FromBody] ActualizarEstadoMatriculaCommand command)
    {
        // Validar que los IDs sean válidos
        if (command.IdAlumno <= 0 || command.IdRegistro <= 0 || command.IdMatricula <= 0)
        {
            return BadRequest("Los identificadores proporcionados no son válidos.");
        }

        var result = await _mediator.Send(command);

        if (result)
        {
            return Ok(new { Mensaje = "Estado de matrícula actualizado correctamente." });
        }

        return BadRequest("No se pudo actualizar el estado de la matrícula.");
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarMatricula([FromBody] RegistrarMatriculaCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(new { Exito = true, Mensaje = "Matrícula registrada correctamente", Data = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Exito = false, Mensaje = ex.Message });
        }
    }
}