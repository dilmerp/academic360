using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using Academic360.Application.MatriculasCursosLibres.DTOs;
using Academic360.Application.MatriculasCursosLibres.Commands.CrearMatricula;
using Microsoft.AspNetCore.Authorization;

namespace Academic360.Api.Controllers
{
    [ApiController]
    [Route("api/matriculas-cursos-libres")]
    [Authorize] // Aseguramos el acceso
    public class MatriculasCursosLibresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatriculasCursosLibresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CrearMatricula([FromBody] CrearMatriculaRequestDto request)
        {
            // El ID del usuario que registra se puede inyectar desde el Token JWT usando tu servicio IUsuarioActual
            // request.UsuarioCreacion = _usuarioActual.Id;

            var command = new CrearMatriculaCommand(request);
            var idMatricula = await _mediator.Send(command);

            return Ok(new { message = "Matrícula registrada con éxito", idMatricula });
        }
    }
}