import {
  Component, Input, Output, EventEmitter, OnChanges, inject, signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveyStepDef } from '../../models/question.model';
import { StepAnswer } from '../../models/survey-session.model';
import { QuestionRendererComponent } from '../question-renderer/question-renderer.component';

/**
 * Renders all questions of a single survey step.
 * The user navigates question by question within the step.
 * Emits `stepComplete` when all required questions are answered.
 */
@Component({
  selector: 'app-survey-step',
  standalone: true,
  imports: [CommonModule, QuestionRendererComponent],
  template: `
    <div class="w-full max-w-2xl mx-auto px-4 py-8">

      <!-- Progress header -->
      <div class="flex items-center justify-between mb-6">
        <span class="text-sm font-medium text-slate-500">
          Крок {{ step.stepNumber }} / {{ totalSteps }}
        </span>
        <div class="flex-1 mx-4 h-2 bg-slate-100 rounded-full overflow-hidden">
          <div
            class="h-full bg-blue-500 rounded-full transition-all duration-500"
            [style.width.%]="progressPct"
          ></div>
        </div>
        <span class="text-sm font-medium text-slate-500">
          Питання {{ questionIndex() + 1 }}/{{ step.questions.length }}
        </span>
      </div>

      <!-- Current question -->
      @if (currentQuestion(); as q) {
        <app-question-renderer
          [question]="q"
          [value]="answers()[q.id] || ''"
          (valueChange)="onAnswer(q.id, $event)"
        />
      }

      <!-- Navigation -->
      <div class="flex justify-between mt-8">
        <button
          type="button"
          class="px-6 py-3 rounded-xl border-2 border-slate-200 text-slate-600 font-medium hover:bg-slate-50 transition disabled:opacity-40 disabled:cursor-not-allowed"
          [disabled]="questionIndex() === 0"
          (click)="prevQuestion()"
        >
          ← Назад
        </button>

        @if (isLastQuestion()) {
          <button
            type="button"
            class="px-8 py-3 rounded-xl bg-blue-600 hover:bg-blue-700 text-white font-semibold transition disabled:opacity-40 disabled:cursor-not-allowed"
            [disabled]="!canProceed()"
            (click)="submitStep()"
          >
            {{ isLastStep ? 'Завершити →' : 'Далі →' }}
          </button>
        } @else {
          <button
            type="button"
            class="px-8 py-3 rounded-xl bg-blue-600 hover:bg-blue-700 text-white font-semibold transition disabled:opacity-40 disabled:cursor-not-allowed"
            [disabled]="!canAdvanceQuestion()"
            (click)="nextQuestion()"
          >
            Наступне →
          </button>
        }
      </div>
    </div>
  `,
})
export class SurveyStepComponent implements OnChanges {
  @Input() step!: SurveyStepDef;
  @Input() totalSteps: number = 1;
  @Input() isLastStep: boolean = false;
  /** Pre-fill with existing session answers */
  @Input() existingAnswers: StepAnswer[] = [];
  @Output() stepComplete = new EventEmitter<StepAnswer[]>();

  questionIndex = signal(0);
  answers = signal<Record<string, string>>({});

  get progressPct(): number {
    return ((this.step.stepNumber - 1) / this.totalSteps) * 100;
  }

  ngOnChanges(): void {
    // Restore previous answers if user navigated back
    const restored: Record<string, string> = {};
    for (const a of (this.existingAnswers ?? [])) {
      restored[a.questionId] = a.answer;
    }
    this.answers.set(restored);
    this.questionIndex.set(0);
  }

  currentQuestion() {
    return this.step?.questions?.[this.questionIndex()] ?? null;
  }

  isLastQuestion(): boolean {
    return this.questionIndex() === (this.step?.questions?.length ?? 1) - 1;
  }

  onAnswer(questionId: string, value: string): void {
    this.answers.update(prev => ({ ...prev, [questionId]: value }));
  }

  /** Current question has a non-empty answer */
  canAdvanceQuestion(): boolean {
    const q = this.currentQuestion();
    if (!q) return false;
    const val = this.answers()[q.id] ?? '';
    return val.trim().length > 0;
  }

  canProceed(): boolean {
    const required = this.step.questions.filter(q => q.type !== 'feedback');
    return required.every(q => (this.answers()[q.id] ?? '').trim().length > 0);
  }

  nextQuestion(): void {
    if (!this.isLastQuestion()) {
      this.questionIndex.update(i => i + 1);
    }
  }

  prevQuestion(): void {
    if (this.questionIndex() > 0) {
      this.questionIndex.update(i => i - 1);
    }
  }

  submitStep(): void {
    const stepAnswers: StepAnswer[] = this.step.questions.map(q => ({
      questionId: q.id,
      questionText: '', // filled by parent from locale
      answer: this.answers()[q.id] ?? '',
    }));
    this.stepComplete.emit(stepAnswers);
  }
}
