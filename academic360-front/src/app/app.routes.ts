import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout.component';
import { AlumnoFormComponent } from './features/alumnos/components/alumno-form/alumno-form.component';
import { LoginComponent } from './features/auth/components/login/login.component';
import { AlumnosListComponent } from './features/alumnos/alumnos-list/alumnos-list.component';
import { MatriculaListComponent } from './features/matriculas/matricula-list/matricula-list.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { MatriculaFormComponent } from './features/matriculas/components/matricula-form/matricula-form.component';
import { MatriculasLibresListComponent } from './features/matriculas/components/matriculas-libres-list/matriculas-libres-list.component';


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
      { path: 'dashboard', component: DashboardComponent },
      { path: 'alumnos', component: AlumnosListComponent },
      { path: 'alumnos/nuevo', component: AlumnoFormComponent },
      { path: 'matriculas-carrera', component: MatriculaListComponent },
      { path: 'matriculas-libres/nuevo', component: MatriculaFormComponent },
      { path: 'matriculas-libres', component: MatriculasLibresListComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }

    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
