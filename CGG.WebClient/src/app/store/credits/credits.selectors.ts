import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CreditsState } from './credits.state';

export const selectCreditsState = createFeatureSelector<CreditsState>('credits');

export const selectTransactions = createSelector(
  selectCreditsState,
  (state) => state.transactions
);

export const selectTransactionsLoading = createSelector(
  selectCreditsState,
  (state) => state.loading
);

export const selectTransactionsHasMore = createSelector(
  selectCreditsState,
  (state) => state.hasMore
);

export const selectCreditsError = createSelector(
  selectCreditsState,
  (state) => state.error
);

export const selectCreditsPage = createSelector(
  selectCreditsState,
  (state) => state.page
);

// Topup selectors
export const selectTopupLoading = createSelector(
  selectCreditsState,
  (state) => state.topupLoading
);

export const selectTopupResult = createSelector(
  selectCreditsState,
  (state) => state.topupResult
);

export const selectTopupError = createSelector(
  selectCreditsState,
  (state) => state.topupError
);
