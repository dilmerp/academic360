using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Alumnos.DTOs;
using Academic360.Application.Alumnos.Interfaces;
using Academic360.Application.Alumnos.Queries.ObtenerAlumnos;
using Academic360.Domain.Actores;
using Dapper;

namespace Academic360.Infrastructure.Repositories
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly IDbConnection _connection;

        public AlumnoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<long> InsertarAsync(Actor alumno, IDbTransaction transaction, CancellationToken cancellationToken = default)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@usuario", alumno.Usuario, DbType.String, size: 20);
            // CRÍTICO: Asegurarse de que el Handler haya encriptado alumno.Contrasena con BCrypt antes de llegar aquí.
            parameters.Add("@contrasena", alumno.Contrasena, DbType.String, size: 100);
            parameters.Add("@paterno", alumno.Paterno, DbType.String, size: 50);
            parameters.Add("@materno", string.IsNullOrWhiteSpace(alumno.Materno) ? null : alumno.Materno, DbType.String, size: 50);
            parameters.Add("@nombres", alumno.Nombres, DbType.String, size: 50);
            parameters.Add("@idtenor", alumno.IdTenor, DbType.Int32);
            parameters.Add("@genero", alumno.Sexo, DbType.Boolean);
            parameters.Add("@idcivil", alumno.IdCivil, DbType.Int32);
            parameters.Add("@email", alumno.Email, DbType.String, size: 100);
            parameters.Add("@emailopcional", string.IsNullOrWhiteSpace(alumno.EmailOpcional) ? null : alumno.EmailOpcional, DbType.String, size: 100);
            parameters.Add("@fechanacimiento", alumno.FechaNacimiento, DbType.Date);
            parameters.Add("@idpaisnacimiento", alumno.IdPaisNacimiento, DbType.Int32);
            parameters.Add("@idubigeonacimiento", string.IsNullOrWhiteSpace(alumno.IdUbigeoNacimiento) ? null : alumno.IdUbigeoNacimiento, DbType.String, size: 6);
            parameters.Add("@idubigeonacimientootro", string.IsNullOrWhiteSpace(alumno.IdUbigeoNacimientoOtro) ? null : alumno.IdUbigeoNacimientoOtro, DbType.String, size: 100);
            parameters.Add("@telefono", string.IsNullOrWhiteSpace(alumno.Telefono) ? null : alumno.Telefono, DbType.String, size: 100);
            parameters.Add("@celular", string.IsNullOrWhiteSpace(alumno.Celular) ? null : alumno.Celular, DbType.String, size: 100);
            parameters.Add("@telefonoreferencial", string.IsNullOrWhiteSpace(alumno.TelefonoReferencial) ? null : alumno.TelefonoReferencial, DbType.String, size: 100);
            parameters.Add("@direccion", alumno.Direccion, DbType.String, size: 500);
            parameters.Add("@urbanizacion", string.IsNullOrWhiteSpace(alumno.Urbanizacion) ? null : alumno.Urbanizacion, DbType.String, size: 100);
            parameters.Add("@direccionreferencia", string.IsNullOrWhiteSpace(alumno.DireccionReferencia) ? null : alumno.DireccionReferencia, DbType.String, size: 100);
            parameters.Add("@idpais", alumno.IdPais, DbType.Int32);
            parameters.Add("@idubigeo", string.IsNullOrWhiteSpace(alumno.IdUbigeo) ? null : alumno.IdUbigeo, DbType.String, size: 6);
            parameters.Add("@idubigeoresidenciaotro", string.IsNullOrWhiteSpace(alumno.IdUbigeoResidenciaOtro) ? null : alumno.IdUbigeoResidenciaOtro, DbType.String, size: 100);
            parameters.Add("@idnacionalidad", alumno.IdNacionalidad, DbType.Int32);
            parameters.Add("@codigopostal", string.IsNullOrWhiteSpace(alumno.CodigoPostal) ? null : alumno.CodigoPostal, DbType.String, size: 10);
            parameters.Add("@iddocumento", alumno.IdDocumento == 0 ? null : alumno.IdDocumento, DbType.Int32);
            parameters.Add("@numerodocumento", string.IsNullOrWhiteSpace(alumno.NumeroDocumento) ? null : alumno.NumeroDocumento, DbType.String, size: 20);
            parameters.Add("@ruc", string.IsNullOrWhiteSpace(alumno.Ruc) ? null : alumno.Ruc, DbType.String, size: 20);
            parameters.Add("@trabaja", alumno.Trabaja ? 1 : 0, DbType.Boolean);
            parameters.Add("@estado", alumno.Estado, DbType.StringFixedLength, size: 1);
            parameters.Add("@idperiodo", alumno.IdPeriodo, DbType.Int32);
            parameters.Add("@usuariocreacion", alumno.UsuarioCreacion, DbType.Int32);
            parameters.Add("@retval", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

            // CORRECCIÓN: Agregar cancellationToken usando CommandDefinition para transacciones
            var command = new CommandDefinition(
                "up_ins_actor_xalumno",
                parameters,
                transaction: transaction,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            await transaction.Connection.ExecuteAsync(command);

            var strDatos = parameters.Get<string>("@retval");
            if (string.IsNullOrEmpty(strDatos))
                throw new Exception("El Stored Procedure no devolvió el ID esperado en el parámetro OUTPUT.");

            var arrDatos = strDatos.Split('|');
            if (arrDatos.Length > 0 && long.TryParse(arrDatos[0], out long idActor))
            {
                return idActor;
            }

            throw new Exception("No se pudo obtener el IdActor desde la respuesta del SP.");
        }

        public async Task<AlumnoResponseDto?> ObtenerPorIdAsync(long idActor, CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition("up_sel_actor_xid", new { IdActor = idActor }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return await _connection.QueryFirstOrDefaultAsync<AlumnoResponseDto>(command);
        }

        public async Task<IEnumerable<AlumnoListadoDto>> ListarAlumnosAsync(string codigo, string nombre, CancellationToken cancellationToken = default)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@codigo", string.IsNullOrWhiteSpace(codigo) ? "" : codigo, DbType.String, size: 20);
            parameters.Add("@nombre", string.IsNullOrWhiteSpace(nombre) ? "" : nombre, DbType.String, size: 200);

            var command = new CommandDefinition("up_sel_actor_xcontrol_xalumno", parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<AlumnoListadoDto>(command);
        }

        public async Task<int> EliminarAlumnoAsync(long idActor, CancellationToken cancellationToken = default)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idactor", idActor, DbType.Int32);
            parameters.Add("@retval", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var command = new CommandDefinition("up_del_actor_xalumno", parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(command);

            return parameters.Get<int>("@retval");
        }
    }
}