import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

// --- INTERFACES DE LISTADO Y RESULTADOS ---
export interface AlumnoMatriculaListadoDto {
  idRegistro: number;
  idMatricula: number;
  idAlumno: number;
  nombreCompleto: string;
  grupoCodigo: string;
  seccion: string;
  matriculaFecha?: Date;
  matriculadoPor: string;
  esMatricula: boolean;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

// --- INTERFACES DE CATÁLOGOS ---
export interface SubTipoDto {
  idTipo: number;
  idSubTipo: number;
  subTipoNombre: string;
}

export interface ProductoDto {
  id: number;
  nombre: string;
}

export interface PromocionDto {
  id: number;
  codigo: string;
  nombre: string;
  dirigido: string;
}

export interface GrupoDto {
  id: number;
  codigo: string;
}

export interface MedioDto {
  idMedio: number;
  medioNombre: string;
}

export interface PlanDto {
  idPlan: number;
  planNombre: string;
  idMoneda: number;
  monedaNombre: string;
}

export interface MonedaDto {
  id: number;
  nombre: string;
}

export interface BecaDto {
  idBeca: number;
  beneficioNombre: string;
  descuento: number;
  descripcion: string;
}

// --- INTERFACES DE GUARDADO (CURSOS LIBRES) ---
export interface CursoMatricula {
  idModulo: number;
  idCurso: number;
  idSeccion: number;
}

export interface CuotaMatricula {
  idConcepto: number;
  idPrecio: number;
  precioDescripcion: string;
  cuota: number;
  totalCuotas: number;
  precio: number;
  vencimiento: string;
}

export interface CrearMatriculaRequest {
  idAlumno: number;
  idProducto: number;
  idPromocion: number;
  idGrupo: number;
  idMedio: number;
  idBeca: number;
  idPlan: number;
  idMoneda: number;
  usuarioCreacion: number;
  cursos: CursoMatricula[];
  cuotas: CuotaMatricula[];
}

@Injectable({
  providedIn: 'root'
})
export class MatriculaService {
  private http = inject(HttpClient);

  // Endpoints
  private apiMatriculas = `${environment.apiUrl}/api/matriculas`;
  private apiCatalogos = `${environment.apiUrl}/api/catalogos`;
  private apiCursosLibres = `${environment.apiUrl}/api/matriculas-cursos-libres`;

  // --- MÉTODOS DE CATÁLOGOS ---
  obtenerSubTipos(): Observable<SubTipoDto[]> {
    return this.http.get<SubTipoDto[]>(`${this.apiCatalogos}/subtipos`);
  }

  obtenerProductos(idTipo: number, idSubTipo: number): Observable<ProductoDto[]> {
    return this.http.get<ProductoDto[]>(`${this.apiCatalogos}/productos?idTipo=${idTipo}&idSubTipo=${idSubTipo}`);
  }

  obtenerPromociones(idProducto: number): Observable<PromocionDto[]> {
    return this.http.get<PromocionDto[]>(`${this.apiCatalogos}/promociones/${idProducto}`);
  }

  obtenerGrupos(idPromocion: number): Observable<GrupoDto[]> {
    return this.http.get<GrupoDto[]>(`${this.apiCatalogos}/grupos/${idPromocion}`);
  }

  obtenerPlanes(idPromocion: number, idGrupo: number): Observable<PlanDto[]> {
    return this.http.get<PlanDto[]>(`${this.apiCatalogos}/planes?idPromocion=${idPromocion}&idGrupo=${idGrupo}`);
  }

  obtenerMedios(): Observable<MedioDto[]> {
    return this.http.get<MedioDto[]>(`${this.apiCatalogos}/medios-comunicacion`);
  }

  obtenerMonedas(): Observable<MonedaDto[]> {
    return this.http.get<MonedaDto[]>(`${this.apiCatalogos}/monedas`);
  }

  obtenerBecas(idPromocion: number): Observable<BecaDto[]> {
    return this.http.get<BecaDto[]>(`${this.apiCatalogos}/becas/${idPromocion}`);
  }

  // --- MÉTODOS DE MATRÍCULAS (CARRERA) ---
  obtenerPorPromocion(
    idPromocion: number,
    estado: number,
    searchTerm: string = '',
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<AlumnoMatriculaListadoDto>> {
    let params = new HttpParams()
      .set('estado', estado.toString())
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<PagedResult<AlumnoMatriculaListadoDto>>(`${this.apiMatriculas}/promocion/${idPromocion}`, { params });
  }

  actualizarEstado(idAlumno: number, idRegistro: number, idMatricula: number, esMatricula: boolean): Observable<any> {
    const command = { idAlumno, idRegistro, idMatricula, esMatricula };
    return this.http.put(`${this.apiMatriculas}/estado`, command);
  }

  // --- NUEVOS MÉTODOS (CURSOS LIBRES) ---
  registrarMatriculaCursoLibre(matricula: CrearMatriculaRequest): Observable<{ message: string, idMatricula: number }> {
    return this.http.post<{ message: string, idMatricula: number }>(this.apiCursosLibres, matricula);
  }
}
