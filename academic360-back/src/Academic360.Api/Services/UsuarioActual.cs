using System;
using System.Security.Claims;
using Academic360.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Academic360.Api.Services;

public class UsuarioActual : IUsuarioActual
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioActual(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long ObtenerIdUsuario()
    {
        // Extrae el Claim (NameIdentifier) que inyectamos al generar el JWT en el Login
        var idClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (long.TryParse(idClaim, out long idUsuario))
        {
            return idUsuario;
        }

        // Si alguien logra evadir el controlador pero su token no tiene el ID, la capa de aplicación lo rechaza
        throw new UnauthorizedAccessException("Usuario no autenticado o token inválido.");
    }
}