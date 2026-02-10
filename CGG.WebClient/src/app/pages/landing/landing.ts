import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CarouselComponent, CarouselSlide } from '../../shared/components/carousel/carousel';

@Component({
  selector: 'app-landing',
  imports: [CarouselComponent],
  templateUrl: './landing.html',
  styleUrl: './landing.scss'
})
export class LandingComponent {
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
    },
    {
      id: 3,
      image: '/assets/images/family-support.png',
      title: '[1] SUPPORT THEIR FUTURE',
      subtitle: 'WITHOUT GUESSING',
      description: 'Help your family make informed career decisions',
      secondaryButton: 'TRY DEMO SURVEY',
      primaryButton: 'JOIN THE GUILD',
      secondaryAction: () => this.tryDemo(),
      primaryAction: () => this.joinGuild()
    },
    {
      id: 4,
      image: '/assets/images/family-balance.png',
      title: '[2] MORE ABOUT THE FAMILY',
      subtitle: 'BALANCES INSIDE GUILD',
      description: 'Learn how families grow together in our community',
      secondaryButton: 'TRY DEMO SURVEY',
      primaryButton: 'JOIN THE GUILD',
      secondaryAction: () => this.tryDemo(),
      primaryAction: () => this.joinGuild()
    }
  ];

  constructor(private router: Router) {}

  moreForParents() {
    // TODO: Navigate to parents info page
    console.log('More for parents');
  }

  startSurvey() {
    this.router.navigate(['/survey']);
  }

  tryDemo() {
    this.router.navigate(['/demo']);
  }

  joinGuild() {
    this.router.navigate(['/register']);
  }
}

