namespace Academic360.Application.Alumnos.Queries.ObtenerAlumnos;

public class AlumnoListadoDto
{
    public long IdActor { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string EstadoNombre { get; set; } = string.Empty;
}