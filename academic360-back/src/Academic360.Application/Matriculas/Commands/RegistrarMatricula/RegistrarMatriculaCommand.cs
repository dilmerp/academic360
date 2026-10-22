using MediatR;

namespace Academic360.Application.Matriculas.Commands.RegistrarMatricula
{
    public class RegistrarMatriculaCommand : IRequest<string>
    {
        public int IdAlumno { get; set; }
        public string Paterno { get; set; } = string.Empty;
        public string Materno { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public int IdTenor { get; set; }
        public bool Sexo { get; set; }
        public int IdDocumento { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public int IdCivil { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Urbanizacion { get; set; } = string.Empty;
        public int IdPais { get; set; }
        public string IdUbigeo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int IdPeriodo { get; set; }

        public int IdTipo { get; set; }
        public int IdSubTipo { get; set; }
        public int IdProducto { get; set; }
        public int IdPromocion { get; set; }
        public int IdGrupo { get; set; }
        public int IdMedio { get; set; }
        public int IdBeca { get; set; }
        public int IdPlan { get; set; }
        public int IdMoneda { get; set; }
        public DateTime MatriculaFecha { get; set; } = DateTime.UtcNow;

        public int UsuarioCreacion { get; set; }
        public int IdFacultad { get; set; }
        public int IdMovimiento { get; set; }

        public List<CursoMatriculaDto> Cursos { get; set; } = new();
        public List<CuotaProgramacionDto> Cuotas { get; set; } = new();
    }

    public class CursoMatriculaDto
    {
        public int IdModulo { get; set; }
        public int IdCurso { get; set; }
        public int IdSeccion { get; set; }
    }

    public class CuotaProgramacionDto
    {
        public int Cantidad { get; set; }
        public string Cuota { get; set; } = string.Empty;
        public int CuotaNumero { get; set; }
        public int Cuotas { get; set; }
        public decimal TotalCuotas { get; set; }
        public int IdConcepto { get; set; }
        public int IdPrecio { get; set; }
        public decimal Monto { get; set; }
        public int IdMora { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool EsContado { get; set; }
        public bool EsInicial { get; set; }
        public bool EsCuota { get; set; }
        public bool EsObligatorio { get; set; }
    }
}