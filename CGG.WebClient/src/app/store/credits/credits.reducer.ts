import { createReducer, on } from '@ngrx/store';
import { initialCreditsState } from './credits.state';
import * as CreditsActions from './credits.actions';

const PAGE_SIZE = 20;

export const creditsReducer = createReducer(
  initialCreditsState,

  on(CreditsActions.loadTransactions, (state) => ({
    ...state,
    loading: true,
    error: null,
    page: 1,
    transactions: [],
    hasMore: true,
  })),

  on(CreditsActions.loadMoreTransactions, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(CreditsActions.loadTransactionsSuccess, (state, { transactions, page }) => ({
    ...state,
    loading: false,
    loaded: true,
    transactions: page === 1 ? transactions : [...state.transactions, ...transactions],
    page,
    hasMore: transactions.length >= PAGE_SIZE,
  })),

  on(CreditsActions.loadTransactionsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  on(CreditsActions.clearCreditsError, (state) => ({
    ...state,
    error: null,
  })),

  // Topup
  on(CreditsActions.createPayment, (state) => ({
    ...state,
    topupLoading: true,
    topupResult: null,
    topupError: null,
  })),

  on(CreditsActions.createPaymentSuccess, (state, { result }) => ({
    ...state,
    topupLoading: false,
    topupResult: result,
    topupError: null,
  })),

  on(CreditsActions.createPaymentFailure, (state, { error }) => ({
    ...state,
    topupLoading: false,
    topupError: error,
  })),

  on(CreditsActions.clearTopupState, (state) => ({
    ...state,
    topupLoading: false,
    topupResult: null,
    topupError: null,
  })),
);
