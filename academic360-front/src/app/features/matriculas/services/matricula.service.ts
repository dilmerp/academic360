import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

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

export interface SubTipoDto {
  idTipo: number;
  idSubTipo: number;
  subTipoNombre: string; // <-- CORRECCIÓN: Actualizado para coincidir con tu Backend
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

@Injectable({
  providedIn: 'root'
})
export class MatriculaService {
  private http = inject(HttpClient);
  private apiMatriculas = `${environment.apiUrl}/matriculas`;
  private apiCatalogos = `${environment.apiUrl}/catalogos`;

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
}
