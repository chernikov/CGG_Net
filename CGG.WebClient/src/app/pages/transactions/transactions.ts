import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import { BackHeaderComponent } from '../../shared/components/back-header/back-header.component';
import { CreditsBalanceComponent } from '../../features/dashboard/components/credits-balance/credits-balance';
import * as CreditsActions from '../../store/credits/credits.actions';
import {
  selectTransactions,
  selectTransactionsLoading,
  selectTransactionsHasMore,
} from '../../store/credits/credits.selectors';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [CommonModule, TranslateModule, BackHeaderComponent, CreditsBalanceComponent],
  templateUrl: './transactions.html',
  styleUrl: './transactions.scss',
})
export class TransactionsComponent implements OnInit, OnDestroy {
  private store = inject(Store);

  transactions = this.store.selectSignal(selectTransactions);
  loading = this.store.selectSignal(selectTransactionsLoading);
  hasMore = this.store.selectSignal(selectTransactionsHasMore);

  ngOnInit(): void {
    this.store.dispatch(CreditsActions.loadTransactions());
  }

  ngOnDestroy(): void {
    this.store.dispatch(CreditsActions.clearCreditsError());
  }

  loadMore(): void {
    this.store.dispatch(CreditsActions.loadMoreTransactions());
  }
}
