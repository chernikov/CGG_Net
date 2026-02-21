import { Component } from '@angular/core';
import { CommonModule, AsyncPipe } from '@angular/common';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { selectIsAuthenticated } from '../../../store/auth/auth.selectors';
import * as AuthActions from '../../../store/auth/auth.actions';

@Component({
  selector: 'app-header',
  imports: [CommonModule, AsyncPipe],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class HeaderComponent {
  isAuthenticated$: Observable<boolean>;

  constructor(private router: Router, private store: Store) {
    this.isAuthenticated$ = this.store.select(selectIsAuthenticated);
  }

  navigateToLanding() {
    this.router.navigate(['/landing']);
  }

  navigateToLogin() {
    this.router.navigate(['/login']);
  }

  logout() {
    this.store.dispatch(AuthActions.logout());
  }
}
