using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Alumnos.DTOs;
using Academic360.Application.Alumnos.Queries.ObtenerAlumnos;
using Academic360.Domain.Actores;

namespace Academic360.Application.Alumnos.Interfaces;

public interface IAlumnoRepository
{
    Task<long> InsertarAsync(Actor alumno, IDbTransaction transaction, CancellationToken cancellationToken = default);
    Task<AlumnoResponseDto?> ObtenerPorIdAsync(long idActor, CancellationToken cancellationToken = default);
    Task<IEnumerable<AlumnoListadoDto>> ListarAlumnosAsync(string codigo, string nombre, CancellationToken cancellationToken = default);
    Task<int> EliminarAlumnoAsync(long idActor, CancellationToken cancellationToken = default);
}