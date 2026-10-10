import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout.component';
import { AlumnoFormComponent } from './features/alumnos/components/alumno-form/alumno-form.component';
import { LoginComponent } from './features/auth/components/login/login.component';
import { AlumnosListComponent } from './features/alumnos/alumnos-list/alumnos-list.component';
import { MatriculaListComponent } from './features/matriculas/matricula-list/matricula-list.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: 'dashboard', redirectTo: 'alumnos', pathMatch: 'full' },
      { path: 'alumnos', component: AlumnosListComponent },
      { path: 'alumnos/nuevo', component: AlumnoFormComponent },

      // AQUI: Cambiamos 'matriculas' por 'matriculas-carrera'
      { path: 'matriculas-carrera', component: MatriculaListComponent }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
