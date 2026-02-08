import { Routes } from '@angular/router';
import { LandingComponent } from './pages/landing/landing';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  // TODO: Add auth routes when auth module is created
  // {
  //   path: 'auth',
  //   loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
  // },
  { path: '**', redirectTo: '' }
];
