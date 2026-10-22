import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import {
  MatriculaService, ProductoDto, PromocionDto, GrupoDto,
  MedioDto, MonedaDto, PlanDto, BecaDto
} from '../../services/matricula.service';
import { RegistrarMatriculaCommand } from '../../models/matricula.model';

@Component({
  selector: 'app-matricula-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterLink],
  templateUrl: './matricula-form.component.html'
})
export class MatriculaFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private matriculaService = inject(MatriculaService);

  matriculaForm!: FormGroup;

  // Catálogos
  productos: ProductoDto[] = [];
  promociones: PromocionDto[] = [];
  grupos: GrupoDto[] = [];
  medios: MedioDto[] = [];
  planes: PlanDto[] = [];
  monedas: MonedaDto[] = [];
  becas: BecaDto[] = [];
  cursosDisponibles: any[] = [];

  // Estados de la vista
  mensajeError: string = '';
  mensajeExito: string = '';
  isSubmitting: boolean = false;

  // Búsqueda de Alumno
  mostrarModalBusqueda: boolean = false;
  buscando: boolean = false;
  filtroBusqueda: string = '';
  resultadosBusqueda: any[] = [];
  alumnoSeleccionadoNombre: string = '';
  alumnoSeleccionadoObj: any = null;

  ngOnInit(): void {
    this.initForm();
    this.cargarCatalogosIniciales();
  }

  private initForm(): void {
    this.matriculaForm = this.fb.group({
      idAlumno: [null, Validators.required],
      idProducto: [null, Validators.required],
      idPromocion: [null, Validators.required],
      idGrupo: [null, Validators.required],
      idMedio: [null, Validators.required],
      idPlan: [null, Validators.required],
      idMoneda: [null, Validators.required],
      idBeca: [null],
      cursos: this.fb.array([]),
      cuotas: this.fb.array([])
    });

    this.agregarCurso();
    this.agregarCuota();
  }

  private cargarCatalogosIniciales(): void {
    this.matriculaService.obtenerProductos(3, 17).subscribe({
      next: (data) => this.productos = data,
      error: (err) => console.error('Error al cargar productos', err)
    });

    this.matriculaService.obtenerMedios().subscribe({
      next: (data) => this.medios = data,
      error: (err) => console.error('Error al cargar medios', err)
    });

    this.matriculaService.obtenerMonedas().subscribe({
      next: (data) => this.monedas = data,
      error: (err) => console.error('Error al cargar monedas', err)
    });
  }

  onProductoChange(): void {
    const idProducto = this.matriculaForm.get('idProducto')?.value;

    this.promociones = [];
    this.grupos = [];
    this.planes = [];
    this.becas = [];
    this.cursosDisponibles = [];
    this.matriculaForm.patchValue({ idPromocion: null, idGrupo: null, idPlan: null, idBeca: null });

    if (idProducto) {
      this.matriculaService.obtenerPromociones(idProducto).subscribe({
        next: (data) => this.promociones = data,
        error: (err) => console.error('Error al cargar promociones', err)
      });

      this.matriculaService.obtenerCursosSeccionesDisponibles(17, idProducto, '').subscribe({
        next: (data) => {
          this.cursosDisponibles = data.map((item: any) => ({
            ...item,
            idModulo: item.idModulo !== undefined ? item.idModulo : (item.IdModulo || 0),
            idCurso: item.idCurso !== undefined ? item.idCurso : (item.IdCurso || 0),
            idSeccion: item.idSeccion !== undefined ? item.idSeccion : (item.IdSeccion || 0),
            moduloNombre: item.moduloNombre || item.ModuloNombre || '[Ninguno]',
            nombre: item.nombre || item.Nombre || '[Sin Curso]',
            codigo: item.codigo || item.Codigo || '[Sin Sección]'
          }));
        },
        error: (err) => console.error('Error al cargar cursos disponibles', err)
      });
    }
  }

  onPromocionChange(): void {
    const idPromocion = this.matriculaForm.get('idPromocion')?.value;

    this.grupos = [];
    this.planes = [];
    this.matriculaForm.patchValue({ idGrupo: null, idPlan: null, idBeca: null });

    if (idPromocion) {
      this.matriculaService.obtenerGrupos(idPromocion).subscribe({
        next: (data) => this.grupos = data,
        error: (err) => console.error('Error al cargar grupos', err)
      });

      this.matriculaService.obtenerBecas(idPromocion).subscribe({
        next: (data) => this.becas = data,
        error: (err) => console.error('Error al cargar becas', err)
      });
    }
  }

  onGrupoChange(): void {
    const idPromocion = this.matriculaForm.get('idPromocion')?.value;
    const idGrupo = this.matriculaForm.get('idGrupo')?.value;

    this.planes = [];
    this.matriculaForm.patchValue({ idPlan: null });

    if (idPromocion && idGrupo) {
      this.matriculaService.obtenerPlanes(idPromocion, idGrupo, -1).subscribe({
        next: (data) => this.planes = data,
        error: (err) => console.error('Error al cargar planes', err)
      });
    }
  }

  get cursos(): FormArray {
    return this.matriculaForm.get('cursos') as FormArray;
  }

  agregarCurso(): void {
    const cursoGroup = this.fb.group({
      idModulo: [-1],
      idCurso: [null, Validators.required],
      idSeccion: [null, Validators.required]
    });
    this.cursos.push(cursoGroup);
  }

  removerCurso(index: number): void {
    if (this.cursos.length > 1) {
      this.cursos.removeAt(index);
    }
  }

  get cuotas(): FormArray {
    return this.matriculaForm.get('cuotas') as FormArray;
  }

  agregarCuota(): void {
    const cuotaGroup = this.fb.group({
      idConcepto: [null, Validators.required],
      precioDescripcion: ['', Validators.required],
      cuota: [1, Validators.required],
      totalCuotas: [0, Validators.required],
      precio: [0, Validators.required],
      vencimiento: ['', Validators.required]
    });
    this.cuotas.push(cuotaGroup);
  }

  removerCuota(index: number): void {
    if (this.cuotas.length > 1) {
      this.cuotas.removeAt(index);
    }
  }

  buscarAlumno(): void {
    if (this.filtroBusqueda.length < 3) return;
    this.buscando = true;
    this.mensajeError = '';

    this.matriculaService.buscarAlumnos(this.filtroBusqueda).subscribe({
      next: (data) => {
        this.resultadosBusqueda = data;
        this.buscando = false;
      },
      error: (err) => {
        this.buscando = false;
        this.mensajeError = 'Error al buscar el alumno.';
        console.error(err);
      }
    });
  }

  seleccionarAlumno(alumno: any): void {
    this.alumnoSeleccionadoObj = alumno;

    const idReal = alumno.idAlumno || alumno.idactor || alumno.idalumno || alumno.id;

    this.matriculaForm.patchValue({
        idAlumno: idReal
    });

    const documento = alumno.dni || alumno.numeroDocumento || '';
    const nombres = alumno.nombresCompletos || alumno.nombres || '';
    this.alumnoSeleccionadoNombre = `${documento} - ${nombres}`;

    this.mostrarModalBusqueda = false;
    this.filtroBusqueda = '';
    this.resultadosBusqueda = [];
  }

  guardarMatricula(): void {
    if (this.matriculaForm.invalid || !this.alumnoSeleccionadoObj) {
      this.matriculaForm.markAllAsTouched();
      this.mensajeError = 'Por favor, complete todos los campos obligatorios y busque un alumno.';
      return;
    }

    this.isSubmitting = true;
    this.mensajeError = '';
    this.mensajeExito = '';

    const formValues = this.matriculaForm.getRawValue();

    const payload: RegistrarMatriculaCommand = {
      idAlumno: formValues.idAlumno || this.alumnoSeleccionadoObj.idAlumno || this.alumnoSeleccionadoObj.idActor || this.alumnoSeleccionadoObj.id || -1,
      paterno: this.alumnoSeleccionadoObj.paterno || '',
      materno: this.alumnoSeleccionadoObj.materno || '',
      nombres: this.alumnoSeleccionadoObj.nombres || this.alumnoSeleccionadoObj.nombresCompletos || '',
      idTenor: this.alumnoSeleccionadoObj.idTenor || this.alumnoSeleccionadoObj.idtenor || -1,
      sexo: this.alumnoSeleccionadoObj.sexo === 'M' || this.alumnoSeleccionadoObj.genero === 1,
      idDocumento: this.alumnoSeleccionadoObj.idDocumento || this.alumnoSeleccionadoObj.iddocumento || -1,
      numeroDocumento: this.alumnoSeleccionadoObj.numeroDocumento || this.alumnoSeleccionadoObj.dni || '',
      idCivil: this.alumnoSeleccionadoObj.idCivil || this.alumnoSeleccionadoObj.idcivil || -1,
      direccion: this.alumnoSeleccionadoObj.direccion || '',
      urbanizacion: this.alumnoSeleccionadoObj.urbanizacion || '',
      idPais: this.alumnoSeleccionadoObj.idPais || this.alumnoSeleccionadoObj.idpais || -1,
      idUbigeo: this.alumnoSeleccionadoObj.idUbigeo || this.alumnoSeleccionadoObj.idubigeo || '',
      telefono: this.alumnoSeleccionadoObj.telefono || '',
      celular: this.alumnoSeleccionadoObj.celular || '',
      fax: this.alumnoSeleccionadoObj.telefonoReferencial || this.alumnoSeleccionadoObj.fax || '',
      email: this.alumnoSeleccionadoObj.email || '',
      idPeriodo: 1,

      idTipo: 3,
      idSubTipo: 17,
      idProducto: formValues.idProducto,
      idPromocion: formValues.idPromocion,
      idGrupo: formValues.idGrupo,
      idMedio: formValues.idMedio,
      idBeca: formValues.idBeca || -1,
      idPlan: formValues.idPlan,
      idMoneda: formValues.idMoneda,
      matriculaFecha: new Date().toISOString(),

      usuarioCreacion: 1,
      idFacultad: 1,
      idMovimiento: 0,

      // Extracción 100% segura forzando números y respetando los ceros
      cursos: formValues.cursos.map((c: any) => {
        const cur = Number(c.idCurso);
        const sec = Number(c.idSeccion);

        return {
          idModulo: -1, // Forzado fijo según la regla del sistema legacy
          idCurso: isNaN(cur) ? 0 : cur,
          idSeccion: isNaN(sec) ? 0 : sec
        };
      }),

      cuotas: formValues.cuotas.map((c: any, index: number) => ({
        cantidad: 1,
        cuota: Number(c.cuota) || (index + 1),
        cuotaNumero: Number(c.cuota) || (index + 1),
        precioDescripcion: c.precioDescripcion || `Cuota ${index + 1}`,
        cuotas: formValues.cuotas.length,
        totalCuotas: formValues.cuotas.length,
        idConcepto: c.idConcepto || 1,
        idPrecio: c.idPrecio || 1,
        monto: Number(c.precio || 0),
        idMora: 1,
        fechaVencimiento: new Date(c.vencimiento).toISOString(),
        esContado: false,
        esInicial: index === 0,
        esCuota: true,
        esObligatorio: true
      }))
    };

    console.log("PAYLOAD ENVIADO AL BACKEND:", payload);

    this.matriculaService.registrarMatricula(payload).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        this.mensajeExito = `Matrícula registrada correctamente para el alumno con ID: ${payload.idAlumno}`;

        this.matriculaForm.reset();
        this.alumnoSeleccionadoNombre = '';
        this.alumnoSeleccionadoObj = null;
        this.cursos.clear();
        this.cuotas.clear();
        this.agregarCurso();
        this.agregarCuota();
      },
      error: (err) => {
        this.isSubmitting = false;
        this.mensajeError = 'Ocurrió un error al registrar la matrícula. ' + (err.error?.mensaje || '');
        console.error(err);
      }
    });
  }
}
