import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-landing',
  imports: [CommonModule],
  templateUrl: './landing.html',
  styleUrl: './landing.scss'
})
export class LandingComponent {
  constructor(private router: Router) {}

  navigateToParent() {
    this.router.navigate(['/parent-choice']);
  }

  navigateToSchool() {
    this.router.navigate(['/school-choice']);
  }

  navigateToStudent() {
    this.router.navigate(['/student-choice']);
  }
}

