export interface CrearAlumnoRequestDto {
  usuario: string;
  contrasena: string;
  paterno: string;
  materno: string;
  nombres: string;
  idTenor: number;
  sexo: boolean;
  idCivil: number;
  email: string;
  emailOpcional?: string | null;
  fechaNacimiento: string;
  idPaisNacimiento: number;
  idUbigeoNacimiento: string;
  idUbigeoNacimientoOtro?: string | null;
  telefono?: string | null;
  celular?: string | null;
  telefonoReferencial?: string | null;
  direccion: string;
  urbanizacion?: string | null;
  direccionReferencia?: string | null;
  idPais: number;
  idUbigeo: string;
  idUbigeoResidenciaOtro?: string | null;
  idNacionalidad: number;
  codigoPostal?: string | null;
  idDocumento: number;
  numeroDocumento: string;
  ruc?: string | null;
  trabaja: boolean;
  estado: string;
  idPeriodo: number;
  foto?: string | null;
}

export interface AlumnoResponseDto {
  idActor: number;
  codigo: string;
  paterno: string;
  materno: string;
  nombres: string;
  nombreCompleto: string;
  idTenor: number;
  tenorCodigo: string;
  genero: boolean;
  idCivil: number;
  civilNombre: string;
  email: string;
  emailOpcional: string;
  fechaNacimiento: string;
  idPaisNacimiento: number;
  paisNacimiento: string;
  idUbigeoNacimiento: string;
  distritoNombreNacimiento: string;
  telefono: string;
  celular: string;
  telefonoReferencial: string;
  direccion: string;
  urbanizacion: string;
  direccionReferencia: string;
  idDocumento: number;
  documentoCodigo: string;
  numeroDocumento: string;
  ruc: string;
  trabaja: boolean;
  idNacionalidad: number;
  nacionalidadNombre: string;
  idPaisResidencia: number;
  paisResidencia: string;
  idUbigeoResidencia: string;
  distritoNombreResidencia: string;
  estado: string;
  estadoNombre: string;
  idFamilia: number;
}

export interface ActualizarEstadoMatriculaRequestDto {
  esMatricula: boolean;
}
