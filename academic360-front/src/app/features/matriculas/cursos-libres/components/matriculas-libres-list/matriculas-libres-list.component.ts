import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatriculaService, MatriculaCursosLibresListadoDto } from '../../../services/matricula.service';

@Component({
  selector: 'app-matriculas-libres-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './matriculas-libres-list.component.html'
})
export class MatriculasLibresListComponent implements OnInit {
  private matriculaService = inject(MatriculaService);

  filtroGlobal: string = '';
  matriculas: MatriculaCursosLibresListadoDto[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;
  cargando: boolean = false;

  ngOnInit(): void {
    this.cargarMatriculas();
  }

  cargarMatriculas(): void {
    this.cargando = true;
    this.matriculaService.listarMatriculasLibres(this.filtroGlobal).subscribe({
      next: (data) => {
        this.matriculas = data;
        this.totalCount = data.length;
        this.cargando = false;
      },
      error: (err) => {
        console.error('Error al cargar el listado de matrículas libres:', err);
        this.cargando = false;
      }
    });
  }

  buscar(): void {
    this.cargarMatriculas();
  }

  cargarTodos(): void {
    this.filtroGlobal = '';
    this.cargarMatriculas();
  }

  cambiarPagina(nuevaPagina: number): void {
    this.pageNumber = nuevaPagina;
    // Lógica de paginación adicional si se implementa en backend
  }

  mathMin(a: number, b: number): number {
    return Math.min(a, b);
  }
}
