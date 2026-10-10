import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="flex h-screen bg-gray-900 text-gray-100 font-sans">
      <!-- Menú Lateral (Sidebar) -->
      <aside class="w-64 bg-gray-800 flex flex-col shadow-lg">
        <div class="h-16 flex items-center justify-center border-b border-gray-700">
          <h1 class="text-xl font-bold text-white tracking-wider">Academic<span class="text-blue-500">360</span></h1>
        </div>

        <nav class="flex-1 overflow-y-auto py-4">
          <ul class="space-y-1">
            <!-- Dashboard -->
            <li>
              <a routerLink="/dashboard" routerLinkActive="bg-gray-700 border-l-4 border-blue-500 text-white" class="flex items-center px-6 py-3 text-gray-400 hover:bg-gray-700 hover:text-white transition-colors">
                <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"></path></svg>
                Inicio
              </a>
            </li>
            <!-- Alumnos -->
            <li>
              <a routerLink="/alumnos" routerLinkActive="bg-gray-700 border-l-4 border-blue-500 text-white" class="flex items-center px-6 py-3 text-gray-400 hover:bg-gray-700 hover:text-white transition-colors">
                <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"></path></svg>
                Alumnos
              </a>
            </li>
            <!-- Matrículas -->
            <li>
              <div class="px-6 py-3 mt-4 text-xs font-semibold text-gray-500 uppercase tracking-wider">Gestión Académica</div>
            </li>
            <li>
              <a routerLink="/matriculas-carrera" routerLinkActive="bg-gray-700 border-l-4 border-blue-500 text-white" class="flex items-center px-6 py-3 text-gray-400 hover:bg-gray-700 hover:text-white transition-colors">
                <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>
                Matrículas Carrera
              </a>
            </li>
            <li>
              <a routerLink="/matriculas-libres" routerLinkActive="bg-gray-700 border-l-4 border-blue-500 text-white" class="flex items-center px-6 py-3 text-gray-400 hover:bg-gray-700 hover:text-white transition-colors">
                <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"></path></svg>
                Matrículas Cursos Libres
              </a>
            </li>
          </ul>
        </nav>

        <div class="p-4 border-t border-gray-700">
          <button (click)="cerrarSesion()" class="w-full flex items-center justify-center px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded-md transition-colors">
            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path></svg>
            Cerrar Sesión
          </button>
        </div>
      </aside>

      <!-- Área de Contenido Principal -->
      <main class="flex-1 flex flex-col overflow-hidden">
        <!-- Barra superior -->
        <header class="h-16 bg-gray-800 shadow flex items-center justify-end px-6">
          <div class="flex items-center">
            <div class="w-8 h-8 rounded-full bg-blue-500 flex items-center justify-center text-white font-bold mr-2">
              U
            </div>
            <span class="text-sm font-medium text-gray-300">Usuario Activo</span>
          </div>
        </header>

        <!-- Contenedor dinámico (Aquí se cargan las páginas internas) -->
        <div class="flex-1 overflow-x-hidden overflow-y-auto bg-gray-900 p-6">
          <router-outlet></router-outlet>
        </div>
      </main>
    </div>
  `
})
export class MainLayoutComponent {
  constructor(private router: Router) {}

  cerrarSesion(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
