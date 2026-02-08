import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { trigger, transition, style, animate } from '@angular/animations';

export interface CarouselSlide {
  id: number;
  image: string;
  title: string;
  subtitle?: string;
  description: string;
  primaryButton: string;
  secondaryButton?: string;
  primaryAction: () => void;
  secondaryAction?: () => void;
}

@Component({
  selector: 'app-carousel',
  imports: [CommonModule],
  templateUrl: './carousel.html',
  styleUrl: './carousel.scss',
  animations: [
    trigger('slideAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateX(100%)' }),
        animate('500ms ease-out', style({ opacity: 1, transform: 'translateX(0)' }))
      ]),
      transition(':leave', [
        animate('500ms ease-out', style({ opacity: 0, transform: 'translateX(-100%)' }))
      ])
    ])
  ]
})
export class CarouselComponent {
  @Input() slides: CarouselSlide[] = [];
  @Input() autoPlayInterval = 5000;
  
  currentIndex = 0;
  autoPlayTimer: any;

  get progressPercentage(): number {
    return ((this.currentIndex + 1) / this.slides.length) * 100;
  }

  ngOnInit() {
    this.startAutoPlay();
  }

  ngOnDestroy() {
    this.stopAutoPlay();
  }

  startAutoPlay() {
    this.autoPlayTimer = setInterval(() => {
      this.next();
    }, this.autoPlayInterval);
  }

  stopAutoPlay() {
    if (this.autoPlayTimer) {
      clearInterval(this.autoPlayTimer);
    }
  }

  next() {
    this.currentIndex = (this.currentIndex + 1) % this.slides.length;
  }

  previous() {
    this.currentIndex = (this.currentIndex - 1 + this.slides.length) % this.slides.length;
  }

  goToSlide(index: number) {
    this.currentIndex = index;
    this.stopAutoPlay();
    this.startAutoPlay();
  }

  onUserInteraction() {
    this.stopAutoPlay();
    this.startAutoPlay();
  }
}

