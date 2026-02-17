import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as AuthActions from '../../store/auth/auth.actions';
import { selectAuthError, selectAuthLoading } from '../../store/auth/auth.selectors';
import { UserRole } from '../../store/auth/auth.state';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-parent-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './parent-register.html',
  styleUrl: './parent-register.scss'
})
export class ParentRegisterComponent {
  formData = {
    email: '',
    password: '',
    confirmPassword: '',
    firstName: '',
    lastName: ''
  };

  loading$: Observable<boolean>;
  error$: Observable<string | null>;
  validationError: string | null = null;

  constructor(
    private router: Router,
    private store: Store
  ) {
    this.loading$ = this.store.select(selectAuthLoading);
    this.error$ = this.store.select(selectAuthError);
  }

  onSubmit() {
    // Валідація
    this.validationError = null;

    if (!this.formData.email || !this.formData.password || !this.formData.firstName || !this.formData.lastName) {
      this.validationError = 'Будь ласка, заповніть всі поля';
      return;
    }

    if (this.formData.password !== this.formData.confirmPassword) {
      this.validationError = 'Паролі не співпадають';
      return;
    }

    if (this.formData.password.length < 8) {
      this.validationError = 'Пароль має містити мінімум 8 символів';
      return;
    }

    // Відправка на backend через NgRx
    const displayName = `${this.formData.firstName} ${this.formData.lastName}`;
    
    console.log('Parent registration:', {
      email: this.formData.email,
      displayName,
      role: UserRole.UserParent
    });

    this.store.dispatch(AuthActions.register({
      email: this.formData.email,
      password: this.formData.password,
      displayName: displayName,
      role: UserRole.UserParent
    }));
  }

  goBack() {
    this.router.navigate(['/parent-choice']);
  }
}
