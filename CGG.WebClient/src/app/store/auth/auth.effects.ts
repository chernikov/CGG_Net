import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, catchError, exhaustMap, tap } from 'rxjs/operators';
import { ApiService } from '../../core/services/api.service';
import { UserRole, UserTokenContext } from './auth.state';
import * as AuthActions from './auth.actions';
import { TranslateService } from '@ngx-translate/core';
import { toTranslationKey } from '../../core/utils/error-translations';

interface LoginResponse {
  token: string;
  user: {
    id: string;
    email: string;
    firstName: string | null;
    surname: string | null;
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
  token: string;
  user: {
    id: string;
    email: string;
    firstName: string | null;
    surname: string | null;
    role: string;
    credits: number;
  };
  activeContext: UserTokenContext;
  availableContexts: UserTokenContext[];
  canSwitchContext: boolean;
}

@Injectable()
export class AuthEffects {
  private actions$ = inject(Actions);
  private apiService = inject(ApiService);
  private router = inject(Router);
  private translate = inject(TranslateService);

  private t(backendMessage: string | undefined): string {
    return this.translate.instant(toTranslationKey(backendMessage));
  }

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
              error: this.t(error.error?.message || error.message)
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
        tap((action) => {
          const role = Number(action.user.role);
          const isAdmin = isNaN(role)
            ? action.user.role === 'Admin'
            : role === UserRole.Admin;
          this.router.navigate([isAdmin ? '/admin' : '/dashboard']);
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
              error: this.t(error.error?.message || error.message)
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
          firstName: action.firstName,
          surname: action.surname,
          role: action.role
        }).pipe(
          map((response) => {
            localStorage.setItem('token', response.token);
            localStorage.setItem('currentUser', JSON.stringify(response.user));
            localStorage.setItem('activeContext', JSON.stringify(response.activeContext));
            localStorage.setItem('availableContexts', JSON.stringify(response.availableContexts));
            localStorage.setItem('canSwitchContext', JSON.stringify(response.canSwitchContext));
            return AuthActions.registerSuccess({
              token: response.token,
              user: {
                id: response.user.id,
                email: response.user.email,
                firstName: response.user.firstName,
                surname: response.user.surname,
                role: response.user.role,
                credits: response.user.credits
              },
              activeContext: response.activeContext,
              availableContexts: response.availableContexts,
              canSwitchContext: response.canSwitchContext
            });
          }),
          catchError((error) =>
            of(AuthActions.registerFailure({
              error: this.t(error.error?.message || error.message)
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
          this.router.navigate(['/dashboard']);
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
