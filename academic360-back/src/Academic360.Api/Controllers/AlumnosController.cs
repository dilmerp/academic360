using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Academic360.Application.Alumnos.Commands.ActualizarEstadoMatricula;
using Academic360.Application.Alumnos.Commands.CrearAlumno;
using Academic360.Application.Alumnos.Commands.EliminarAlumno;
using Academic360.Application.Alumnos.DTOs;
using Academic360.Application.Alumnos.Queries.ObtenerAlumnoPorId;
using Academic360.Application.Alumnos.Queries.ObtenerAlumnos;
using Academic360.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Academic360.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AlumnosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AlumnosController> _logger;

        public AlumnosController(IMediator mediator, ILogger<AlumnosController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        // CORRECCIÓN OWASP A01: Solo administradores o personal autorizado pueden crear alumnos.
        // Nota: Requiere que en el LoginCommandHandler agregues un Claim de tipo ClaimTypes.Role
        // [Authorize(Roles = "Administrador,Secretaria")] 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Añadido para rechazos por falta de permisos
        public async Task<IActionResult> CrearAlumno([FromBody] CrearAlumnoRequestDto request, [FromQuery] bool forceHomologo = false)
        {
            try
            {
                var command = new CrearAlumnoCommand(request);
                var idGenerado = await _mediator.Send(command);

                return CreatedAtAction(nameof(ObtenerAlumnoPorId), new { id = idGenerado }, new { Id = idGenerado });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear alumno. Posible intento de manipulación de datos.");
                return BadRequest(new { detail = "No se pudo procesar la solicitud de creación." });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AlumnoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerAlumnoPorId(long id)
        {
            try
            {
                // CORRECCIÓN OWASP A01 (IDOR): Extraer quién es el usuario que hace la petición
                var idUsuarioActualString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // REGLA DE NEGOCIO: Si eres alumno, solo puedes ver tu propio ID. 
                // Si eres Admin, puedes ver cualquiera. (Lógica pseudo-código para tu equipo)
                /*
                if (!User.IsInRole("Administrador") && idUsuarioActualString != id.ToString())
                {
                    _logger.LogWarning("Intento de acceso IDOR bloqueado. Usuario: {IdUsuario} intentó ver Alumno: {Id}", idUsuarioActualString, id);
                    return Forbid(); // Retorna 403 en lugar de 404 para no dar pistas
                }
                */

                var query = new ObtenerAlumnoPorIdQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { detail = "El recurso solicitado no existe." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno obteniendo alumno {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { detail = "Ocurrió un error interno." });
            }
        }

        [HttpGet]
        // CORRECCIÓN OWASP A01: Exposición masiva de datos. Restringir a personal administrativo.
        // [Authorize(Roles = "Administrador,Docente")]
        [ProducesResponseType(typeof(PagedResult<AlumnoListadoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar([FromQuery] string? filtroGlobal = "", [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            throw new Exception("Simulando una caída catastrófica del sistema");

            var query = new ObtenerAlumnosPorCriterioQuery
            {
                FiltroGlobal = filtroGlobal ?? string.Empty,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        // [Authorize(Roles = "Administrador")] // Solo admin puede eliminar
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Eliminar(long id)
        {
            try
            {
                var command = new EliminarAlumnoCommand { IdActor = id };
                await _mediator.Send(command);

                _logger.LogInformation("Alumno {Id} eliminado por el usuario {IdUsuario}", id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return Ok(new { Mensaje = "Alumno eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error intentando eliminar al alumno {Id}", id);
                return BadRequest(new { detail = "No se pudo eliminar el recurso." });
            }
        }

        [HttpPatch("{idAlumno}/registros/{idRegistro}/matriculas/{idMatricula}/estado")]
        // [Authorize(Roles = "Administrador,Tesoreria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ActualizarEstadoMatricula(long idAlumno, long idRegistro, long idMatricula, [FromBody] ActualizarEstadoMatriculaRequestDto request)
        {
            try
            {
                // Aquí también aplica la prevención IDOR: ¿Puede el alumno actualizar su propia matrícula o es exclusivo de Tesorería/Admin?
                var command = new ActualizarEstadoMatriculaCommand(idAlumno, idRegistro, idMatricula, request.EsMatricula);
                await _mediator.Send(command);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo actualizando estado de matrícula {IdMatricula} para Alumno {IdAlumno}", idMatricula, idAlumno);
                return StatusCode(StatusCodes.Status500InternalServerError, new { detail = "Ocurrió un error interno procesando la actualización." });
            }
        }
    }

    public class ActualizarEstadoMatriculaRequestDto
    {
        public bool EsMatricula { get; set; }
    }
}