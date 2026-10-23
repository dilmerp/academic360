using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using Academic360.Application.Matriculas.CursosLibres.Commands.CrearMatricula;
using Academic360.Application.Matriculas.CursosLibres.Queries.ObtenerMatriculasCursosLibres;
using Microsoft.AspNetCore.Authorization;
using Academic360.Application.Matriculas.CursosLibres.DTOs;

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
        public async Task<IActionResult> CrearMatricula([FromBody] RegistrarMatriculaRequestDto request)
        {
            // El ID del usuario que registra se puede inyectar desde el Token JWT usando tu servicio IUsuarioActual
            // request.UsuarioCreacion = _usuarioActual.Id;

            var command = new RegistrarMatriculaCommand(request);
            var idMatricula = await _mediator.Send(command);

            return Ok(new { message = "Matrícula registrada con éxito", idMatricula });
        }

        [HttpGet]
        public async Task<IActionResult> ListarMatriculas([FromQuery] string filtro = "")
        {
            // Utiliza la Query de listado general para alimentar la grilla y la barra de búsqueda de la pantalla principal
            var query = new ObtenerMatriculasCursosLibresQuery(filtro);
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }
    }
}