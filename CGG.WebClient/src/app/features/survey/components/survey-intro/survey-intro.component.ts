import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveyDef } from '../../models/question.model';

@Component({
  selector: 'app-survey-intro',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="flex flex-col items-center justify-center min-h-[60vh] text-center px-4 py-12">
      <div class="max-w-2xl w-full">
        <div class="text-6xl mb-6">🎯</div>
        <h1 class="text-3xl md:text-4xl font-bold text-slate-800 mb-4">
          {{ survey.title }}
        </h1>
        @if (survey.description) {
          <p class="text-slate-500 text-base md:text-lg mb-8 leading-relaxed">
            {{ survey.description }}
          </p>
        }

        <div class="flex items-center justify-center gap-6 mb-10 text-sm text-slate-500">
          <div class="flex items-center gap-2">
            <span class="text-blue-500 text-xl">📝</span>
            <span>{{ survey.steps.length }} кроків</span>
          </div>
          <div class="flex items-center gap-2">
            <span class="text-blue-500 text-xl">🤖</span>
            <span>AI аналіз після кожного кроку</span>
          </div>
          <div class="flex items-center gap-2">
            <span class="text-blue-500 text-xl">⏱</span>
            <span>~10 хвилин</span>
          </div>
        </div>

        <button
          type="button"
          class="px-10 py-4 bg-blue-600 hover:bg-blue-700 text-white text-lg font-semibold rounded-2xl shadow-md hover:shadow-lg transition-all duration-200 active:scale-95"
          (click)="start.emit()"
        >
          Розпочати опитувальник →
        </button>
      </div>
    </div>
  `,
})
export class SurveyIntroComponent {
  @Input() survey!: SurveyDef;
  @Output() start = new EventEmitter<void>();
}
