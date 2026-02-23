export interface ChildProfile {
  id: string;
  name: string;
  avatarUrl?: string;
  email?: string;
  age?: number;
  gender?: string;
  createdAt?: string;
}

export interface FamilyState {
  children: ChildProfile[];
  loading: boolean;
  loaded: boolean;
  error: string | null;
  addingChild: boolean;
  addChildError: string | null;
}

export const initialFamilyState: FamilyState = {
  children: [],
  loading: false,
  loaded: false,
  error: null,
  addingChild: false,
  addChildError: null
};
