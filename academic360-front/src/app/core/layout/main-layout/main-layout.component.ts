import { Component, OnInit } from '@angular/core';
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

      <!-- Menú Lateral (Sidebar) - Diseño Compacto (w-56 en lugar de w-64) -->
      <aside class="w-56 bg-slate-900 flex flex-col shadow-xl absolute inset-y-0 left-0 transform transition-transform duration-300 ease-in-out z-30 md:relative md:translate-x-0"
             [ngClass]="{'translate-x-0': isSidebarOpen, '-translate-x-full': !isSidebarOpen}">
        <!-- Altura de cabecera reducida a h-14 -->
        <div class="h-14 flex items-center justify-center border-b border-slate-800">
          <h1 class="text-lg font-bold text-white tracking-wider">Academic.<span class="text-indigo-500">360</span></h1>
        </div>

        <nav class="flex-1 overflow-y-auto py-3">
          <ul class="space-y-0.5">
            <li>
              <a routerLink="/dashboard" (click)="cerrarSidebarEnMovil()" routerLinkActive="bg-slate-800 border-l-4 border-indigo-500 text-white" [routerLinkActiveOptions]="{exact: true}" class="flex items-center px-4 py-2 text-sm text-slate-400 hover:bg-slate-800 hover:text-white transition-colors">
                <svg class="w-4 h-4 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"></path></svg>
                Inicio
              </a>
            </li>
            <li>
              <a routerLink="/alumnos" (click)="cerrarSidebarEnMovil()" routerLinkActive="bg-slate-800 border-l-4 border-indigo-500 text-white" class="flex items-center px-4 py-2 text-sm text-slate-400 hover:bg-slate-800 hover:text-white transition-colors">
                <svg class="w-4 h-4 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"></path></svg>
                Alumnos
              </a>
            </li>

            <li>
              <div class="px-4 py-2 mt-2 text-[11px] font-semibold text-slate-500 uppercase tracking-wider">Gestión Académica</div>
            </li>

            <!-- Menú Desplegable (Acordeón) para Matrículas -->
            <li>
              <button (click)="toggleMatriculas()"
                      class="w-full flex items-center justify-between px-4 py-2 text-sm text-slate-400 hover:bg-slate-800 hover:text-white transition-colors focus:outline-none"
                      [ngClass]="{'bg-slate-800 text-white': isMatriculasOpen || router.url.includes('/matriculas')}">
                <div class="flex items-center">
                  <!-- Ícono de Carpeta/Documentos -->
                  <svg class="w-4 h-4 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 002-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"></path></svg>
                  Matrículas
                </div>
                <!-- Chevron (Flecha) Animada -->
                <svg class="w-4 h-4 transform transition-transform duration-200"
                     [ngClass]="{'rotate-180': isMatriculasOpen}"
                     fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path>
                </svg>
              </button>

              <!-- Submenú -->
              <ul class="bg-slate-900/50 py-1 space-y-0.5 overflow-hidden transition-all duration-300"
                  [ngClass]="{'hidden': !isMatriculasOpen, 'block': isMatriculasOpen}">
                <li>
                  <a routerLink="/matriculas-carrera" (click)="cerrarSidebarEnMovil()" routerLinkActive="text-white font-medium" class="flex items-center pl-11 pr-4 py-2 text-sm text-slate-400 hover:text-white transition-colors">
                    <span class="w-1.5 h-1.5 rounded-full mr-2" [ngClass]="{'bg-indigo-500': router.url.includes('/matriculas-carrera'), 'bg-slate-600': !router.url.includes('/matriculas-carrera')}"></span>
                    Carrera
                  </a>
                </li>
                <li>
                  <a routerLink="/matriculas-libres" (click)="cerrarSidebarEnMovil()" routerLinkActive="text-white font-medium" class="flex items-center pl-11 pr-4 py-2 text-sm text-slate-400 hover:text-white transition-colors">
                    <span class="w-1.5 h-1.5 rounded-full mr-2" [ngClass]="{'bg-indigo-500': router.url.includes('/matriculas-libres'), 'bg-slate-600': !router.url.includes('/matriculas-libres')}"></span>
                    Cursos Libres
                  </a>
                </li>
              </ul>
            </li>

          </ul>
        </nav>

        <div class="p-3 border-t border-slate-800">
          <!-- Botón de logout más compacto -->
          <button (click)="cerrarSesion()" class="w-full flex items-center justify-center px-3 py-1.5 text-sm bg-rose-600 hover:bg-rose-700 text-white rounded-md transition-colors shadow-sm font-medium">
            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path></svg>
            Cerrar Sesión
          </button>
        </div>
      </aside>

      <!-- Área de Contenido Principal -->
      <main class="flex-1 flex flex-col min-w-0 overflow-hidden bg-slate-50">
        <!-- Barra superior blanca reducida a h-14 -->
        <header class="h-14 bg-white border-b border-slate-200 shadow-sm flex items-center justify-between md:justify-end px-4 md:px-5 relative z-10">
          <!-- Botón Hamburguesa -->
          <button (click)="toggleSidebar()" class="md:hidden text-slate-500 hover:text-slate-800 focus:outline-none">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"></path>
            </svg>
          </button>

          <!-- Perfil de usuario ajustado -->
          <div class="flex items-center cursor-pointer hover:bg-slate-50 p-1.5 rounded-lg transition-colors">
            <div class="w-8 h-8 rounded-full bg-indigo-100 flex items-center justify-center text-indigo-700 font-bold mr-2.5 border border-indigo-200 text-sm">
              U
            </div>
            <span class="text-[13px] font-medium text-slate-700">Usuario Activo</span>
            <svg class="w-3.5 h-3.5 ml-2 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path></svg>
          </div>
        </header>

        <!-- Contenedor dinámico con paddings reducidos -->
        <div class="flex-1 overflow-x-hidden overflow-y-auto p-4 md:p-5">
          <router-outlet></router-outlet>
        </div>
      </main>
    </div>
  `
})
export class MainLayoutComponent implements OnInit {
  isSidebarOpen = false;
  isMatriculasOpen = false;

  constructor(public router: Router) {}

  ngOnInit(): void {
    // Si al cargar la app ya estamos en alguna ruta de matrículas, mantenemos el acordeón abierto
    if (this.router.url.includes('/matriculas')) {
      this.isMatriculasOpen = true;
    }
  }

  toggleSidebar(): void {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  toggleMatriculas(): void {
    this.isMatriculasOpen = !this.isMatriculasOpen;
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
