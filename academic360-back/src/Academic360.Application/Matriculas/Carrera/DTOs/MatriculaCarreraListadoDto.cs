namespace Academic360.Application.Matriculas.Carrera.DTOs;

// Asegúrate de que tenga la palabra "public" antes de "class"
public class MatriculaCarreraListadoDto
{
    public int IdRegistro { get; set; }
    public int IdMatricula { get; set; }
    public int IdAlumno { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string GrupoCodigo { get; set; } = string.Empty;
    public string Seccion { get; set; } = string.Empty;
    public DateTime? MatriculaFecha { get; set; }
    public string MatriculadoPor { get; set; } = string.Empty;
    public bool EsMatricula { get; set; }
}