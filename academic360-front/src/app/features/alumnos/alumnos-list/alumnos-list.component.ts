import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AlumnoService, AlumnoListadoDto } from '../services/alumno.service';

@Component({
  selector: 'app-alumnos-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './alumnos-list.component.html',
})
export class AlumnosListComponent implements OnInit {
  alumnos: AlumnoListadoDto[] = [];
  filtroGlobal: string = '';
  mensajeError: string = '';

  pageNumber: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  totalPages: number = 0;

  constructor(private alumnoService: AlumnoService) {}

  ngOnInit(): void {
    this.cargarTodos();
  }

  cargarTodos(): void {
    this.filtroGlobal = '';
    this.pageNumber = 1;
    this.ejecutarBusqueda();
  }

  buscar(): void {
    this.pageNumber = 1;
    this.ejecutarBusqueda();
  }

  cambiarPagina(nuevaPagina: number): void {
    if (nuevaPagina >= 1 && nuevaPagina <= this.totalPages) {
      this.pageNumber = nuevaPagina;
      this.ejecutarBusqueda();
    }
  }

  private ejecutarBusqueda(): void {
    this.mensajeError = '';
    this.alumnoService.listarAlumnos(this.filtroGlobal, this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.alumnos = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.pageNumber = response.pageNumber;
      },
      error: (err) => {
        console.error('Error al listar alumnos', err);
        this.mensajeError = 'Ocurrió un error al cargar el listado de alumnos.';
      }
    });
  }

  eliminar(alumno: AlumnoListadoDto): void {
    if (confirm(`¿Está seguro que desea eliminar al alumno: ${alumno.nombreCompleto}?`)) {
      this.mensajeError = '';
      this.alumnoService.eliminarAlumno(alumno.idActor).subscribe({
        next: () => {
          this.ejecutarBusqueda();
        },
        error: (err) => {
          console.error('Error al eliminar', err);
          if (err.error && err.error.detail) {
            this.mensajeError = err.error.detail;
          } else {
            this.mensajeError = 'No se pudo eliminar el alumno. Es probable que tenga registros dependientes.';
          }
        }
      });
    }
  }

  mathMin(a: number, b: number): number {
    return Math.min(a, b);
  }
}
