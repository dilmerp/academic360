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
            // 1. Obtenemos el usuario SOLO por su Login (El SP ya no valida la clave)
            // Nota: Debes actualizar IUsuarioRepository para que el método solo reciba (request.Usuario)
            var usuarioDto = await _usuarioRepository.ObtenerUsuarioPorLoginAsync(request.Usuario, cancellationToken);

            // CORRECCIÓN OWASP A07: Mensaje opaco unificado si el usuario no existe
            if (usuarioDto == null)
            {
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            // 2. CORRECCIÓN OWASP A02: Validación Criptográfica en Memoria con BCrypt
            // Comparamos el texto plano (request.Contrasena) contra el Hash de BD (usuarioDto.ClaveHash)
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Contrasena, usuarioDto.ClaveHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            // Defensa en profundidad: Aunque el SP ya filtra por activos (AND U.activo = 1), 
            // es una buena práctica validarlo también en el core del dominio.
            if (!usuarioDto.Activo)
            {
                throw new UnauthorizedAccessException("El usuario se encuentra inactivo, comuníquese con el administrador.");
            }

            // 3. CORRECCIÓN OWASP A05: Evitar secretos expuestos como mecanismo de "Fallback"
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKeyValue = jwtSettings["Secret"];

            if (string.IsNullOrWhiteSpace(secretKeyValue))
            {
                // Fail-Fast: Si el secreto no se inyecta correctamente (ej. desde User Secrets o Key Vault),
                // el sistema se detiene, evitando generar tokens vulnerables.
                throw new InvalidOperationException("Seguridad Comprometida: Falta la configuración crítica Jwt:Secret.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyValue));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Claims del JWT
            // Se incluyen solo datos identificativos no sensibles. ¡Nunca agregar ClaveHash o similares aquí!
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioDto.IdUsuario.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuarioDto.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuarioDto.Login),
                new Claim("Nombre", usuarioDto.Nombre),
                new Claim("IdArea", usuarioDto.IdArea.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Mitiga ataques de Replay si decides implementar listas negras
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