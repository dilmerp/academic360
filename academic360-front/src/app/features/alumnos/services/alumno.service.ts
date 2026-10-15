import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ActualizarEstadoMatriculaRequestDto, AlumnoResponseDto, CrearAlumnoRequestDto } from '../models/alumno.model';

export interface AlumnoListadoDto {
  idActor: number;
  codigo: string;
  nombreCompleto: string;
  telefono: string;
  email: string;
  estado: string;
  estadoNombre: string;
}

// Nueva interfaz para soportar la respuesta paginada del backend
export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

// Interfaz para el catálogo de tenores
export interface TenorDto {
  idTenor: number;
  tenorCodigo: string;
  tenorNombre: string;
  activo: number;
}

@Injectable({
  providedIn: 'root'
})
export class AlumnoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/alumnos`;
  private readonly catalogosUrl = `${environment.apiUrl}/catalogos`; // Apunta correctamente a /api/catalogos

  crearAlumno(alumno: CrearAlumnoRequestDto, forceHomologo: boolean = false): Observable<{ id: number }> {
    let params = new HttpParams();
    if (forceHomologo) {
      params = params.set('forceHomologo', 'true');
    }
    return this.http.post<{ id: number }>(this.baseUrl, alumno, { params });
  }

  obtenerAlumnoPorId(id: number): Observable<AlumnoResponseDto> {
    return this.http.get<AlumnoResponseDto>(`${this.baseUrl}/${id}`);
  }

  actualizarEstadoMatricula(idAlumno: number, idRegistro: number, idMatricula: number, request: ActualizarEstadoMatriculaRequestDto): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${idAlumno}/registros/${idRegistro}/matriculas/${idMatricula}/estado`, request);
  }

  // --- MÉTODOS PARA EL LISTADO PAGINADO Y BÚSQUEDA GLOBAL ---

  listarAlumnos(filtroGlobal: string = '', pageNumber: number = 1, pageSize: number = 10): Observable<PagedResult<AlumnoListadoDto>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (filtroGlobal) {
      params = params.set('filtroGlobal', filtroGlobal);
    }

    return this.http.get<PagedResult<AlumnoListadoDto>>(this.baseUrl, { params });
  }

  eliminarAlumno(idActor: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${idActor}`);
  }

  // --- CATÁLOGO DE TENORES (Apunta correctamente a /api/catalogos/tenor) ---
  obtenerTenores(): Observable<TenorDto[]> {
    return this.http.get<TenorDto[]>(`${this.catalogosUrl}/tenor`);
  }
}
