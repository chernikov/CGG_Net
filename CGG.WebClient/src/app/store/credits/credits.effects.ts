import { inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { switchMap, catchError, map, withLatestFrom } from 'rxjs/operators';
import { of } from 'rxjs';
import * as CreditsActions from './credits.actions';
import { selectCreditsPage } from './credits.selectors';
import { CreditsApiService } from '../../core/services/credits-api.service';

export class CreditsEffects {
  private actions$ = inject(Actions);
  private store = inject(Store);
  private creditsApi = inject(CreditsApiService);

  loadTransactions$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CreditsActions.loadTransactions),
      switchMap(() =>
        this.creditsApi.getTransactions(1, 20).pipe(
          map((transactions) =>
            CreditsActions.loadTransactionsSuccess({ transactions, page: 1 })
          ),
          catchError((error) =>
            of(CreditsActions.loadTransactionsFailure({
              error: error.error?.message || 'Failed to load transactions'
            }))
          )
        )
      )
    )
  );

  loadMoreTransactions$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CreditsActions.loadMoreTransactions),
      withLatestFrom(this.store.select(selectCreditsPage)),
      switchMap(([, currentPage]) => {
        const nextPage = currentPage + 1;
        return this.creditsApi.getTransactions(nextPage, 20).pipe(
          map((transactions) =>
            CreditsActions.loadTransactionsSuccess({ transactions, page: nextPage })
          ),
          catchError((error) =>
            of(CreditsActions.loadTransactionsFailure({
              error: error.error?.message || 'Failed to load transactions'
            }))
          )
        );
      })
    )
  );
}
