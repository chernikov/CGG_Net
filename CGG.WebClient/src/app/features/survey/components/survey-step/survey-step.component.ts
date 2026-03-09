import {
  Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, inject, signal, computed,
  ChangeDetectionStrategy, ChangeDetectorRef, HostListener
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveyStepDef, SurveyQuestion } from '../../models/question.model';
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
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, QuestionRendererComponent],
  templateUrl: './survey-step.component.html',
})
export class SurveyStepComponent implements OnChanges {
  @Input() step!: SurveyStepDef;
  @Input() totalSteps: number = 1;
  @Input() isLastStep: boolean = false;
  @Input() existingAnswers: StepAnswer[] = [];
  @Output() stepComplete = new EventEmitter<StepAnswer[]>();
  /** Emitted on every "Наступне" click with all answers collected so far in this step. */
  @Output() questionAnswered = new EventEmitter<StepAnswer[]>();
  /** Emitted after debug autofill — allows parent to persist partial answers in session. */
  @Output() autofillApplied = new EventEmitter<StepAnswer[]>();

  private cdr = inject(ChangeDetectorRef);

  questionIndex = signal(0);
  answers = signal<Record<string, string>>({});

  visibleQuestions = computed(() =>
    (this.step?.questions ?? []).filter(q => this.isVisible(q))
  );

  currentQuestion = computed(() => this.visibleQuestions()[this.questionIndex()] ?? null);
  isLastQuestion = computed(() => this.questionIndex() === (this.visibleQuestions().length || 1) - 1);
  canAdvanceQuestion = computed(() => {
    const q = this.currentQuestion();
    if (!q) return false;
    return (this.answers()[q.id] ?? '').trim().length > 0;
  });
  canProceed = computed(() => {
    const required = this.visibleQuestions().filter(q => q.type !== 'feedback');
    return required.every(q => (this.answers()[q.id] ?? '').trim().length > 0);
  });

  get progressPct(): number {
    return ((this.step.stepNumber - 1) / this.totalSteps) * 100;
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Only reset when the step itself changes, NOT when existingAnswers gets a new reference
    if (changes['step']) {
      const restored: Record<string, string> = {};
      for (const a of (this.existingAnswers ?? [])) {
        restored[a.questionId] = a.answer;
      }
      this.answers.set(restored);
      this.questionIndex.set(0);
    }
  }

  onAnswer(questionId: string, value: string): void {
    this.answers.set({ ...this.answers(), [questionId]: value });
    this.cdr.markForCheck();
  }

  nextQuestion(): void {
    if (!this.isLastQuestion()) {
      this.questionAnswered.emit(this.currentAnswersAsStepAnswers());
      this.questionIndex.update(i => i + 1);
    }
  }

  private isVisible(q: SurveyQuestion): boolean {
    if (!q.visibleIf) return true;
    const { field, equals, contains } = q.visibleIf;
    const sourceQ = this.step.questions.find(qq => qq.purpose === field);
    const val = sourceQ ? (this.answers()[sourceQ.id] ?? '') : '';
    if (equals !== undefined) return val === equals;
    if (contains !== undefined) {
      try { return (JSON.parse(val) as string[]).includes(contains); }
      catch { return val.includes(contains); }
    }
    return true;
  }

  prevQuestion(): void {
    if (this.questionIndex() > 0) {
      this.questionIndex.update(i => i - 1);
    }
  }

  @HostListener('window:debugAutofill', ['$event'])
  onDebugAutofill(event: Event): void {
    const data = (event as CustomEvent<Record<string, unknown>>).detail;
    const filled: Record<string, string> = { ...this.answers() };
    for (const q of this.step.questions) {
      if (!q.purpose) continue;
      const val = data[q.purpose];
      if (val === undefined || val === null) continue;
      let strVal = Array.isArray(val) ? JSON.stringify(val) : String(val);

      // For single-choice: resolve label text → option value
      // Profile data may store Ukrainian labels (e.g. "Так") while DB stores
      // snake_case codes (e.g. "yes"). Match by translation text as fallback.
      if (q.type === 'single-choice' && q.options.length > 0) {
        const match = q.options.find(
          o => o.value === strVal || o.translations.some(t => t.text === strVal)
        );
        if (match?.value) strVal = match.value;
      }

      filled[q.id] = strVal;
    }
    this.answers.set(filled);
    this.cdr.markForCheck();
    this.autofillApplied.emit(this.currentAnswersAsStepAnswers());
  }

  submitStep(): void {
    this.stepComplete.emit(this.currentAnswersAsStepAnswers());
  }

  private currentAnswersAsStepAnswers(): StepAnswer[] {
    return this.visibleQuestions().map(q => ({
      questionId: q.id,
      questionText: '', // filled by parent from locale
      answer: this.answers()[q.id] ?? '',
    }));
  }
}
