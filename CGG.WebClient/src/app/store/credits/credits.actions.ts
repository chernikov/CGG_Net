import { createAction, props } from '@ngrx/store';
import { Transaction } from './credits.state';

export const loadTransactions = createAction('[Credits] Load Transactions');

export const loadTransactionsSuccess = createAction(
  '[Credits] Load Transactions Success',
  props<{ transactions: Transaction[]; page: number }>()
);

export const loadTransactionsFailure = createAction(
  '[Credits] Load Transactions Failure',
  props<{ error: string }>()
);

export const loadMoreTransactions = createAction('[Credits] Load More Transactions');

export const clearCreditsError = createAction('[Credits] Clear Error');
