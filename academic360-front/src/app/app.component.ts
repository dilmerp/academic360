//import { AlumnoFormComponent } from './features/alumnos/components/alumno-form/alumno-form.component';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'] // o .scss dependiendo de lo que tengas
})
export class AppComponent {
  title = 'academic360-front';
}


