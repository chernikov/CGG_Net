import { Injectable, inject } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable, of, delay } from 'rxjs';

export interface ChildProfile {
  id: string;
  name: string;
  avatarUrl?: string;
  age: number;
}

export interface AddChildRequest {
  name: string;
  email?: string;
  age: number;
  gender: string;
}

@Injectable({
  providedIn: 'root',
})
export class FamilyService {
  private apiService = inject(ApiService);

  getChildren(): Observable<ChildProfile[]> {
    // TODO: Replace with actual backend call when ready
    // return this.apiService.get<ChildProfile[]>('family/children');
    
    // Mocking empty array for now as requested ("0 поки")
    return of([]).pipe(delay(500));
  }

  addChild(request: AddChildRequest): Observable<any> {
    return this.apiService.post<any>('family/children', request);
  }
}
