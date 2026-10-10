namespace Academic360.Application.Alumnos.DTOs;

public class AlumnoResponseDto
{
    public long IdActor { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Paterno { get; set; } = string.Empty;
    public string Materno { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public long IdTenor { get; set; }
    public string TenorCodigo { get; set; } = string.Empty;
    public bool Genero { get; set; }
    public long IdCivil { get; set; }
    public string CivilNombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmailOpcional { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public long IdPaisNacimiento { get; set; }
    public string PaisNacimiento { get; set; } = string.Empty;
    public string IdUbigeoNacimiento { get; set; } = string.Empty;
    public string DistritoNombreNacimiento { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Celular { get; set; } = string.Empty;
    public string TelefonoReferencial { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Urbanizacion { get; set; } = string.Empty;
    public string DireccionReferencia { get; set; } = string.Empty;
    public long IdDocumento { get; set; }
    public string DocumentoCodigo { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Ruc { get; set; } = string.Empty;
    public bool Trabaja { get; set; }
    public long IdNacionalidad { get; set; }
    public string NacionalidadNombre { get; set; } = string.Empty;
    public long IdPaisResidencia { get; set; }
    public string PaisResidencia { get; set; } = string.Empty;
    public string IdUbigeoResidencia { get; set; } = string.Empty;
    public string DistritoNombreResidencia { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string EstadoNombre { get; set; } = string.Empty;
    public long IdFamilia { get; set; }
}