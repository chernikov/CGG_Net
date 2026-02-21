import { createAction, props } from '@ngrx/store';
import { User, UserTokenContext } from './auth.state';

// Login Actions
export const login = createAction(
  '[Auth] Login',
  props<{ email: string; password: string }>()
);

export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ user: User; token: string; activeContext: UserTokenContext; availableContexts: UserTokenContext[]; canSwitchContext: boolean }>()
);

export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: string }>()
);

// Register Actions
export const register = createAction(
  '[Auth] Register',
  props<{ email: string; password: string; displayName: string; role: number }>()
);

export const registerSuccess = createAction(
  '[Auth] Register Success',
  props<{ message: string }>()
);

export const registerFailure = createAction(
  '[Auth] Register Failure',
  props<{ error: string }>()
);

// Switch Context Actions
export const switchContext = createAction(
  '[Auth] Switch Context',
  props<{ contextType: string; contextId?: string }>()
);

export const switchContextSuccess = createAction(
  '[Auth] Switch Context Success',
  props<{ token: string; activeContext: UserTokenContext }>()
);

export const switchContextFailure = createAction(
  '[Auth] Switch Context Failure',
  props<{ error: string }>()
);

// Logout Action
export const logout = createAction('[Auth] Logout');

// Load User from Storage
export const loadUserFromStorage = createAction('[Auth] Load User From Storage');

export const loadUserFromStorageSuccess = createAction(
  '[Auth] Load User From Storage Success',
  props<{ user: User; token: string; activeContext: UserTokenContext | null; availableContexts: UserTokenContext[]; canSwitchContext: boolean }>()
);

// Clear Error
export const clearError = createAction('[Auth] Clear Error');
