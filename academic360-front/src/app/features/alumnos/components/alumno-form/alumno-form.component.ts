import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AlumnoService, TenorDto } from '../../services/alumno.service';
import { CrearAlumnoRequestDto } from '../../models/alumno.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-alumno-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './alumno-form.component.html',
  styleUrls: ['./alumno-form.component.css']
})
export class AlumnoFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly alumnoService = inject(AlumnoService);

  // Signals para estado
  isSubmitting = signal<boolean>(false);
  errorMessage = signal<string>('');
  successMessage = signal<string>('');
  requiresHomologoConfirmation = signal<boolean>(false);

  // Lista real para alimentar el selector de tenores
  tenoresList: any[] = []; // Lo pasamos temporalmente a any[] para evitar errores de tipado si cambió el DTO

  // Signal para manejar las pestañas
  activeTab = signal<number>(1);

  alumnoForm: FormGroup = this.fb.group({
    usuario: ['', Validators.required],
    contrasena: ['', Validators.required],
    paterno: ['', [Validators.required, Validators.maxLength(50)]],
    materno: ['', Validators.maxLength(50)],
    nombres: ['', [Validators.required, Validators.maxLength(50)]],
    // Mantenemos la regla: debe ser requerido y mayor a 0
    idTenor: [0, [Validators.required, Validators.min(1)]],
    sexo: [true, Validators.required],
    idCivil: [0, [Validators.required, Validators.min(1)]],
    email: ['', [Validators.required, Validators.email]],
    emailOpcional: ['', Validators.email],
    fechaNacimiento: ['', Validators.required],
    idPaisNacimiento: [166, [Validators.required, Validators.min(1)]],
    idUbigeoNacimiento: [''],
    idUbigeoNacimientoOtro: [''],
    telefono: [''],
    celular: [''],
    telefonoReferencial: [''],
    direccion: ['', Validators.required],
    urbanizacion: [''],
    direccionReferencia: [''],
    idPais: [166, [Validators.required, Validators.min(1)]],
    idUbigeo: [''],
    idUbigeoResidenciaOtro: [''],
    idNacionalidad: [166, Validators.min(1)],
    codigoPostal: [''],
    idDocumento: [0, Validators.min(1)],
    numeroDocumento: [''],
    ruc: ['', [Validators.pattern('^\\d{11}$')]],
    trabaja: [false],
    estado: ['A'],
    idPeriodo: [1]
  });

  ngOnInit(): void {
    this.cargarTenores();
  }

  cargarTenores(): void {
    this.alumnoService.obtenerTenores().subscribe({
      next: (data) => {
        // CAZADOR DE PROPIEDADES: Esto imprimirá en tu consola (F12) lo que realmente manda el backend
        console.log('👀 TENORES RECIBIDOS DEL BACKEND:', data);
        this.tenoresList = data;
      },
      error: (err) => {
        this.errorMessage.set('Error al cargar el catálogo de tenores desde el servidor.');
        console.error(err);
      }
    });
  }

  setTab(tabIndex: number) {
    this.activeTab.set(tabIndex);
  }

  nextTab() {
    if (this.activeTab() < 4) {
      this.activeTab.set(this.activeTab() + 1);
    }
  }

  previousTab() {
    if (this.activeTab() > 1) {
      this.activeTab.set(this.activeTab() - 1);
    }
  }

  onSubmit(forceHomologo: boolean = false) {
    // Imprimir qué valores tiene el formulario justo al presionar Guardar
    console.log('Valores actuales del formulario:', this.alumnoForm.value);

    if (this.alumnoForm.invalid) {
      this.alumnoForm.markAllAsTouched();

      const camposInvalidos: string[] = [];
      Object.keys(this.alumnoForm.controls).forEach(key => {
        const control = this.alumnoForm.get(key);
        if (control?.invalid) {
          camposInvalidos.push(key);
          console.log(`❌ Error de validación en el campo '${key}':`, control.errors, 'Valor actual:', control.value);
        }
      });

      this.errorMessage.set(`Por favor, complete o corrija los siguientes campos: ${camposInvalidos.join(', ')}`);
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');
    this.requiresHomologoConfirmation.set(false);

    const formData: CrearAlumnoRequestDto = this.alumnoForm.value;

    this.alumnoService.crearAlumno(formData, forceHomologo).subscribe({
      next: (response: { id: number }) => {
        this.successMessage.set(`¡Alumno registrado correctamente! ID Actor generado: ${response.id}`);
        this.alumnoForm.reset({ idTenor: 0, sexo: true, idPaisNacimiento: 166, idPais: 166, idNacionalidad: 166, estado: 'A', idPeriodo: 1, trabaja: false, idDocumento: 0, idCivil: 0 });
        this.isSubmitting.set(false);
        this.activeTab.set(1);
      },
      error: (error: HttpErrorResponse) => {
        this.isSubmitting.set(false);
        if (error.status === 409) {
          this.errorMessage.set('Atención: El alumno ya está registrado (Homónimo). ¿Desea guardarlo de todas formas?');
          this.requiresHomologoConfirmation.set(true);
        } else if (error.status === 400 && error.error?.errors) {
          const validationErrors = Object.values(error.error.errors).flat().join(' | ');
          this.errorMessage.set(`Validación fallida: ${validationErrors}`);
        } else {
          this.errorMessage.set(`Error del Servidor (${error.status}): ${error.error?.detail || error.message}`);
        }
      }
    });
  }

  confirmarHomologo() {
    this.onSubmit(true);
  }

  cancelarHomologo() {
    this.requiresHomologoConfirmation.set(false);
    this.errorMessage.set('');
  }
}
