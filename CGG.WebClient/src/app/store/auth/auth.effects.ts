import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, catchError, exhaustMap, tap } from 'rxjs/operators';
import { ApiService } from '../../core/services/api.service';
import { UserTokenContext } from './auth.state';
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
  activeContext: UserTokenContext;
  availableContexts: UserTokenContext[];
  canSwitchContext: boolean;
}

interface SwitchContextResponse {
  token: string;
  activeContext: UserTokenContext;
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
            localStorage.setItem('token', response.token);
            localStorage.setItem('currentUser', JSON.stringify(response.user));
            localStorage.setItem('activeContext', JSON.stringify(response.activeContext));
            localStorage.setItem('availableContexts', JSON.stringify(response.availableContexts));
            localStorage.setItem('canSwitchContext', String(response.canSwitchContext));

            return AuthActions.loginSuccess({
              user: response.user,
              token: response.token,
              activeContext: response.activeContext,
              availableContexts: response.availableContexts,
              canSwitchContext: response.canSwitchContext
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

  // Switch Context Effect
  switchContext$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.switchContext),
      exhaustMap((action) =>
        this.apiService.post<SwitchContextResponse>('auth/switch-context', {
          contextType: action.contextType,
          contextId: action.contextId
        }).pipe(
          map((response) => {
            localStorage.setItem('token', response.token);
            localStorage.setItem('activeContext', JSON.stringify(response.activeContext));

            return AuthActions.switchContextSuccess({
              token: response.token,
              activeContext: response.activeContext
            });
          }),
          catchError((error) =>
            of(AuthActions.switchContextFailure({
              error: error.error?.message || error.message || 'Context switch failed'
            }))
          )
        )
      )
    )
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
          localStorage.removeItem('activeContext');
          localStorage.removeItem('availableContexts');
          localStorage.removeItem('canSwitchContext');
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
        const activeContextJson = localStorage.getItem('activeContext');
        const availableContextsJson = localStorage.getItem('availableContexts');
        const canSwitchContext = localStorage.getItem('canSwitchContext') === 'true';

        if (token && userJson) {
          try {
            const user = JSON.parse(userJson);
            const activeContext = activeContextJson ? JSON.parse(activeContextJson) : null;
            const availableContexts = availableContextsJson ? JSON.parse(availableContextsJson) : [];

            return AuthActions.loadUserFromStorageSuccess({
              user,
              token,
              activeContext,
              availableContexts,
              canSwitchContext
            });
          } catch (e) {
            console.error('Failed to parse user from storage', e);
            localStorage.removeItem('token');
            localStorage.removeItem('currentUser');
            localStorage.removeItem('activeContext');
            localStorage.removeItem('availableContexts');
            localStorage.removeItem('canSwitchContext');
          }
        }

        return { type: '[Auth] No User in Storage' };
      })
    )
  );
}
