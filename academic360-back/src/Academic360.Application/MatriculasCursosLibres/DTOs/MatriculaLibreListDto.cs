namespace Academic360.Application.MatriculasCursosLibres.DTOs
{
    public class MatriculaLibreListDto
    {
        public long IdRegistro { get; set; }
        public long IdMatricula { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Alumno { get; set; } = string.Empty;
        public string Curso { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
    }
}