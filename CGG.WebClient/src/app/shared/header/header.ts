import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { Store } from '@ngrx/store';
import { ContextSwitcher } from '../components/context-switcher/context-switcher';
import { selectIsAuthenticated } from '../../store/auth/auth.selectors';
import * as AuthActions from '../../store/auth/auth.actions';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [AsyncPipe, ContextSwitcher],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  private store = inject(Store);

  isAuthenticated$ = this.store.select(selectIsAuthenticated);

  logout(): void {
    this.store.dispatch(AuthActions.logout());
  }
}
