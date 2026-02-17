import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { Subject, takeUntil, Observable } from 'rxjs';
import * as AuthActions from '../../store/auth/auth.actions';
import * as AuthSelectors from '../../store/auth/auth.selectors';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent implements OnInit, OnDestroy {
  formData = {
    email: '',
    password: ''
  };

  loading$!: Observable<boolean>;
  error$!: Observable<string | null>;
  
  private destroy$ = new Subject<void>();

  constructor(
    private router: Router,
    private store: Store
  ) {
    this.loading$ = this.store.select(AuthSelectors.selectAuthLoading);
    this.error$ = this.store.select(AuthSelectors.selectAuthError);
  }

  ngOnInit() {
    // Clear any previous errors
    this.store.dispatch(AuthActions.clearError());
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSubmit() {
    if (this.formData.email && this.formData.password) {
      this.store.dispatch(AuthActions.login({
        email: this.formData.email,
        password: this.formData.password
      }));
    }
  }

  goBack() {
    this.router.navigate(['/landing']);
  }
}
