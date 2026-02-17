import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, catchError, exhaustMap, tap } from 'rxjs/operators';
import { ApiService } from '../../core/services/api.service';
import * as AuthActions from './auth.actions';

interface LoginResponse {
  token: string;
  user: {
    id: string;
    email: string;
    displayName: string;
    role: string;
    credits: number;
  };
}

interface RegisterResponse {
  message: string;
  userId: string;
}

@Injectable()
export class AuthEffects {
  private actions$ = inject(Actions);
  private apiService = inject(ApiService);
  private router = inject(Router);

  // Login Effect
  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      exhaustMap((action) =>
        this.apiService.post<LoginResponse>('auth/login', {
          email: action.email,
          password: action.password
        }).pipe(
          map((response) => {
            // Save to localStorage
            localStorage.setItem('token', response.token);
            localStorage.setItem('currentUser', JSON.stringify(response.user));
            
            return AuthActions.loginSuccess({
              user: response.user,
              token: response.token
            });
          }),
          catchError((error) =>
            of(AuthActions.loginFailure({
              error: error.error?.message || error.message || 'Login failed'
            }))
          )
        )
      )
    )
  );

  // Login Success Navigation
  loginSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess),
        tap(() => {
          this.router.navigate(['/admin']);
        })
      ),
    { dispatch: false }
  );

  // Register Effect
  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.register),
      exhaustMap((action) =>
        this.apiService.post<RegisterResponse>('auth/register', {
          email: action.email,
          password: action.password,
          displayName: action.displayName,
          role: action.role
        }).pipe(
          map((response) =>
            AuthActions.registerSuccess({ message: response.message })
          ),
          catchError((error) =>
            of(AuthActions.registerFailure({
              error: error.error?.message || error.message || 'Registration failed'
            }))
          )
        )
      )
    )
  );

  // Register Success Navigation
  registerSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.registerSuccess),
        tap(() => {
          this.router.navigate(['/login']);
        })
      ),
    { dispatch: false }
  );

  // Logout Effect
  logout$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.logout),
        tap(() => {
          localStorage.removeItem('token');
          localStorage.removeItem('currentUser');
          this.router.navigate(['/login']);
        })
      ),
    { dispatch: false }
  );

  // Load User from Storage
  loadUserFromStorage$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.loadUserFromStorage),
      map(() => {
        const token = localStorage.getItem('token');
        const userJson = localStorage.getItem('currentUser');

        if (token && userJson) {
          try {
            const user = JSON.parse(userJson);
            return AuthActions.loadUserFromStorageSuccess({ user, token });
          } catch (e) {
            console.error('Failed to parse user from storage', e);
            // Clear invalid data but don't redirect
            localStorage.removeItem('token');
            localStorage.removeItem('currentUser');
          }
        }

        // User not logged in - no action needed, no redirect
        return { type: '[Auth] No User in Storage' };
      })
    )
  );
}
