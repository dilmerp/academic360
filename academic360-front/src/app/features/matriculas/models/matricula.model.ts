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

export interface MatriculaLibreListDto {
  idRegistro: number;
  idMatricula: number;
  codigo: string;
  alumno: string;
  curso: string;
  fecha: string;
}
