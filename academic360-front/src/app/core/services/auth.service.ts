import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable, tap } from 'rxjs';

export interface LoginRequest {
  usuario: string;
  contrasena: string;
}

export interface LoginResponse {
  token: string;
  idUsuario: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/auth/login`;

  // Signal para mantener el estado de sesión reactivo en toda la app
  isAuthenticated = signal<boolean>(this.hasToken());

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.apiUrl, credentials).pipe(
      tap(response => {
        this.setToken(response.token);
        this.isAuthenticated.set(true);
      })
    );
  }

  logout(): void {
    // CORRECCIÓN OWASP: Limpiamos sessionStorage al cerrar sesión.
    sessionStorage.removeItem('jwt_token');
    this.isAuthenticated.set(false);
  }

  private setToken(token: string): void {
    // CORRECCIÓN OWASP A03 / A07: Migración a sessionStorage.
    // Reduce la persistencia del token. Si el usuario cierra la pestaña, la sesión muere.
    // Nota de Seguridad: Para aplicaciones de alta criticidad, planificar migración a Cookies HttpOnly.
    sessionStorage.setItem('jwt_token', token);
  }

  getToken(): string | null {
    return sessionStorage.getItem('jwt_token');
  }

  private hasToken(): boolean {
    return !!sessionStorage.getItem('jwt_token');
  }
}
