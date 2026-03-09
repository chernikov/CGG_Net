export interface Transaction {
  id: string;
  amount: number;
  type: string;
  description: string | null;
  status: string;
  createdAt: string;
}

export interface CreditsState {
  transactions: Transaction[];
  loading: boolean;
  loaded: boolean;
  error: string | null;
  page: number;
  hasMore: boolean;
}

export const initialCreditsState: CreditsState = {
  transactions: [],
  loading: false,
  loaded: false,
  error: null,
  page: 1,
  hasMore: true,
};
