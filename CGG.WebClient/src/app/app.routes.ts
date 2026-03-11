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
import { DashboardComponent } from './pages/dashboard/dashboard';
import { AddChild } from './pages/add-child/add-child';
import { EditChild } from './pages/child/edit-child/edit-child';
import { ProfileComponent } from './pages/profile/profile.component';
import { TransactionsComponent } from './pages/transactions/transactions';
import { authGuard } from './core/guards/auth-guard';
import { homeGuard } from './core/guards/home-guard';

export const routes: Routes = [
  { path: '', component: LandingComponent, canActivate: [homeGuard] },
  { path: 'landing', component: LandingComponent, canActivate: [homeGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'parent-choice', component: ParentChoiceComponent },
  { path: 'school-choice', component: SchoolChoiceComponent },
  { path: 'student-choice', component: StudentChoiceComponent },
  { path: 'parent-register', component: ParentRegisterComponent },
  { path: 'school-register', component: SchoolRegisterComponent },
  { path: 'student-register', component: StudentRegisterComponent },
  { path: 'admin', component: AdminComponent, canActivate: [authGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'add-child', component: AddChild, canActivate: [authGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [authGuard] },
  { path: 'child/edit/:id', component: EditChild, canActivate: [authGuard] },
  { path: 'child/:id', component: ProfileComponent, canActivate: [authGuard] },
  { path: 'transactions', component: TransactionsComponent, canActivate: [authGuard] },
  { path: 'buy-credits', loadComponent: () => import('./pages/buy-credits/buy-credits').then(m => m.BuyCreditsComponent), canActivate: [authGuard] },
  { path: 'payment/result', loadComponent: () => import('./pages/payment-result/payment-result').then(m => m.PaymentResultComponent), canActivate: [authGuard] },
  {
    path: 'survey',
    loadComponent: () =>
      import('./features/survey/pages/survey-page/survey-page.component').then(
        m => m.SurveyPageComponent
      ),
  },
  { path: '**', redirectTo: '' }
];
