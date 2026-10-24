using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
// Requiere: dotnet add package BCrypt.Net-Next

namespace Academic360.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MigracionController : ControllerBase
    {
        private readonly string _connectionString;

        public MigracionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("migrar-claves-hex-a-bcrypt")]
        public async Task<IActionResult> MigrarClaves()
        {
            int registrosActualizados = 0;
            var usuarios = new List<(int Id, string ClaveHex)>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // 1. Leer todas las claves en formato Hexadecimal
                using (SqlCommand cmdSelect = new SqlCommand("SELECT IdUsuario, clave FROM usuario WHERE clave NOT LIKE '$2a$%'", conn))
                using (SqlDataReader reader = await cmdSelect.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usuarios.Add((reader.GetInt32(0), reader.GetString(1)));
                    }
                }

                // 2. Convertir y actualizar una por una
                foreach (var user in usuarios)
                {
                    try
                    {
                        // Decodificar HEX a Texto Plano
                        byte[] bytes = Convert.FromHexString(user.ClaveHex);
                        string passwordPlano = System.Text.Encoding.UTF8.GetString(bytes);

                        // Encriptar a BCrypt
                        string hashSeguro = BCrypt.Net.BCrypt.HashPassword(passwordPlano, 12);

                        // Actualizar en BD
                        using (SqlCommand cmdUpdate = new SqlCommand("UPDATE usuario SET clave = @Hash WHERE IdUsuario = @Id", conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@Hash", hashSeguro);
                            cmdUpdate.Parameters.AddWithValue("@Id", user.Id);
                            await cmdUpdate.ExecuteNonQueryAsync();
                            registrosActualizados++;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return Ok(new { Mensaje = $"Migración exitosa. Se actualizaron {registrosActualizados} usuarios a BCrypt." });
        }
    }
}