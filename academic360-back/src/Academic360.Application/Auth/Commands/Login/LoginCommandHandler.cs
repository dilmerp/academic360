using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Academic360.Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var usuarioDto = await _usuarioRepository.ObtenerUsuarioPorLoginAsync(request.Usuario, cancellationToken);

            if (usuarioDto == null)
            {
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Contrasena, usuarioDto.ClaveHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            if (!usuarioDto.Activo)
            {
                throw new UnauthorizedAccessException("El usuario se encuentra inactivo, comuníquese con el administrador.");
            }

            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKeyValue = jwtSettings["Secret"];

            if (string.IsNullOrWhiteSpace(secretKeyValue))
            {
                throw new InvalidOperationException("Seguridad Comprometida: Falta la configuración crítica Jwt:Secret.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyValue));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Claims del JWT: Base para Políticas RBAC y prevención BOLA
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioDto.IdUsuario.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuarioDto.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuarioDto.Login),
                new Claim("Nombre", usuarioDto.Nombre),
                new Claim("IdArea", usuarioDto.IdArea.ToString()),
                // Ejemplo para habilitar [Authorize(Roles = "Admin")] si el DTO tuviera un Rol:
                // new Claim(ClaimTypes.Role, usuarioDto.Rol ?? "UsuarioEstandar"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? throw new InvalidOperationException("Falta Jwt:Issuer"),
                audience: jwtSettings["Audience"] ?? throw new InvalidOperationException("Falta Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                IdUsuario = usuarioDto.IdUsuario,
                NombreUsuario = usuarioDto.Nombre
            };
        }
    }
}