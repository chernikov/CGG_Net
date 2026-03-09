import { Component, Output, EventEmitter, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

const FEEDBACK_RATINGS = [1, 2, 3, 4, 5];

@Component({
  selector: 'app-feedback-step',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './feedback-step.component.html',
})
export class FeedbackStepComponent {
  @Output() feedbackComplete = new EventEmitter<{ rating: number; comment: string }>();
  @Output() back = new EventEmitter<void>();

  stars = FEEDBACK_RATINGS;
  rating = signal<number>(5);
  comment = '';

  submit(): void {
    this.feedbackComplete.emit({ rating: this.rating(), comment: this.comment });
  }
}
