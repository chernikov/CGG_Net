import { Injectable, inject } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable } from 'rxjs';
import { ChildProfile } from '../../../store/family/family.state';

export type { ChildProfile };

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
    return this.apiService.get<ChildProfile[]>('family/children');
  }

  addChild(request: AddChildRequest): Observable<any> {
    return this.apiService.post<any>('family/children', request);
  }
}
