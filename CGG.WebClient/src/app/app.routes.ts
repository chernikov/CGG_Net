import { Routes } from '@angular/router';
import { LandingComponent } from './pages/landing/landing';
import { AdminComponent } from './pages/admin/admin';
import { ParentChoiceComponent } from './pages/parent-choice/parent-choice';
import { SchoolChoiceComponent } from './pages/school-choice/school-choice';
import { StudentChoiceComponent } from './pages/student-choice/student-choice';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'parent-choice', component: ParentChoiceComponent },
  { path: 'school-choice', component: SchoolChoiceComponent },
  { path: 'student-choice', component: StudentChoiceComponent },
  { path: 'admin', component: AdminComponent },
  // TODO: Add auth routes when auth module is created
  // {
  //   path: 'auth',
  //   loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
  // },
  { path: '**', redirectTo: '' }
];
