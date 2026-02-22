import { createReducer, on } from '@ngrx/store';
import { AuthState, initialAuthState } from './auth.state';
import * as AuthActions from './auth.actions';

export const authReducer = createReducer(
  initialAuthState,

  // Login
  on(AuthActions.login, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(AuthActions.loginSuccess, (state, { user, token, activeContext, availableContexts, canSwitchContext }) => ({
    ...state,
    user,
    token,
    isAuthenticated: true,
    loading: false,
    error: null,
    activeContext,
    availableContexts,
    canSwitchContext
  })),

  on(AuthActions.loginFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Register
  on(AuthActions.register, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(AuthActions.registerSuccess, (state, { user, token, activeContext, availableContexts, canSwitchContext }) => ({
    ...state,
    loading: false,
    error: null,
    user,
    token,
    isAuthenticated: true,
    activeContext,
    availableContexts,
    canSwitchContext
  })),

  on(AuthActions.registerFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Switch Context
  on(AuthActions.switchContext, (state) => ({
    ...state,
    switchContextLoading: true,
    error: null
  })),

  on(AuthActions.switchContextSuccess, (state, { token, activeContext }) => ({
    ...state,
    token,
    activeContext,
    switchContextLoading: false
  })),

  on(AuthActions.switchContextFailure, (state, { error }) => ({
    ...state,
    switchContextLoading: false,
    error
  })),

  // Logout
  on(AuthActions.logout, () => ({
    ...initialAuthState
  })),

  // Load from Storage
  on(AuthActions.loadUserFromStorageSuccess, (state, { user, token, activeContext, availableContexts, canSwitchContext }) => ({
    ...state,
    user,
    token,
    isAuthenticated: true,
    activeContext,
    availableContexts,
    canSwitchContext
  })),

  // Clear Error
  on(AuthActions.clearError, (state) => ({
    ...state,
    error: null
  }))
);
