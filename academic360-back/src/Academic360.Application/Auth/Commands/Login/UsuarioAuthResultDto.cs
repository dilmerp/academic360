using System;

namespace Academic360.Application.Auth.Commands.Login
{
    public class UsuarioAuthResultDto
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Login { get; set; }

        // --- LA PROPIEDAD FALTANTE ---
        // Aquí Dapper mapeará el hash seguro ($2a$12$...) que viene de SQL Server
        public string ClaveHash { get; set; }

        public string EMail { get; set; }

        public bool Activo { get; set; }

        public int IdArea { get; set; }

        // Opcional: Si necesitas mapear la fecha que devuelve el SP
        // public DateTime FechaActual { get; set; }
    }
}