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
  vencimiento: string; // Formato ISO 8601 (YYYY-MM-DD)
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
