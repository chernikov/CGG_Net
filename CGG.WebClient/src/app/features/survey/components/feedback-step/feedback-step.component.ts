import { Component, Output, EventEmitter, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

const FEEDBACK_RATINGS = [1, 2, 3, 4, 5];

@Component({
  selector: 'app-feedback-step',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="w-full max-w-2xl mx-auto px-4 py-8">
      <div class="text-center mb-8">
        <div class="text-5xl mb-4">💬</div>
        <h2 class="text-2xl font-bold text-slate-800 mb-2">Зворотній зв'язок</h2>
        <p class="text-slate-500">Допоможіть нам стати кращими</p>
      </div>

      <div class="bg-white rounded-2xl shadow-md p-6 md:p-8 flex flex-col gap-6">

        <!-- Overall rating -->
        <div>
          <label class="block text-base font-semibold text-slate-700 mb-3">
            Як вам опитувальник?
          </label>
          <div class="flex gap-3 justify-center">
            @for (star of stars; track star) {
              <button type="button"
                class="text-4xl transition-transform hover:scale-110"
                [class.opacity-30]="rating() !== null && rating()! < star"
                (click)="rating.set(star)"
              >⭐</button>
            }
          </div>
        </div>

        <!-- Comment -->
        <div>
          <label class="block text-base font-semibold text-slate-700 mb-2">
            Коментар (необов'язково)
          </label>
          <textarea
            class="w-full px-4 py-3 rounded-xl border-2 border-slate-200 focus:border-blue-400 outline-none resize-none text-slate-800"
            rows="4"
            placeholder="Що сподобалось? Що можна покращити?"
            [(ngModel)]="comment"
          ></textarea>
        </div>

        <button
          type="button"
          class="w-full py-4 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded-xl transition"
          (click)="submit()"
        >
          Переглянути результати →
        </button>
      </div>
    </div>
  `,
})
export class FeedbackStepComponent {
  @Output() feedbackComplete = new EventEmitter<{ rating: number | null; comment: string }>();

  stars = FEEDBACK_RATINGS;
  rating = signal<number | null>(null);
  comment = '';

  submit(): void {
    this.feedbackComplete.emit({ rating: this.rating(), comment: this.comment });
  }
}
