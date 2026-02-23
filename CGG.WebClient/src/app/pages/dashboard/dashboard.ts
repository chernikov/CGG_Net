import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { selectToken } from '../../store/auth/auth.selectors';
import { ParentComponent } from '../../features/dashboard/components/parent-component/parent-component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ParentComponent],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent implements OnInit {
  private store = inject(Store);

  roles: string[] = [];

  ngOnInit(): void {
    this.store.select(selectToken).subscribe((token) => {
      this.roles = token ? this.extractRoles(token) : [];
    });
  }

  private extractRoles(token: string): string[] {
    try {
      const payload = token.split('.')[1];
      const decoded = JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')));
      const role = decoded['role'] ?? decoded['roles'] ?? decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (!role) return [];
      return Array.isArray(role) ? role : [role];
    } catch {
      return [];
    }
  }
}
