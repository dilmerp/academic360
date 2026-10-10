using Academic360.Domain.Common;
using Academic360.Domain.Enums;

namespace Academic360.Domain.Actores;

public class Actor : EntidadAuditable
{
    public long IdActor { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public string Paterno { get; set; } = string.Empty;
    public string Materno { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public long IdTenor { get; set; }
    public bool Sexo { get; set; }
    public long IdCivil { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? EmailOpcional { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public long IdPaisNacimiento { get; set; }
    public string IdUbigeoNacimiento { get; set; } = string.Empty;
    public string? IdUbigeoNacimientoOtro { get; set; }
    public string? Telefono { get; set; }
    public string? Celular { get; set; }
    public string? TelefonoReferencial { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string? Urbanizacion { get; set; }
    public string? DireccionReferencia { get; set; }
    public long IdPais { get; set; }
    public string IdUbigeo { get; set; } = string.Empty;
    public string? IdUbigeoResidenciaOtro { get; set; }
    public long IdNacionalidad { get; set; }
    public string? CodigoPostal { get; set; }
    public long IdDocumento { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;
    public string? Ruc { get; set; }
    public bool Trabaja { get; set; }
    public string Estado { get; set; } = "A";
    public long IdPeriodo { get; set; }
}