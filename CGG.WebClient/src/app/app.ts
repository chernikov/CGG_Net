import { Component, signal, OnInit } from '@angular/core';
import { Router, RouterOutlet, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { filter, map } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { HeaderComponent } from './shared/components/header/header';
import * as AuthActions from './store/auth/auth.actions';
import { LanguageService } from './core/services/language.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, HeaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  protected readonly title = signal('CGG.WebClient');
  protected readonly isAdminRoute = signal(false);

  constructor(
    private router: Router,
    private store: Store,
    readonly lang: LanguageService // eagerly initialize language on app start
  ) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(event => (event as NavigationEnd).urlAfterRedirects)
    ).subscribe(url => {
      this.isAdminRoute.set(url.startsWith('/admin'));
    });
  }

  ngOnInit() {
    // Load user from localStorage on app initialization
    this.store.dispatch(AuthActions.loadUserFromStorage());
  }
}
