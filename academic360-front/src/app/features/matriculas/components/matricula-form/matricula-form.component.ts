import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import {
  MatriculaService,
  ProductoDto,
  PromocionDto,
  GrupoDto,
  MedioDto,
  PlanDto,
  MonedaDto,
  BecaDto
} from '../../services/matricula.service';

@Component({
  selector: 'app-matricula-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './matricula-form.component.html'
})
export class MatriculaFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private matriculaService = inject(MatriculaService);
  private router = inject(Router);

  matriculaForm!: FormGroup;
  isSubmitting = false;
  mensajeExito = '';
  mensajeError = '';

  // --- LISTAS DE CATÁLOGOS ---
  productos: ProductoDto[] = [];
  promociones: PromocionDto[] = [];
  grupos: GrupoDto[] = [];
  medios: MedioDto[] = [];
  planes: PlanDto[] = [];
  monedas: MonedaDto[] = [];
  becas: BecaDto[] = [];

  ngOnInit(): void {
    this.inicializarFormulario();
    this.cargarCatalogosEstaticos();
  }

  private inicializarFormulario(): void {
    this.matriculaForm = this.fb.group({
      idAlumno: [null, Validators.required],
      idProducto: [null, Validators.required],
      idPromocion: [{ value: null, disabled: true }, Validators.required],
      idGrupo: [{ value: null, disabled: true }, Validators.required],
      idMedio: [null, Validators.required],
      idBeca: [{ value: null, disabled: true }],
      idPlan: [{ value: null, disabled: true }, Validators.required],
      idMoneda: [null, Validators.required],
      usuarioCreacion: [1],
      cursos: this.fb.array([], Validators.required),
      cuotas: this.fb.array([], Validators.required)
    });

    this.agregarCurso();
    this.agregarCuota();
  }

  // --- GETTERS ARRAYS DINÁMICOS ---
  get cursos(): FormArray { return this.matriculaForm.get('cursos') as FormArray; }
  get cuotas(): FormArray { return this.matriculaForm.get('cuotas') as FormArray; }

  // --- CARGA DE DATOS (CASCADA) ---
  private cargarCatalogosEstaticos(): void {
    // 1. Cargamos medios y monedas de inmediato
    this.matriculaService.obtenerMedios().subscribe(res => this.medios = res);
    this.matriculaService.obtenerMonedas().subscribe(res => this.monedas = res);

    // 2. Cargamos productos. Asumimos que "Cursos Libres" tiene Tipo=2 y SubTipo=1 (según tu lógica)
    this.matriculaService.obtenerProductos(2, 1).subscribe(res => this.productos = res);
  }

  onProductoChange(): void {
    const idProducto = this.matriculaForm.get('idProducto')?.value;

    // Reseteamos dependientes
    this.matriculaForm.patchValue({ idPromocion: null, idGrupo: null, idPlan: null, idBeca: null });
    this.matriculaForm.get('idPromocion')?.disable();
    this.matriculaForm.get('idGrupo')?.disable();
    this.matriculaForm.get('idPlan')?.disable();
    this.matriculaForm.get('idBeca')?.disable();

    if (idProducto) {
      this.matriculaService.obtenerPromociones(idProducto).subscribe(res => {
        this.promociones = res;
        this.matriculaForm.get('idPromocion')?.enable();
      });
    }
  }

  onPromocionChange(): void {
    const idPromocion = this.matriculaForm.get('idPromocion')?.value;

    this.matriculaForm.patchValue({ idGrupo: null, idPlan: null, idBeca: null });
    this.matriculaForm.get('idGrupo')?.disable();
    this.matriculaForm.get('idPlan')?.disable();
    this.matriculaForm.get('idBeca')?.disable();

    if (idPromocion) {
      // Cargamos grupos y becas
      this.matriculaService.obtenerGrupos(idPromocion).subscribe(res => {
        this.grupos = res;
        this.matriculaForm.get('idGrupo')?.enable();
      });

      this.matriculaService.obtenerBecas(idPromocion).subscribe(res => {
        this.becas = res;
        this.matriculaForm.get('idBeca')?.enable();
      });
    }
  }

  onGrupoChange(): void {
    const idPromocion = this.matriculaForm.get('idPromocion')?.value;
    const idGrupo = this.matriculaForm.get('idGrupo')?.value;

    this.matriculaForm.patchValue({ idPlan: null });
    this.matriculaForm.get('idPlan')?.disable();

    if (idPromocion && idGrupo) {
      this.matriculaService.obtenerPlanes(idPromocion, idGrupo).subscribe(res => {
        this.planes = res;
        this.matriculaForm.get('idPlan')?.enable();
      });
    }
  }

  // --- MÉTODOS DE CELDAS DINÁMICAS ---
  agregarCurso(): void {
    this.cursos.push(this.fb.group({
      idModulo: [null, Validators.required],
      idCurso: [null, Validators.required],
      idSeccion: [null, Validators.required]
    }));
  }

  removerCurso(index: number): void {
    if (this.cursos.length > 1) this.cursos.removeAt(index);
  }

  agregarCuota(): void {
    this.cuotas.push(this.fb.group({
      idConcepto: [null, Validators.required],
      idPrecio: [null, Validators.required],
      precioDescripcion: ['', Validators.required],
      cuota: [1, Validators.min(0)],
      totalCuotas: [0, [Validators.required, Validators.min(0.01)]],
      precio: [0, [Validators.required, Validators.min(0.01)]],
      vencimiento: ['', Validators.required]
    }));
  }

  removerCuota(index: number): void {
    if (this.cuotas.length > 1) this.cuotas.removeAt(index);
  }

  // --- GUARDADO ---
  guardarMatricula(): void {
    if (this.matriculaForm.invalid) {
      this.matriculaForm.markAllAsTouched();
      this.mensajeError = 'Por favor, complete todos los campos obligatorios marcados en rojo.';
      return;
    }

    this.isSubmitting = true;
    this.mensajeError = '';

    // Mapeamos el payload asegurando que si no hay beca, se envíe -1 o null según tu backend
    const payload = { ...this.matriculaForm.getRawValue() };
    if (!payload.idBeca) payload.idBeca = -1;

    this.matriculaService.registrarMatriculaCursoLibre(payload).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        this.mensajeExito = `Matrícula registrada correctamente. ID: ${res.idMatricula}`;
        setTimeout(() => this.router.navigate(['/matriculas-libres']), 2000);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.mensajeError = err.error?.detail || 'Ocurrió un error al registrar la matrícula.';
      }
    });
  }
}
