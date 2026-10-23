using Academic360.Application.Alumnos.Interfaces;
using Academic360.Application.Common.Interfaces;
using Academic360.Application.Matriculas.Carrera.Commands.RegistrarMatricula;
using Academic360.Application.Matriculas.Carrera.DTOs;
using Academic360.Application.Matriculas.Carrera.Enums;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Academic360.Infrastructure.Repositories
{
    public class AlumnoMatriculaRepository : IAlumnoMatriculaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AlumnoMatriculaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<MatriculaCarreraListadoDto>> ObtenerMatriculasAsync(int idPromocion, FiltroMatriculaEstado estado, string? searchTerm, int? idGrupo, int? idSeccion)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@idpromocion", idPromocion, DbType.Int32);
            parameters.Add("@estado", (int)estado, DbType.Int32);

            // CORRECCIÓN PARA POSTGRESQL (CommandType.Text): 
            // Siempre debemos agregar todos los parámetros a DynamicParameters, incluso si son nulos, 
            // porque la consulta SQL plana los referencia explícitamente en la llamada a la función.
            parameters.Add("@idgrupo", idGrupo, DbType.Int32);
            parameters.Add("@idseccion", idSeccion, DbType.Int32);
            parameters.Add("@nombrecompleto", string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm, DbType.String);

            var result = await connection.QueryAsync<MatriculaCarreraListadoDto>(
                "SELECT * FROM public.up_sel_alumnomatricula_listados_dinamico(@idpromocion, @estado, @idgrupo, @idseccion, @nombrecompleto)",
                parameters,
                commandType: CommandType.Text);

            return result;
        }

        public async Task<bool> ActualizarEstadoMatriculaAsync(int idAlumno, int idRegistro, int idMatricula, bool esMatricula)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@idalumno", idAlumno, DbType.Int32);
            parameters.Add("@idregistro", idRegistro, DbType.Int32);
            parameters.Add("@idmatricula", idMatricula, DbType.Int32);
            parameters.Add("@esmatricula", esMatricula, DbType.Boolean);

            await connection.ExecuteAsync(
                "SELECT public.up_upd_alumnomatricula_esmatricula(@idalumno, @idregistro, @idmatricula, @esmatricula)",
                parameters,
                commandType: CommandType.Text);

            return true;
        }


        public async Task<string> InsertarOActualizarActorAsync(RegistrarMatriculaCommand request, string contrasena, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            if (request.IdAlumno == -1)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@p_contrasena", contrasena);
                parameters.Add("@p_paterno", request.Paterno);
                parameters.Add("@p_materno", request.Materno);
                parameters.Add("@p_nombres", request.Nombres);
                parameters.Add("@p_idtenor", request.IdTenor);
                parameters.Add("@p_sexo", request.Sexo); // Dapper enviará el bool
                parameters.Add("@p_iddocumento", request.IdDocumento);
                parameters.Add("@p_numerodocumento", request.NumeroDocumento);
                parameters.Add("@p_idcivil", request.IdCivil);
                parameters.Add("@p_direccion", request.Direccion);
                parameters.Add("@p_urbanizacion", request.Urbanizacion);
                parameters.Add("@p_idpais", request.IdPais);
                parameters.Add("@p_idubigeo", request.IdUbigeo);
                parameters.Add("@p_telefono", request.Telefono);
                parameters.Add("@p_celular", request.Celular);
                parameters.Add("@p_fax", request.Fax);
                parameters.Add("@p_email", request.Email);
                parameters.Add("@p_idperiodo", request.IdPeriodo);
                parameters.Add("@p_usuariocreacion", request.UsuarioCreacion);

                // Agregamos ::boolean a @p_sexo
                var commandDefinition = new CommandDefinition("SELECT public.up_ins_actor_xmatricula(@p_contrasena, @p_paterno, @p_materno, @p_nombres, @p_idtenor, @p_sexo::boolean, @p_iddocumento, @p_numerodocumento, @p_idcivil, @p_direccion, @p_urbanizacion, @p_idpais, @p_idubigeo, @p_telefono, @p_celular, @p_fax, @p_email, @p_idperiodo, @p_usuariocreacion)", parameters, cancellationToken: cancellationToken);
                return await connection.QuerySingleAsync<string>(commandDefinition);
            }
            else
            {
                var parameters = new DynamicParameters();
                parameters.Add("@p_idactor", request.IdAlumno);
                parameters.Add("@p_paterno", request.Paterno);
                parameters.Add("@p_materno", request.Materno);
                parameters.Add("@p_nombres", request.Nombres);
                parameters.Add("@p_idtenor", request.IdTenor);
                parameters.Add("@p_sexo", request.Sexo); // Dapper enviará el bool
                parameters.Add("@p_iddocumento", request.IdDocumento);
                parameters.Add("@p_numerodocumento", request.NumeroDocumento);
                parameters.Add("@p_idcivil", request.IdCivil);
                parameters.Add("@p_direccion", request.Direccion);
                parameters.Add("@p_urbanizacion", request.Urbanizacion);
                parameters.Add("@p_idpais", request.IdPais);
                parameters.Add("@p_idubigeo", request.IdUbigeo);
                parameters.Add("@p_telefono", request.Telefono);
                parameters.Add("@p_celular", request.Celular);
                parameters.Add("@p_fax", request.Fax);
                parameters.Add("@p_email", request.Email);
                parameters.Add("@p_usuariomodificacion", request.UsuarioCreacion);

                // Agregamos ::boolean a @p_sexo
                var commandDefinition = new CommandDefinition("SELECT public.up_upd_actor_xmatricula(@p_idactor, @p_paterno, @p_materno, @p_nombres, @p_idtenor, @p_sexo::boolean, @p_iddocumento, @p_numerodocumento, @p_idcivil, @p_direccion, @p_urbanizacion, @p_idpais, @p_idubigeo, @p_telefono, @p_celular, @p_fax, @p_email, @p_usuariomodificacion)", parameters, cancellationToken: cancellationToken);
                await connection.ExecuteAsync(commandDefinition);
                return $"{request.IdAlumno}|{request.NumeroDocumento}";
            }
        }

        public async Task<int> InsertarRegistroAsync(int idAlumno, RegistrarMatriculaCommand request, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var parameters = new DynamicParameters();
            parameters.Add("@p_idalumno", idAlumno);
            parameters.Add("@p_idtipo", request.IdTipo);
            parameters.Add("@p_idsubtipo", request.IdSubTipo);
            parameters.Add("@p_idproducto", request.IdProducto);
            parameters.Add("@p_idpromocion", request.IdPromocion);
            parameters.Add("@p_idmedio", request.IdMedio);
            parameters.Add("@p_estado", "A");
            parameters.Add("@p_usuariocreacion", request.UsuarioCreacion);

            var commandDefinition = new CommandDefinition("SELECT public.up_ins_alumnoregistro(@p_idalumno, @p_idtipo, @p_idsubtipo, @p_idproducto, @p_idpromocion, @p_idmedio, @p_estado, @p_usuariocreacion)", parameters, cancellationToken: cancellationToken);
            return await connection.QuerySingleAsync<int>(commandDefinition);
        }

        public async Task<int> InsertarMatriculaAsync(int idAlumno, int idRegistro, RegistrarMatriculaCommand request, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var parameters = new DynamicParameters();
            parameters.Add("@p_idalumno", idAlumno);
            parameters.Add("@p_idregistro", idRegistro);
            parameters.Add("@p_idproducto", request.IdProducto);
            parameters.Add("@p_idpromocion", request.IdPromocion);
            parameters.Add("@p_idgrupo", request.IdGrupo);
            parameters.Add("@p_idcurso", -1);
            parameters.Add("@p_idseccion", -1);
            parameters.Add("@p_matriculafecha", request.MatriculaFecha);
            parameters.Add("@p_matriculausuario", request.UsuarioCreacion);
            parameters.Add("@p_idmedio", request.IdMedio);
            parameters.Add("@p_idbeca", request.IdBeca);
            parameters.Add("@p_esmatricula", false);
            parameters.Add("@p_estado", "A");
            parameters.Add("@p_usuariocreacion", request.UsuarioCreacion);

            var commandDefinition = new CommandDefinition("SELECT public.up_ins_alumnomatricula(@p_idalumno, @p_idregistro, @p_idproducto, @p_idpromocion, @p_idgrupo, @p_idcurso, @p_idseccion, @p_matriculafecha, @p_matriculausuario, @p_idmedio, @p_idbeca, @p_esmatricula, @p_estado, @p_usuariocreacion)", parameters, cancellationToken: cancellationToken);
            return await connection.QuerySingleAsync<int>(commandDefinition);
        }

        public async Task<int> InsertarAlumnoCursoAsync(int idAlumno, int idRegistro, int idMatricula, RegistrarMatriculaCommand request, CursoMatriculaDto curso, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var parameters = new DynamicParameters();
            parameters.Add("@p_idalumno", idAlumno);
            parameters.Add("@p_idregistro", idRegistro);
            parameters.Add("@p_idmatricula", idMatricula);
            parameters.Add("@p_idproducto", request.IdProducto);
            parameters.Add("@p_idpromocion", request.IdPromocion);
            parameters.Add("@p_idgrupo", request.IdGrupo);
            parameters.Add("@p_idcurso", curso.IdCurso);
            parameters.Add("@p_idseccion", curso.IdSeccion);
            parameters.Add("@p_esmatricula", false); // Dapper enviará el bool
            parameters.Add("@p_usuariocreacion", request.UsuarioCreacion);

            // Agregamos ::boolean a @p_esmatricula
            var commandDefinition = new CommandDefinition("SELECT public.up_ins_alumnocurso(@p_idalumno, @p_idregistro, @p_idmatricula, @p_idproducto, @p_idpromocion, @p_idgrupo, @p_idcurso, @p_idseccion, @p_esmatricula::boolean, @p_usuariocreacion)", parameters, cancellationToken: cancellationToken);
            return await connection.QuerySingleAsync<int>(commandDefinition);
        }

        public async Task<int> InsertarAlumnoCurriculaAsync(int idAlumno, int idRegistro, CursoMatriculaDto curso, int usuarioCreacion, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var parameters = new DynamicParameters();
            parameters.Add("@p_idalumno", idAlumno);
            parameters.Add("@p_idregistro", idRegistro);
            parameters.Add("@p_idmodulo", curso.IdModulo);
            parameters.Add("@p_idcurso", curso.IdCurso);
            parameters.Add("@p_usuariocreacion", usuarioCreacion);

            var commandDefinition = new CommandDefinition("SELECT public.up_ins_alumnocurricula(@p_idalumno, @p_idregistro, @p_idmodulo, @p_idcurso, @p_usuariocreacion)", parameters, cancellationToken: cancellationToken);
            return await connection.QuerySingleAsync<int>(commandDefinition);
        }

        public async Task InsertarAlumnoCurriculaCursoAsync(int idAlumno, int idRegistro, int idCurricula, int idAlumnoCurso, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var parameters = new DynamicParameters();
            parameters.Add("@p_idalumno", idAlumno);
            parameters.Add("@p_idregistro", idRegistro);
            parameters.Add("@p_idcurricula", idCurricula);
            parameters.Add("@p_idalumnocurso", idAlumnoCurso);

            var commandDefinition = new CommandDefinition("SELECT public.up_ins_alumnocurriculacurso(@p_idalumno, @p_idregistro, @p_idcurricula, @p_idalumnocurso)", parameters, cancellationToken: cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
        }

        public async Task InsertarProgramacionAsync(int idAlumno, int idMovimiento, CuotaProgramacionDto cuota, RegistrarMatriculaCommand request, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var parameters = new DynamicParameters();
            parameters.Add("@p_idfacultad", request.IdFacultad);
            parameters.Add("@p_idmovimiento", idMovimiento);
            parameters.Add("@p_idactor", idAlumno);
            parameters.Add("@p_cantidad", cuota.Cantidad);
            parameters.Add("@p_cuota", cuota.Cuota);
            parameters.Add("@p_cuotanumero", cuota.CuotaNumero);
            parameters.Add("@p_cuotas", cuota.Cuotas);
            parameters.Add("@p_totalcuotas", cuota.TotalCuotas);
            parameters.Add("@p_idconcepto", cuota.IdConcepto);
            parameters.Add("@p_idprecio", cuota.IdPrecio);
            parameters.Add("@p_idmoneda", request.IdMoneda);
            parameters.Add("@p_monto", cuota.Monto);
            parameters.Add("@p_idmora", cuota.IdMora);
            parameters.Add("@p_fechavencimiento", cuota.FechaVencimiento);
            parameters.Add("@p_escontado", cuota.EsContado);
            parameters.Add("@p_esinicial", cuota.EsInicial);
            parameters.Add("@p_escuota", cuota.EsCuota);
            parameters.Add("@p_esobligatorio", cuota.EsObligatorio);
            parameters.Add("@p_estado", "P");
            parameters.Add("@p_usuariocreacion", request.UsuarioCreacion);

            var commandDefinition = new CommandDefinition("SELECT public.up_ins_programacion(@p_idfacultad, @p_idmovimiento, @p_idactor, @p_cantidad, @p_cuota, @p_cuotanumero, @p_cuotas, @p_totalcuotas, @p_idconcepto, @p_idprecio, @p_idmoneda, @p_monto, @p_idmora, @p_fechavencimiento, @p_escontado, @p_esinicial, @p_escuota, @p_esobligatorio, @p_estado, @p_usuariocreacion)", parameters, cancellationToken: cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
        }

        public async Task<IEnumerable<MatriculaCarreraListadoDto>> ObtenerMatriculasPorProductoAsync(int idProducto, FiltroMatriculaEstado estado, string? searchTerm, int? idGrupo, int? idSeccion)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@idproducto", idProducto, DbType.Int32);
            parameters.Add("@estado", (int)estado, DbType.Int32);
            parameters.Add("@idgrupo", idGrupo, DbType.Int32);
            parameters.Add("@idseccion", idSeccion, DbType.Int32);
            parameters.Add("@nombrecompleto", string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm, DbType.String);

            // Nota: Reemplaza 'up_sel_alumnomatricula_xproducto' por el nombre exacto de la función en tu base de datos PostgreSQL si difiere
            var result = await connection.QueryAsync<MatriculaCarreraListadoDto>(
                "SELECT * FROM public.up_sel_alumnomatricula_xproducto(@idproducto, @estado, @idgrupo, @idseccion, @nombrecompleto)",
                parameters,
                commandType: CommandType.Text);

            return result;
        }
    }
}
