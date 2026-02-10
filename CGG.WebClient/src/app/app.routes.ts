import { Routes } from '@angular/router';
import { LandingComponent } from './pages/landing/landing';
import { AdminComponent } from './pages/admin/admin';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'admin', component: AdminComponent },
  // TODO: Add auth routes when auth module is created
  // {
  //   path: 'auth',
  //   loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
  // },
  { path: '**', redirectTo: '' }
];
