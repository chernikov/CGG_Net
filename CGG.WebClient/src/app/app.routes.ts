import { Routes } from '@angular/router';
import { LandingComponent } from './pages/landing/landing';
import { AdminComponent } from './pages/admin/admin';
import { ParentChoiceComponent } from './pages/parent-choice/parent-choice';
import { SchoolChoiceComponent } from './pages/school-choice/school-choice';
import { StudentChoiceComponent } from './pages/student-choice/student-choice';
import { ParentRegisterComponent } from './pages/parent-register/parent-register';
import { SchoolRegisterComponent } from './pages/school-register/school-register';
import { StudentRegisterComponent } from './pages/student-register/student-register';
import { LoginComponent } from './pages/login/login';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'landing', component: LandingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'parent-choice', component: ParentChoiceComponent },
  { path: 'school-choice', component: SchoolChoiceComponent },
  { path: 'student-choice', component: StudentChoiceComponent },
  { path: 'parent-register', component: ParentRegisterComponent },
  { path: 'school-register', component: SchoolRegisterComponent },
  { path: 'student-register', component: StudentRegisterComponent },
  { path: 'admin', component: AdminComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
