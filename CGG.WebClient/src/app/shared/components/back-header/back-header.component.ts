import { Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-back-header',
  standalone: true,
  imports: [],
  templateUrl: './back-header.component.html',
  styleUrl: './back-header.component.scss',
})
export class BackHeaderComponent {
  private router = inject(Router);

  /** Target URL for the back button. Defaults to /dashboard. */
  backUrl = input<string>('/dashboard');
  /** Button label. Defaults to "На головну". */
  backLabel = input<string>('На головну');

  goBack(): void {
    this.router.navigateByUrl(this.backUrl());
  }
}
