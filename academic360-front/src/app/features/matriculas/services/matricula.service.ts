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

export interface MatriculaLibreListDto {
  idRegistro: number;
  idMatricula: number;
  codigo: string;
  alumno: string;
  curso: string;
  fecha: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

// --- INTERFACES DE CATÁLOGOS EXACTAS AL BACKEND ---
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
  idGrupo: number;
  grupoCodigo: string;
  grupoNombre: string;
  idTipo: number;
  idSubTipo: number;
  idProducto: number;
  idModulo: number;
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
  idMoneda: number;
  monedaCodigo: string;
  monedaNombre: string;
}

export interface BecaDto {
  idBeneficio: number;
  beneficioNombre: string;
  beneficioDescuento: number;
}

// --- INTERFACES DE GUARDADO TRANSACTION SCOPE ---
export interface CursoMatriculaDto {
  idModulo: number;
  idCurso: number;
  idSeccion: number;
}

export interface CuotaProgramacionDto {
  cantidad: number;
  cuota: number;
  cuotaNumero: number;
  precioDescripcion: string;
  cuotas: number;
  totalCuotas: number;
  idConcepto: number;
  idPrecio: number;
  monto: number;
  idMora: number;
  fechaVencimiento: string;
  esContado: boolean;
  esInicial: boolean;
  esCuota: boolean;
  esObligatorio: boolean;
}

export interface RegistrarMatriculaCommand {
  idAlumno: number;
  paterno: string;
  materno: string;
  nombres: string;
  idTenor: number;
  sexo: boolean;
  idDocumento: number;
  numeroDocumento: string;
  idCivil: number;
  direccion: string;
  urbanizacion: string;
  idPais: number;
  idUbigeo: string;
  telefono: string;
  celular: string;
  fax: string;
  email: string;
  idPeriodo: number;

  idTipo: number;
  idSubTipo: number;
  idProducto: number;
  idPromocion: number;
  idGrupo: number;
  idMedio: number;
  idBeca: number;
  idPlan: number;
  idMoneda: number;
  matriculaFecha: string;

  usuarioCreacion: number;
  idFacultad: number;
  idMovimiento: number;

  cursos: CursoMatriculaDto[];
  cuotas: CuotaProgramacionDto[];
}

@Injectable({
  providedIn: 'root'
})
export class MatriculaService {
  private http = inject(HttpClient);

  // Endpoints
  private apiMatriculas = `${environment.apiUrl}/matriculas`;
  private apiCatalogos = `${environment.apiUrl}/catalogos`;
  private apiMatriculasLibres = `${environment.apiUrl}/matriculas-cursos-libres`;

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

  obtenerPlanes(idPromocion: number, idGrupo: number, idSeccion: number = -1): Observable<PlanDto[]> {
    return this.http.get<PlanDto[]>(`${this.apiCatalogos}/planes?idPromocion=${idPromocion}&idGrupo=${idGrupo}&idSeccion=${idSeccion}`);
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

  // --- MÉTODOS DE MATRÍCULAS ---
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

  // --- BÚSQUEDA DE ALUMNOS ---
  buscarAlumnos(filtro: string): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/alumnos/buscar?filtro=${filtro}`);
  }

  // --- MÉTODOS DE CURSOS LIBRES ---
  obtenerCursosSeccionesDisponibles(idSubTipo: number, idProducto: number, texto: string = ''): Observable<any[]> {
    let params = new HttpParams()
      .set('idSubTipo', idSubTipo.toString())
      .set('idProducto', idProducto.toString())
      .set('texto', texto);

    return this.http.get<any[]>(`${this.apiCatalogos}/cursos-disponibles`, { params });
  }

  registrarMatricula(command: RegistrarMatriculaCommand): Observable<any> {
    return this.http.post<any>(this.apiMatriculasLibres, command);
  }

  listarMatriculasLibres(filtro: string = ''): Observable<MatriculaLibreListDto[]> {
    const params = new HttpParams().set('filtro', filtro);
    return this.http.get<MatriculaLibreListDto[]>(this.apiMatriculasLibres, { params });
  }
}
