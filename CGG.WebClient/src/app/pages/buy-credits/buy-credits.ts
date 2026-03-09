import { Component, inject, OnDestroy, OnInit, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import { BackHeaderComponent } from '../../shared/components/back-header/back-header.component';
import * as CreditsActions from '../../store/credits/credits.actions';
import {
  selectTopupLoading,
  selectTopupResult,
  selectTopupError,
} from '../../store/credits/credits.selectors';

@Component({
  selector: 'app-buy-credits',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule, BackHeaderComponent],
  templateUrl: './buy-credits.html',
  styleUrl: './buy-credits.scss',
})
export class BuyCreditsComponent implements OnInit, OnDestroy {
  private store = inject(Store);

  loading = this.store.selectSignal(selectTopupLoading);
  result = this.store.selectSignal(selectTopupResult);
  error = this.store.selectSignal(selectTopupError);

  amountUAH: number | null = null;

  readonly presets = [50, 100, 200, 300, 500, 1000];
  readonly bonusThreshold = 300;

  constructor() {
    // Redirect to payment page when URL is ready
    effect(() => {
      const res = this.result();
      if (res?.paymentUrl) {
        window.location.href = res.paymentUrl;
      }
    });
  }

  ngOnInit(): void {
    this.store.dispatch(CreditsActions.clearTopupState());
  }

  ngOnDestroy(): void {
    this.store.dispatch(CreditsActions.clearTopupState());
  }

  selectPreset(value: number): void {
    this.amountUAH = value;
  }

  calculateCredits(amount: number | null): number {
    if (!amount || amount <= 0) return 0;
    return amount >= this.bonusThreshold
      ? Math.floor(amount * 1.4)
      : Math.floor(amount);
  }

  hasBonus(amount: number | null): boolean {
    return !!amount && amount >= this.bonusThreshold;
  }

  submit(): void {
    if (!this.amountUAH || this.amountUAH < 1) return;
    this.store.dispatch(CreditsActions.createPayment({ amountUAH: this.amountUAH }));
  }
}
