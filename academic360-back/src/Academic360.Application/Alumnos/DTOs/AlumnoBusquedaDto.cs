namespace Academic360.Application.Alumnos.DTOs
{
    public class AlumnoBusquedaDto
    {
        public int IdAlumno { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string NombresCompletos { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}