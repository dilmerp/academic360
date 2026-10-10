using System.Data;
using Academic360.Application.Actores.Interfaces;
using Academic360.Application.Alumnos.Interfaces;
using Academic360.Application.Common.Interfaces;
using Academic360.Domain.Actores;
using MediatR;

namespace Academic360.Application.Alumnos.Commands.CrearAlumno
{
    public class CrearAlumnoCommandHandler : IRequestHandler<CrearAlumnoCommand, long>
    {
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IActorFotoRepository _actorFotoRepository;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUsuarioActual _usuarioActual;

        public CrearAlumnoCommandHandler(
            IAlumnoRepository alumnoRepository,
            IActorFotoRepository actorFotoRepository,
            IDbConnectionFactory connectionFactory,
            IUsuarioActual usuarioActual)
        {
            _alumnoRepository = alumnoRepository;
            _actorFotoRepository = actorFotoRepository;
            _connectionFactory = connectionFactory;
            _usuarioActual = usuarioActual;
        }

        public async Task<long> Handle(CrearAlumnoCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Datos;

            // Excelente mitigación OWASP A01: El ID del autor se extrae de forma segura desde el JWT (Claims)
            long idUsuarioCreacion = _usuarioActual.ObtenerIdUsuario();

            // CORRECCIÓN OWASP A02: Aplicar Hash BCrypt a la contraseña entrante
            // Validamos que no llegue nula para evitar excepciones en la librería BCrypt
            string contraseñaSegura = string.IsNullOrWhiteSpace(dto.Contrasena)
                ? string.Empty
                : BCrypt.Net.BCrypt.HashPassword(dto.Contrasena, 12);

            // Mapeo manual de DTO a Entidad de Dominio (Actor)
            var nuevoAlumno = new Actor
            {
                Usuario = dto.Usuario,
                Contrasena = contraseñaSegura, // El SP ahora recibirá el Hash ($2a$12$...)
                Paterno = dto.Paterno,
                Materno = dto.Materno,
                Nombres = dto.Nombres,
                IdTenor = dto.IdTenor,
                Sexo = dto.Sexo,
                IdCivil = dto.IdCivil,
                Email = dto.Email,
                EmailOpcional = dto.EmailOpcional,
                FechaNacimiento = dto.FechaNacimiento,
                IdPaisNacimiento = dto.IdPaisNacimiento,
                IdUbigeoNacimiento = dto.IdUbigeoNacimiento,
                IdUbigeoNacimientoOtro = dto.IdUbigeoNacimientoOtro,
                Telefono = dto.Telefono,
                Celular = dto.Celular,
                TelefonoReferencial = dto.TelefonoReferencial,
                Direccion = dto.Direccion,
                Urbanizacion = dto.Urbanizacion,
                DireccionReferencia = dto.DireccionReferencia,
                IdPais = dto.IdPais,
                IdUbigeo = dto.IdUbigeo,
                IdUbigeoResidenciaOtro = dto.IdUbigeoResidenciaOtro,
                IdNacionalidad = dto.IdNacionalidad,
                CodigoPostal = dto.CodigoPostal,
                IdDocumento = dto.IdDocumento,
                NumeroDocumento = dto.NumeroDocumento,
                Ruc = dto.Ruc,
                Trabaja = dto.Trabaja,
                Estado = dto.Estado,
                IdPeriodo = dto.IdPeriodo,
                UsuarioCreacion = idUsuarioCreacion
            };

            // Orquestación con Transacción usando Dapper (Clean Architecture)
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            // Validar que la conexión no esté abierta antes de intentar abrirla
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Insertar el alumno invocando el repositorio
                long idActor = await _alumnoRepository.InsertarAsync(nuevoAlumno, transaction, cancellationToken);

                // 2. Si hay foto, invocar la persistencia binaria de la imagen dentro de la misma transacción
                if (dto.Foto != null && dto.Foto.Length > 0)
                {
                    await _actorFotoRepository.ActualizarFotoAsync(idActor, dto.Foto, transaction, cancellationToken);
                }

                // Confirmar transacción
                transaction.Commit();

                return idActor;
            }
            catch
            {
                transaction.Rollback();
                throw; // El middleware global de excepciones interceptará el throw
            }
        }
    }
}