import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Store } from '@ngrx/store';
import { updateUserCredits } from '../../store/auth/auth.actions';
import { environment } from '../../../environments/environment';

type PaymentStatus = 'loading' | 'success' | 'pending' | 'failure';

interface PaymentStatusDto {
  orderId: string;
  status: string;
  amountUAH: number;
  creditsGranted: number;
  description: string | null;
}

@Component({
  selector: 'app-payment-result',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './payment-result.html',
  styleUrl: './payment-result.scss',
})
export class PaymentResultComponent implements OnInit, OnDestroy {
  private route = inject(ActivatedRoute);
  private http = inject(HttpClient);
  private store = inject(Store);

  view: PaymentStatus = 'loading';
  transaction: PaymentStatusDto | null = null;
  orderId: string | null = null;

  private pollInterval: ReturnType<typeof setInterval> | null = null;
  private pollCount = 0;
  private readonly maxPolls = 10;

  ngOnInit(): void {
    this.orderId = this.route.snapshot.queryParamMap.get('orderId');
    if (!this.orderId) {
      this.view = 'failure';
      return;
    }
    this.checkStatus();
  }

  ngOnDestroy(): void {
    this.stopPolling();
  }

  private checkStatus(): void {
    this.http
      .get<PaymentStatusDto>(`${environment.apiUrl}/credits/payment/status`, {
        params: { orderId: this.orderId! },
      })
      .subscribe({
        next: (data) => {
          this.transaction = data;
          if (data.status === 'success' || data.status === 'completed') {
            this.view = 'success';
            this.stopPolling();
            this.refreshBalance();
          } else if (data.status === 'failure' || data.status === 'failed' || data.status === 'error') {
            this.view = 'failure';
            this.stopPolling();
          } else {
            // pending — keep polling
            this.view = 'pending';
            this.startPolling();
          }
        },
        error: () => {
          this.view = 'failure';
          this.stopPolling();
        },
      });
  }

  private refreshBalance(): void {
    this.http.get<number>(`${environment.apiUrl}/credits/balance`).subscribe({
      next: (credits) => this.store.dispatch(updateUserCredits({ credits })),
      error: () => { /* non-critical */ },
    });
  }

  private startPolling(): void {
    if (this.pollInterval) return;
    this.pollInterval = setInterval(() => {
      this.pollCount++;
      if (this.pollCount >= this.maxPolls) {
        this.stopPolling();
        return;
      }
      this.checkStatus();
    }, 3000);
  }

  private stopPolling(): void {
    if (this.pollInterval) {
      clearInterval(this.pollInterval);
      this.pollInterval = null;
    }
  }
}
