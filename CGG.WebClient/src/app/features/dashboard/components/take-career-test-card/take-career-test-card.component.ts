import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-take-career-test-card',
  standalone: true,
  templateUrl: './take-career-test-card.component.html',
  styleUrl: './take-career-test-card.component.scss',
})
export class TakeCareerTestCardComponent {
  private router = inject(Router);

  takeSurvey(): void {
    this.router.navigate(['/survey'], { queryParams: { name: 'intro' } });
  }
}
