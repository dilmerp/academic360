import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatriculaService, AlumnoMatriculaListadoDto, SubTipoDto, ProductoDto, PromocionDto } from '../services/matricula.service';

@Component({
  selector: 'app-matricula-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './matricula-list.component.html'
})
export class MatriculaListComponent implements OnInit {
  private matriculaService = inject(MatriculaService);

  alumnos: AlumnoMatriculaListadoDto[] = [];

  // Listas de catálogos
  subTipos: SubTipoDto[] = [];
  productos: ProductoDto[] = [];
  promociones: PromocionDto[] = [];

  // Selecciones actuales (Mapean el lblIdTipo, lblIdSubTipo, lblIdProducto, lblIdPromocion de VB6)
  subTipoSeleccionado: SubTipoDto | null = null;
  idProductoSeleccionado: number = 0;
  idPromocionSeleccionada: number = 0;
  promocionActual: PromocionDto | null = null; // Para pintar nombre y dirigido

  // Estado de la grilla
  estadoFiltro: number = 0; // 0: Nuevos, 1: Matriculados, 2: Todos
  filtroGlobal: string = '';

  // Paginación
  pageNumber: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  totalPages: number = 0;

  mensajeError: string = '';

  ngOnInit(): void {
    this.cargarSubTipos();
  }

  // --- LÓGICA DE COMBOS EN CASCADA ---
  cargarSubTipos(): void {
    this.matriculaService.obtenerSubTipos().subscribe({
      next: (data) => this.subTipos = data,
      error: (err) => console.error('Error al cargar subtipos', err)
    });
  }

  onSubTipoChange(): void {
    this.productos = [];
    this.promociones = [];
    this.idProductoSeleccionado = 0;
    this.idPromocionSeleccionada = 0;
    this.promocionActual = null;
    this.alumnos = [];

    if (this.subTipoSeleccionado) {
      this.matriculaService.obtenerProductos(this.subTipoSeleccionado.idTipo, this.subTipoSeleccionado.idSubTipo)
        .subscribe({
          next: (data) => this.productos = data,
          error: (err) => console.error('Error al cargar productos', err)
        });
    }
  }

  onProductoChange(): void {
    this.promociones = [];
    this.idPromocionSeleccionada = 0;
    this.promocionActual = null;
    this.alumnos = [];

    if (this.idProductoSeleccionado > 0) {
      this.matriculaService.obtenerPromociones(this.idProductoSeleccionado)
        .subscribe({
          next: (data) => this.promociones = data,
          error: (err) => console.error('Error al cargar promociones', err)
        });
    }
  }

  onPromocionChange(): void {
    this.promocionActual = this.promociones.find(p => p.id === Number(this.idPromocionSeleccionada)) || null;

    // Al seleccionar promoción, equivale al rbnNuevos_Click() del VB6: Forza filtro "Nuevos" y busca
    if (this.idPromocionSeleccionada > 0) {
      this.estadoFiltro = 0;
      this.pageNumber = 1;
      this.cargarMatriculas();
    } else {
      this.alumnos = [];
    }
  }

  // --- LÓGICA DE GRILLA ---
  cambiarFiltroEstado(estado: number): void {
    this.estadoFiltro = estado;
    this.pageNumber = 1;
    this.cargarMatriculas();
  }

  buscar(): void {
    this.pageNumber = 1;
    this.cargarMatriculas();
  }

  cargarMatriculas(): void {
    if (this.idPromocionSeleccionada === 0) return;

    this.matriculaService.obtenerPorPromocion(
      this.idPromocionSeleccionada,
      this.estadoFiltro,
      this.filtroGlobal,
      this.pageNumber,
      this.pageSize
    ).subscribe({
      next: (response) => {
        this.alumnos = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.mensajeError = '';
      },
      error: (err) => {
        this.mensajeError = 'Ocurrió un error al cargar las matrículas.';
        console.error(err);
      }
    });
  }

  cambiarPagina(page: number): void {
    if (page >= 1 && (this.totalPages === 0 || page <= this.totalPages)) {
      this.pageNumber = page;
      this.cargarMatriculas();
    }
  }

  toggleMatricula(alumno: AlumnoMatriculaListadoDto): void {
    const nuevoEstado = !alumno.esMatricula;
    const accionText = nuevoEstado ? 'matricular' : 'deshacer la matrícula de';

    if (confirm(`¿Está seguro que desea ${accionText} a: ${alumno.nombreCompleto}?`)) {
      this.matriculaService.actualizarEstado(alumno.idAlumno, alumno.idRegistro, alumno.idMatricula, nuevoEstado)
        .subscribe({
          next: () => {
            alumno.esMatricula = nuevoEstado;
            if (this.estadoFiltro === 0 || this.estadoFiltro === 1) {
              this.cargarMatriculas();
            }
          },
          error: (err) => {
            this.mensajeError = 'Error al actualizar el estado de la matrícula.';
            console.error(err);
          }
        });
    }
  }

  mathMin(a: number, b: number): number {
    return Math.min(a, b);
  }
}
