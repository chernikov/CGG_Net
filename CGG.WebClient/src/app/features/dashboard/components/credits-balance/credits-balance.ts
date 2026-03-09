import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import { selectUserCredits } from '../../../../store/auth/auth.selectors';

@Component({
  selector: 'app-credits-balance',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './credits-balance.html',
  styleUrl: './credits-balance.scss',
})
export class CreditsBalanceComponent {
  private store = inject(Store);
  private router = inject(Router);

  credits = this.store.selectSignal(selectUserCredits);

  goToTransactions(): void {
    this.router.navigate(['/transactions']);
  }
}
