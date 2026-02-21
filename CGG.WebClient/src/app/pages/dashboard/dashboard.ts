import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { map } from 'rxjs/operators';
import { selectToken } from '../../store/auth/auth.selectors';

interface JwtClaim {
  key: string;
  value: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent implements OnInit {
  private store = inject(Store);

  claims: JwtClaim[] = [];
  rawToken = '';

  ngOnInit(): void {
    this.store.select(selectToken).subscribe((token) => {
      this.rawToken = token ?? '';
      this.claims = token ? this.decodeJwt(token) : [];
    });
  }

  private decodeJwt(token: string): JwtClaim[] {
    try {
      const payload = token.split('.')[1];
      const decoded = JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')));
      return Object.entries(decoded).map(([key, value]) => ({
        key,
        value: typeof value === 'object' ? JSON.stringify(value) : String(value)
      }));
    } catch {
      return [{ key: 'error', value: 'Failed to decode token' }];
    }
  }
}
