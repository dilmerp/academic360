import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-matriculas-libres-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './matriculas-libres-list.component.html'
})
export class MatriculasLibresListComponent implements OnInit {
  filtroGlobal: string = '';
  matriculas: any[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;

  ngOnInit(): void {
  }

  buscar(): void {
    // Lógica de búsqueda futura
  }

  cargarTodos(): void {
    this.filtroGlobal = '';
  }

  cambiarPagina(nuevaPagina: number): void {
    // Lógica de paginación futura
  }

  mathMin(a: number, b: number): number {
    return Math.min(a, b);
  }
}
