import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatriculaService, MatriculaCarreraListadoDto, SubTipoDto, ProductoDto } from '../../../services/matricula.service';

@Component({
  selector: 'app-matricula-carrera-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './matricula-carrera-list.component.html'
})
export class MatriculaCarreraListComponent implements OnInit {
  private matriculaService = inject(MatriculaService);

  alumnos: MatriculaCarreraListadoDto[] = [];

  subTipos: SubTipoDto[] = [];
  productos: ProductoDto[] = [];

  subTipoSeleccionado: SubTipoDto | null = null;
  idProductoSeleccionado: number = 0;

  estadoFiltro: number = 0;
  filtroGlobal: string = '';

  pageNumber: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  totalPages: number = 0;

  mensajeError: string = '';

  ngOnInit(): void {
    this.cargarSubTipos();
  }

  cargarSubTipos(): void {
    this.matriculaService.obtenerSubTipos().subscribe({
      next: (data) => {
        const permitidos = ['CIENCIAS EMPRESARIALES', 'INGENIERIAS', 'INGENIERIA'];
        this.subTipos = data.filter(st =>
          permitidos.includes(st.subTipoNombre.toUpperCase().trim())
        );
      },
      error: (err) => {
        this.mostrarError('Error al cargar los subtipos.');
        console.error(err);
      }
    });
  }

  onSubTipoChange(): void {
    this.productos = [];
    this.idProductoSeleccionado = 0;
    this.alumnos = [];
    this.totalCount = 0;

    if (this.subTipoSeleccionado) {
      this.matriculaService.obtenerProductos(this.subTipoSeleccionado.idTipo, this.subTipoSeleccionado.idSubTipo)
        .subscribe({
          next: (data) => {
            this.productos = data;
            if (this.productos.length > 0) {
              this.idProductoSeleccionado = this.productos[0].id;
              this.onProductoChange();
            }
          },
          error: (err) => {
             this.mostrarError('Error al cargar productos');
             console.error(err);
          }
        });
    }
  }

  onProductoChange(): void {
    if (this.idProductoSeleccionado > 0) {
      this.pageNumber = 1;
      this.cargarMatriculas();
    } else {
      this.alumnos = [];
      this.totalCount = 0;
    }
  }

  buscar(): void {
    if (this.idProductoSeleccionado > 0) {
      this.pageNumber = 1;
      this.cargarMatriculas();
    }
  }

  cargarMatriculas(): void {
    if (this.idProductoSeleccionado === 0) return;

    this.mensajeError = '';
    this.matriculaService.obtenerPorProducto(
      this.idProductoSeleccionado,
      this.estadoFiltro,
      this.filtroGlobal,
      this.pageNumber,
      this.pageSize
    ).subscribe({
      next: (response) => {
        this.alumnos = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
      },
      error: (err) => {
        this.mostrarError('Ocurrió un error al cargar las matrículas.');
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

  toggleMatricula(alumno: MatriculaCarreraListadoDto): void {
    const nuevoEstado = !alumno.esMatricula;
    const accionText = nuevoEstado ? 'matricular' : 'deshacer la matrícula de';

    if (confirm(`¿Está seguro que desea ${accionText} a: ${alumno.nombreCompleto}?`)) {
      this.matriculaService.actualizarEstado(alumno.idAlumno, alumno.idRegistro, alumno.idMatricula, nuevoEstado)
        .subscribe({
          next: () => {
            alumno.esMatricula = nuevoEstado;
            this.cargarMatriculas();
          },
          error: (err) => {
            this.mostrarError('Error al actualizar el estado de la matrícula.');
            console.error(err);
          }
        });
    }
  }

  mathMin(a: number, b: number): number {
    return Math.min(a, b);
  }

  private mostrarError(mensaje: string): void {
    this.mensajeError = mensaje;
    setTimeout(() => {
      this.mensajeError = '';
    }, 5000);
  }
}
