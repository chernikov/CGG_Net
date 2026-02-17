export enum UserRole {
  UserChild = 0,
  UserParent = 1,
  Teacher = 2,
  Admin = 3
}

export interface User {
  id: string;
  email: string;
  displayName: string;
  role: string;
  credits: number;
}

export interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
}

export const initialAuthState: AuthState = {
  user: null,
  token: null,
  isAuthenticated: false,
  loading: false,
  error: null
};
