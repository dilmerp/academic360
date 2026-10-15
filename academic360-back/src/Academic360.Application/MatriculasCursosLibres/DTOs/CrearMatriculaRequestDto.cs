using System;
using System.Collections.Generic;

namespace Academic360.Application.MatriculasCursosLibres.DTOs
{
    public class CrearMatriculaRequestDto
    {
        public long IdAlumno { get; set; }
        public int IdProducto { get; set; }
        public long IdPromocion { get; set; }
        public long IdGrupo { get; set; }
        public int IdMedio { get; set; }
        public long IdBeca { get; set; }
        public long IdPlan { get; set; }
        public long IdMoneda { get; set; }
        public long UsuarioCreacion { get; set; }

        // Detalles enviados en la misma petición
        public List<CursoMatriculaDto> Cursos { get; set; } = new();
        public List<CuotaMatriculaDto> Cuotas { get; set; } = new();
    }

    public class CursoMatriculaDto
    {
        public long IdModulo { get; set; }
        public long IdCurso { get; set; }
        public long IdSeccion { get; set; }
    }

    public class CuotaMatriculaDto
    {
        public long IdConcepto { get; set; }
        public long IdPrecio { get; set; }
        public string PrecioDescripcion { get; set; }
        public int Cuota { get; set; }
        public decimal TotalCuotas { get; set; }
        public decimal Precio { get; set; }
        public DateTime Vencimiento { get; set; }
    }
}