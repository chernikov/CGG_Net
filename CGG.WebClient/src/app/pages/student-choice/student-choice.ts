import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CarouselComponent, CarouselSlide } from '../../shared/components/carousel/carousel';

@Component({
  selector: 'app-student-choice',
  imports: [CarouselComponent],
  templateUrl: './student-choice.html',
  styleUrl: './student-choice.scss'
})
export class StudentChoiceComponent {
  slides: CarouselSlide[] = [
    {
      id: 1,
      image: '/assets/images/castle-path.png',
      title: 'FIND YOUR PATH',
      subtitle: 'WITHOUT RUSHING INTO THE WRONG ONE',
      description: 'Navigate your career journey with confidence and clarity',
      secondaryButton: 'MORE FOR PARENTS',
      primaryButton: 'START JOURNEY',
      secondaryAction: () => this.moreForParents(),
      primaryAction: () => this.startSurvey()
    },
    {
      id: 2,
      image: '/assets/images/portal-confidence.png',
      title: 'LIGHT UP YOUR PATH',
      subtitle: 'INTO NEW WAYS OF CONFIDENCE',
      description: 'Discover new possibilities and unlock your potential',
      secondaryButton: 'MORE FOR PARENTS',
      primaryButton: 'START JOURNEY',
      secondaryAction: () => this.moreForParents(),
      primaryAction: () => this.startSurvey()
    }
  ];

  constructor(private router: Router) {}

  moreForParents() {
    this.router.navigate(['/parent-choice']);
  }

  startSurvey() {
    this.router.navigate(['/survey']);
  }
}
