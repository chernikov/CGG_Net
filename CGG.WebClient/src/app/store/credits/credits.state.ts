export interface Transaction {
  id: string;
  amount: number;
  amountUAH: number;
  creditsGranted: number;
  type: string;
  description: string | null;
  status: string;
  orderId: string | null;
  paymentUrl: string | null;
  createdAt: string;
}

export interface TopupResult {
  orderId: string;
  paymentUrl: string | null;
  invoiceId: string | null;
  amountUAH: number;
  creditsToReceive: number;
}

export interface CreditsState {
  transactions: Transaction[];
  loading: boolean;
  loaded: boolean;
  error: string | null;
  page: number;
  hasMore: boolean;

  topupLoading: boolean;
  topupResult: TopupResult | null;
  topupError: string | null;
}

export const initialCreditsState: CreditsState = {
  transactions: [],
  loading: false,
  loaded: false,
  error: null,
  page: 1,
  hasMore: true,

  topupLoading: false,
  topupResult: null,
  topupError: null,
};
