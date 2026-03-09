import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

const STARS = [1, 2, 3, 4, 5];

@Component({
  selector: 'app-rating-question',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './rating-question.component.html',
})
export class RatingQuestionComponent implements OnChanges {
  @Input() value: string = '';
  @Output() valueChange = new EventEmitter<string>();

  stars = STARS;
  selected: number | null = null;

  ngOnChanges(): void {
    this.selected = this.value ? parseInt(this.value, 10) : null;
  }

  select(star: number): void {
    this.selected = star;
    this.valueChange.emit(String(star));
  }
}
