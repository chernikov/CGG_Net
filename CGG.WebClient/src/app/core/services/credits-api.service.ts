import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Transaction } from '../../store/credits/credits.state';

@Injectable({ providedIn: 'root' })
export class CreditsApiService {
  private api = inject(ApiService);

  getBalance(): Observable<number> {
    return this.api.get<number>('credits/balance');
  }

  getTransactions(page: number, pageSize: number): Observable<Transaction[]> {
    return this.api.get<Transaction[]>(`credits/transactions?page=${page}&pageSize=${pageSize}`);
  }
}
