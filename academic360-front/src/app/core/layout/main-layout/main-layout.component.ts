import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="flex h-screen bg-slate-50 text-slate-800 font-sans overflow-hidden">

      <!-- Overlay oscuro para móvil -->
      <div *ngIf="isSidebarOpen"
           (click)="toggleSidebar()"
           class="fixed inset-0 bg-slate-900 bg-opacity-50 z-20 md:hidden transition-opacity">
      </div>

      <!-- Menú Lateral (Sidebar) - Elegancia Corporativa -->
      <aside class="w-64 bg-slate-900 flex flex-col shadow-xl absolute inset-y-0 left-0 transform transition-transform duration-300 ease-in-out z-30 md:relative md:translate-x-0"
             [ngClass]="{'translate-x-0': isSidebarOpen, '-translate-x-full': !isSidebarOpen}">
        <div class="h-16 flex items-center justify-center border-b border-slate-800">
          <h1 class="text-xl font-bold text-white tracking-wider">Academic.<span class="text-indigo-500">360</span></h1>
        </div>

        <nav class="flex-1 overflow-y-auto py-4">
          <ul class="space-y-1">
            <li>
              <a routerLink="/dashboard" (click)="cerrarSidebarEnMovil()" routerLinkActive="bg-slate-800 border-l-4 border-indigo-500 text-white" class="flex items-center px-6 py-3 text-slate-400 hover:bg-slate-800 hover:text-white transition-colors">
                <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"></path></svg>
                Inicio
              </a>
            </li>
            <li>
              <a routerLink="/alumnos" (click)="cerrarSidebarEnMovil()" routerLinkActive="bg-slate-800 border-l-4 border-indigo-500 text-white" class="flex items-center px-6 py-3 text-slate-400 hover:bg-slate-800 hover:text-white transition-colors">
                <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"></path></svg>
                Alumnos
              </a>
            </li>
            <li>
              <div class="px-6 py-3 mt-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">Gestión Académica</div>
            </li>
            <li>
              <a routerLink="/matriculas-carrera" (click)="cerrarSidebarEnMovil()" routerLinkActive="bg-slate-800 border-l-4 border-indigo-500 text-white" class="flex items-center px-6 py-3 text-slate-400 hover:bg-slate-800 hover:text-white transition-colors">
                <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>
                Matrículas Carrera
              </a>
            </li>
            <li>
              <a routerLink="/matriculas-libres" (click)="cerrarSidebarEnMovil()" routerLinkActive="bg-slate-800 border-l-4 border-indigo-500 text-white" class="flex items-center px-6 py-3 text-slate-400 hover:bg-slate-800 hover:text-white transition-colors">
                <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"></path></svg>
                Matrículas Cursos Libres
              </a>
            </li>
          </ul>
        </nav>

        <div class="p-4 border-t border-slate-800">
          <button (click)="cerrarSesion()" class="w-full flex items-center justify-center px-4 py-2 bg-rose-600 hover:bg-rose-700 text-white rounded-lg transition-colors shadow-sm">
            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path></svg>
            Cerrar Sesión
          </button>
        </div>
      </aside>

      <!-- Área de Contenido Principal -->
      <main class="flex-1 flex flex-col min-w-0 overflow-hidden bg-slate-50">
        <!-- Barra superior blanca y limpia -->
        <header class="h-16 bg-white border-b border-slate-200 shadow-sm flex items-center justify-between md:justify-end px-4 md:px-6 relative z-10">
          <!-- Botón Hamburguesa -->
          <button (click)="toggleSidebar()" class="md:hidden text-slate-500 hover:text-slate-800 focus:outline-none">
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"></path>
            </svg>
          </button>

          <div class="flex items-center cursor-pointer hover:bg-slate-50 p-2 rounded-lg transition-colors">
            <div class="w-9 h-9 rounded-full bg-indigo-100 flex items-center justify-center text-indigo-700 font-bold mr-3 border border-indigo-200">
              U
            </div>
            <span class="text-sm font-medium text-slate-700">Usuario Activo</span>
            <svg class="w-4 h-4 ml-2 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path></svg>
          </div>
        </header>

        <!-- Contenedor dinámico -->
        <div class="flex-1 overflow-x-hidden overflow-y-auto p-4 md:p-8">
          <router-outlet></router-outlet>
        </div>
      </main>
    </div>
  `
})
export class MainLayoutComponent {
  isSidebarOpen = false;

  constructor(private router: Router) {}

  toggleSidebar(): void {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  cerrarSidebarEnMovil(): void {
    if (this.isSidebarOpen) {
      this.isSidebarOpen = false;
    }
  }

  cerrarSesion(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
