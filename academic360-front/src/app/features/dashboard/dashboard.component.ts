import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css' // o .scss dependiendo de tu configuración
})
export class DashboardComponent {
  // No se requiere ninguna lógica en el controlador por ahora
}
