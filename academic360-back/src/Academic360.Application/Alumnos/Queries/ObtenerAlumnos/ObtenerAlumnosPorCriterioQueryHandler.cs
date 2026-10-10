using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Academic360.Application.Alumnos.Interfaces;
using Academic360.Application.Common.Models;
using MediatR;

namespace Academic360.Application.Alumnos.Queries.ObtenerAlumnos;

public class ObtenerAlumnosPorCriterioQueryHandler : IRequestHandler<ObtenerAlumnosPorCriterioQuery, PagedResult<AlumnoListadoDto>>
{
    private readonly IAlumnoRepository _alumnoRepository;

    public ObtenerAlumnosPorCriterioQueryHandler(IAlumnoRepository alumnoRepository)
    {
        _alumnoRepository = alumnoRepository;
    }

    public async Task<PagedResult<AlumnoListadoDto>> Handle(ObtenerAlumnosPorCriterioQuery request, CancellationToken cancellationToken)
    {
        // Ejecutamos el SP heredado trayendo todos los registros (parámetros vacíos)
        var resultados = await _alumnoRepository.ListarAlumnosAsync(string.Empty, string.Empty, cancellationToken);
        var lista = resultados.ToList();

        // Aplicamos el filtro global en memoria
        if (!string.IsNullOrWhiteSpace(request.FiltroGlobal))
        {
            var filtro = request.FiltroGlobal.ToLower();
            lista = lista.Where(x =>
                (x.Codigo != null && x.Codigo.ToLower().Contains(filtro)) ||
                (x.NombreCompleto != null && x.NombreCompleto.ToLower().Contains(filtro)) ||
                (x.Telefono != null && x.Telefono.ToLower().Contains(filtro)) ||
                (x.Email != null && x.Email.ToLower().Contains(filtro)) ||
                (x.EstadoNombre != null && x.EstadoNombre.ToLower().Contains(filtro))
            ).ToList();
        }

        var totalCount = lista.Count;
        var items = lista
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return new PagedResult<AlumnoListadoDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}