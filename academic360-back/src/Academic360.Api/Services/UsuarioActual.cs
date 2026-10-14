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
        var idClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (long.TryParse(idClaim, out long idUsuario))
        {
            return idUsuario;
        }

        throw new UnauthorizedAccessException("Usuario no autenticado o token inválido.");
    }

    // NUEVO: Permite extraer el IdArea inyectado en el Login para limitar queries
    public int ObtenerIdArea()
    {
        var areaClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("IdArea")?.Value;

        if (int.TryParse(areaClaim, out int idArea))
        {
            return idArea;
        }

        throw new UnauthorizedAccessException("El contexto del usuario carece de un Área válida.");
    }

    // NUEVO: Permite evaluar roles programáticamente desde los comandos (RBAC avanzado)
    public bool PerteneceARol(string rolEsperado)
    {
        var tieneRol = _httpContextAccessor.HttpContext?.User?.IsInRole(rolEsperado);
        return tieneRol ?? false;
    }
}