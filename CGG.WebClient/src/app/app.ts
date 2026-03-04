import { Component, signal, OnInit, OnDestroy, HostListener, isDevMode } from '@angular/core';
import { Router, RouterOutlet, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { filter, map } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { HeaderComponent } from './shared/components/header/header';
import { SurveyDebugPanelComponent } from './shared/components/survey-debug-panel/survey-debug-panel.component';
import * as AuthActions from './store/auth/auth.actions';
import { LanguageService } from './core/services/language.service';

const DESKTOP_BREAKPOINT = 768;

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, HeaderComponent, SurveyDebugPanelComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit, OnDestroy {
  protected readonly title = signal('CGG.WebClient');
  protected readonly isAdminRoute = signal(false);
  protected readonly isDesktop = signal(window.innerWidth >= DESKTOP_BREAKPOINT);
  protected readonly isDevMode = isDevMode();

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

  @HostListener('window:resize')
  onResize(): void {
    this.isDesktop.set(window.innerWidth >= DESKTOP_BREAKPOINT);
  }

  ngOnInit() {
    // Load user from localStorage on app initialization
    this.store.dispatch(AuthActions.loadUserFromStorage());
  }

  ngOnDestroy() {}
}
