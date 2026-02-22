export enum UserRole {
  UserChild = 0,
  UserParent = 1,
  Teacher = 2,
  Admin = 3,
  UserStudent = 4
}

export type ContextType = 'System' | 'Family' | 'School';

export interface UserTokenContext {
  type: ContextType;
  contextId?: string;
  contextRole?: string;
  contextName?: string;
}

export interface User {
  id: string;
  email: string;
  firstName: string | null;
  surname: string | null;
  role: string;
  credits: number;
}

export interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
  activeContext: UserTokenContext | null;
  availableContexts: UserTokenContext[];
  canSwitchContext: boolean;
  switchContextLoading: boolean;
}

export const initialAuthState: AuthState = {
  user: null,
  token: null,
  isAuthenticated: false,
  loading: false,
  error: null,
  activeContext: null,
  availableContexts: [],
  canSwitchContext: false,
  switchContextLoading: false
};
