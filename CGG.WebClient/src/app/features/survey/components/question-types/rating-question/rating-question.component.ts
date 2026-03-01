import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

const STARS = [1, 2, 3, 4, 5];

@Component({
  selector: 'app-rating-question',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="flex gap-3 justify-center">
      @for (star of stars; track star) {
        <button
          type="button"
          class="text-4xl transition-transform duration-150 hover:scale-110 focus:outline-none"
          [class.opacity-30]="selected !== null && selected < star"
          (click)="select(star)"
          [title]="star + ' / 5'"
        >⭐</button>
      }
    </div>
    @if (selected) {
      <p class="text-center text-sm text-slate-500 mt-2">{{ selected }} / 5</p>
    }
  `,
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
