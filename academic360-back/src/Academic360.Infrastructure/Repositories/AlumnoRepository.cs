using Academic360.Application.Alumnos.DTOs;
using Academic360.Application.Alumnos.Interfaces;
using Academic360.Application.Alumnos.Queries.ObtenerAlumnos;
using Academic360.Application.Common.Exceptions;
using Academic360.Application.Common.Interfaces;
using Academic360.Domain.Actores;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Academic360.Infrastructure.Repositories
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AlumnoRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<long> InsertarAsync(Actor alumno, IDbTransaction transaction, CancellationToken cancellationToken = default)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@usuario", alumno.Usuario, DbType.String);
                parameters.Add("@contrasena", alumno.Contrasena, DbType.String);
                parameters.Add("@paterno", alumno.Paterno, DbType.String);
                parameters.Add("@materno", string.IsNullOrWhiteSpace(alumno.Materno) ? null : alumno.Materno, DbType.String);
                parameters.Add("@nombres", alumno.Nombres, DbType.String);
                parameters.Add("@idtenor", alumno.IdTenor, DbType.Int32);
                //parameters.Add("@genero", alumno.Sexo, DbType.Boolean);
                parameters.Add("@genero", alumno.Sexo);
                parameters.Add("@idcivil", alumno.IdCivil, DbType.Int32);
                parameters.Add("@email", alumno.Email, DbType.String);
                parameters.Add("@emailopcional", string.IsNullOrWhiteSpace(alumno.EmailOpcional) ? null : alumno.EmailOpcional, DbType.String);
                parameters.Add("@fechanacimiento", alumno.FechaNacimiento, DbType.Date);
                parameters.Add("@idpaisnacimiento", alumno.IdPaisNacimiento, DbType.Int32);
                parameters.Add("@idubigeonacimiento", string.IsNullOrWhiteSpace(alumno.IdUbigeoNacimiento) ? null : alumno.IdUbigeoNacimiento, DbType.String);
                parameters.Add("@idubigeonacimientootro", string.IsNullOrWhiteSpace(alumno.IdUbigeoNacimientoOtro) ? null : alumno.IdUbigeoNacimientoOtro, DbType.String);
                parameters.Add("@telefono", string.IsNullOrWhiteSpace(alumno.Telefono) ? null : alumno.Telefono, DbType.String);
                parameters.Add("@celular", string.IsNullOrWhiteSpace(alumno.Celular) ? null : alumno.Celular, DbType.String);
                parameters.Add("@telefonoreferencial", string.IsNullOrWhiteSpace(alumno.TelefonoReferencial) ? null : alumno.TelefonoReferencial, DbType.String);
                parameters.Add("@direccion", alumno.Direccion, DbType.String);
                parameters.Add("@urbanizacion", string.IsNullOrWhiteSpace(alumno.Urbanizacion) ? null : alumno.Urbanizacion, DbType.String);
                parameters.Add("@direccionreferencia", string.IsNullOrWhiteSpace(alumno.DireccionReferencia) ? null : alumno.DireccionReferencia, DbType.String);
                parameters.Add("@idpais", alumno.IdPais, DbType.Int32);
                parameters.Add("@idubigeo", string.IsNullOrWhiteSpace(alumno.IdUbigeo) ? null : alumno.IdUbigeo, DbType.String);
                parameters.Add("@idubigeoresidenciaotro", string.IsNullOrWhiteSpace(alumno.IdUbigeoResidenciaOtro) ? null : alumno.IdUbigeoResidenciaOtro, DbType.String);
                parameters.Add("@idnacionalidad", alumno.IdNacionalidad, DbType.Int32);
                parameters.Add("@codigopostal", string.IsNullOrWhiteSpace(alumno.CodigoPostal) ? null : alumno.CodigoPostal, DbType.String);
                parameters.Add("@iddocumento", alumno.IdDocumento == 0 ? null : alumno.IdDocumento, DbType.Int32);
                parameters.Add("@numerodocumento", string.IsNullOrWhiteSpace(alumno.NumeroDocumento) ? null : alumno.NumeroDocumento, DbType.String);
                parameters.Add("@ruc", string.IsNullOrWhiteSpace(alumno.Ruc) ? null : alumno.Ruc, DbType.String);
                parameters.Add("@trabaja", alumno.Trabaja, DbType.Boolean);
                parameters.Add("@estado", alumno.Estado, DbType.String);
                parameters.Add("@idperiodo", alumno.IdPeriodo, DbType.Int32);
                parameters.Add("@usuariocreacion", alumno.UsuarioCreacion, DbType.Int32);


                var commandText = @"SELECT * FROM public.up_ins_actor_xalumno(
                                    @usuario::varchar, 
                                    @contrasena::varchar, 
                                    @paterno::varchar, 
                                    @materno::varchar, 
                                    @nombres::varchar, 
                                    @idtenor::integer, 
                                    @genero::boolean, 
                                    @idcivil::integer, 
                                    @email::varchar, 
                                    @emailopcional::varchar, 
                                    @fechanacimiento::date, 
                                    @idpaisnacimiento::integer, 
                                    @idubigeonacimiento::varchar, 
                                    @idubigeonacimientootro::varchar, 
                                    @telefono::varchar, 
                                    @celular::varchar, 
                                    @telefonoreferencial::varchar, 
                                    @direccion::varchar, 
                                    @urbanizacion::varchar, 
                                    @direccionreferencia::varchar, 
                                    @idpais::integer, 
                                    @idubigeo::varchar, 
                                    @idubigeoresidenciaotro::varchar, 
                                    @idnacionalidad::integer, 
                                    @codigopostal::varchar, 
                                    @iddocumento::integer, 
                                    @numerodocumento::varchar, 
                                    @ruc::varchar, 
                                    @trabaja::boolean, 
                                    @estado::varchar, 
                                    @idperiodo::integer, 
                                    @usuariocreacion::integer
                                )";

                var strDatos = await transaction.Connection!.QueryFirstOrDefaultAsync<string>(commandText, parameters, transaction);

                if (string.IsNullOrEmpty(strDatos))
                    throw new Exception("La función en PostgreSQL no devolvió ningún valor.");

                var arrDatos = strDatos.Split('|');
                if (arrDatos.Length > 0 && long.TryParse(arrDatos[0], out long idActor))
                {
                    return idActor;
                }

                throw new Exception($"No se pudo obtener el IdActor desde la respuesta de la base de datos. Respuesta cruda: {strDatos}");
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error real en PostgreSQL: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AlumnoBusquedaDto>> BuscarAlumnosAsync(string filtro, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            var command = new CommandDefinition(
                commandText: "SELECT * FROM public.up_sel_alumno_buscar(@Filtro)",
                parameters: new { Filtro = filtro },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken
            );

            return await connection.QueryAsync<AlumnoBusquedaDto>(command);
        }

        // Para SQL SERVER
        //public async Task<long> InsertarAsync(Actor alumno, IDbTransaction transaction, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@usuario", alumno.Usuario, DbType.String, size: 20);
        //        parameters.Add("@contrasena", alumno.Contrasena, DbType.String, size: 100);
        //        parameters.Add("@paterno", alumno.Paterno, DbType.String, size: 50);
        //        parameters.Add("@materno", string.IsNullOrWhiteSpace(alumno.Materno) ? null : alumno.Materno, DbType.String, size: 50);
        //        parameters.Add("@nombres", alumno.Nombres, DbType.String, size: 50);
        //        parameters.Add("@idtenor", alumno.IdTenor, DbType.Int32);
        //        parameters.Add("@genero", alumno.Sexo, DbType.Boolean);
        //        parameters.Add("@idcivil", alumno.IdCivil, DbType.Int32);
        //        parameters.Add("@email", alumno.Email, DbType.String, size: 100);
        //        parameters.Add("@emailopcional", string.IsNullOrWhiteSpace(alumno.EmailOpcional) ? null : alumno.EmailOpcional, DbType.String, size: 100);
        //        parameters.Add("@fechanacimiento", alumno.FechaNacimiento, DbType.Date);
        //        parameters.Add("@idpaisnacimiento", alumno.IdPaisNacimiento, DbType.Int32);
        //        parameters.Add("@idubigeonacimiento", string.IsNullOrWhiteSpace(alumno.IdUbigeoNacimiento) ? null : alumno.IdUbigeoNacimiento, DbType.String, size: 6);
        //        parameters.Add("@idubigeonacimientootro", string.IsNullOrWhiteSpace(alumno.IdUbigeoNacimientoOtro) ? null : alumno.IdUbigeoNacimientoOtro, DbType.String, size: 100);
        //        parameters.Add("@telefono", string.IsNullOrWhiteSpace(alumno.Telefono) ? null : alumno.Telefono, DbType.String, size: 100);
        //        parameters.Add("@celular", string.IsNullOrWhiteSpace(alumno.Celular) ? null : alumno.Celular, DbType.String, size: 100);
        //        parameters.Add("@telefonoreferencial", string.IsNullOrWhiteSpace(alumno.TelefonoReferencial) ? null : alumno.TelefonoReferencial, DbType.String, size: 100);
        //        parameters.Add("@direccion", alumno.Direccion, DbType.String, size: 500);
        //        parameters.Add("@urbanizacion", string.IsNullOrWhiteSpace(alumno.Urbanizacion) ? null : alumno.Urbanizacion, DbType.String, size: 100);
        //        parameters.Add("@direccionreferencia", string.IsNullOrWhiteSpace(alumno.DireccionReferencia) ? null : alumno.DireccionReferencia, DbType.String, size: 100);
        //        parameters.Add("@idpais", alumno.IdPais, DbType.Int32);
        //        parameters.Add("@idubigeo", string.IsNullOrWhiteSpace(alumno.IdUbigeo) ? null : alumno.IdUbigeo, DbType.String, size: 6);
        //        parameters.Add("@idubigeoresidenciaotro", string.IsNullOrWhiteSpace(alumno.IdUbigeoResidenciaOtro) ? null : alumno.IdUbigeoResidenciaOtro, DbType.String, size: 100);
        //        parameters.Add("@idnacionalidad", alumno.IdNacionalidad, DbType.Int32);
        //        parameters.Add("@codigopostal", string.IsNullOrWhiteSpace(alumno.CodigoPostal) ? null : alumno.CodigoPostal, DbType.String, size: 10);
        //        parameters.Add("@iddocumento", alumno.IdDocumento == 0 ? null : alumno.IdDocumento, DbType.Int32);
        //        parameters.Add("@numerodocumento", string.IsNullOrWhiteSpace(alumno.NumeroDocumento) ? null : alumno.NumeroDocumento, DbType.String, size: 20);
        //        parameters.Add("@ruc", string.IsNullOrWhiteSpace(alumno.Ruc) ? null : alumno.Ruc, DbType.String, size: 20);
        //        parameters.Add("@trabaja", alumno.Trabaja, DbType.Boolean);
        //        parameters.Add("@estado", alumno.Estado, DbType.StringFixedLength, size: 1);
        //        parameters.Add("@idperiodo", alumno.IdPeriodo, DbType.Int32);
        //        parameters.Add("@usuariocreacion", alumno.UsuarioCreacion, DbType.Int32);
        //        parameters.Add("@retval", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

        //        var command = new CommandDefinition(
        //            commandText: @"SELECT * FROM public.up_ins_actor_xalumno(
        //                @usuario, @contrasena, @paterno, @materno, @nombres, @idtenor, @genero, @idcivil, 
        //                @email, @emailopcional, @fechanacimiento, @idpaisnacimiento, @idubigeonacimiento, 
        //                @idubigeonacimientootro, @telefono, @celular, @telefonoreferencial, @direccion, 
        //                @urbanizacion, @direccionreferencia, @idpais, @idubigeo, @idubigeoresidenciaotro, 
        //                @idnacionalidad, @codigopostal, @iddocumento, @numerodocumento, @ruc, @trabaja, 
        //                @estado, @idperiodo, @usuariocreacion, @retval
        //            )",
        //            parameters: parameters,
        //            transaction: transaction,
        //            commandType: CommandType.Text,
        //            cancellationToken: cancellationToken
        //        );

        //        await transaction.Connection!.ExecuteAsync(command);

        //        var strDatos = parameters.Get<string>("@retval");
        //        if (string.IsNullOrEmpty(strDatos))
        //            throw new Exception("La función en PostgreSQL no devolvió el ID esperado en el parámetro OUTPUT.");

        //        var arrDatos = strDatos.Split('|');
        //        if (arrDatos.Length > 0 && long.TryParse(arrDatos[0], out long idActor))
        //        {
        //            return idActor;
        //        }

        //        throw new Exception("No se pudo obtener el IdActor desde la respuesta de la base de datos.");
        //    }
        //    catch (NpgsqlException ex)
        //    {
        //        // Aislamiento: No permitimos que la excepción de PostgreSQL bubujee con datos sensibles
        //        throw new DatabaseOperationException("Ocurrió un error a nivel de base de datos al intentar registrar al alumno.", ex);
        //    }
        //}

        public async Task<AlumnoResponseDto?> ObtenerPorIdAsync(long idActor, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

                var command = new CommandDefinition(
                    commandText: "SELECT * FROM public.up_sel_actor_xid(@IdActor)",
                    parameters: new { IdActor = idActor },
                    commandType: CommandType.Text,
                    cancellationToken: cancellationToken
                );
                return await connection.QueryFirstOrDefaultAsync<AlumnoResponseDto>(command);
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseOperationException("Ocurrió un error a nivel de base de datos al consultar el alumno.", ex);
            }
        }

        public async Task<IEnumerable<AlumnoListadoDto>> ListarAlumnosAsync(string codigo, string nombre, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

                var parameters = new DynamicParameters();
                parameters.Add("@codigo", string.IsNullOrWhiteSpace(codigo) ? "" : codigo, DbType.String, size: 20);
                parameters.Add("@nombre", string.IsNullOrWhiteSpace(nombre) ? "" : nombre, DbType.String, size: 200);

                var command = new CommandDefinition(
                    commandText: "SELECT * FROM public.up_sel_actor_xcontrol_xalumno(@codigo, @nombre)",
                    parameters: parameters,
                    commandType: CommandType.Text,
                    cancellationToken: cancellationToken
                );
                return await connection.QueryAsync<AlumnoListadoDto>(command);
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseOperationException("Ocurrió un error a nivel de base de datos al listar los alumnos.", ex);
            }
        }

        public async Task<int> EliminarAlumnoAsync(long idActor, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

                var parameters = new DynamicParameters();
                parameters.Add("@idactor", idActor, DbType.Int32);
                parameters.Add("@retval", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var command = new CommandDefinition(
                    commandText: "SELECT * FROM public.up_del_actor_xalumno(@idactor, @retval)",
                    parameters: parameters,
                    commandType: CommandType.Text,
                    cancellationToken: cancellationToken
                );
                await connection.ExecuteAsync(command);

                return parameters.Get<int>("@retval");
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseOperationException("Ocurrió un error a nivel de base de datos al eliminar el alumno.", ex);
            }
        }
    }
}