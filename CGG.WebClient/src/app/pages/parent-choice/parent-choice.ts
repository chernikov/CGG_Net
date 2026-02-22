import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CarouselComponent, CarouselSlide } from '../../shared/components/carousel/carousel';

@Component({
  selector: 'app-parent-choice',
  imports: [CarouselComponent],
  templateUrl: './parent-choice.html',
  styleUrl: './parent-choice.scss'
})
export class ParentChoiceComponent {
  slides: CarouselSlide[] = [
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

  tryDemo() {
    this.router.navigate(['/demo']);
  }

  joinGuild() {
    this.router.navigate(['/parent-register']);
  }
}
