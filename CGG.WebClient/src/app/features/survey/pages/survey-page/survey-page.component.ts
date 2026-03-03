import {
  Component, OnInit, inject, signal, DestroyRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { switchMap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

import { SurveyApiService } from '../../services/survey-api.service';
import { SurveySessionService } from '../../services/survey-session.service';
import { SurveyLocaleService } from '../../services/survey-locale.service';

import { SurveyDef } from '../../models/question.model';
import { SurveySession, StepAnswer, AiStepResult } from '../../models/survey-session.model';

import { SurveyIntroComponent } from '../../components/survey-intro/survey-intro.component';
import { SurveyStepComponent } from '../../components/survey-step/survey-step.component';
import { FeedbackStepComponent } from '../../components/feedback-step/feedback-step.component';
import { SurveyResultComponent } from '../../components/survey-result/survey-result.component';

type PageView = 'loading' | 'error' | 'intro' | 'step' | 'analyzing' | 'feedback' | 'result';

@Component({
  selector: 'app-survey-page',
  standalone: true,
  imports: [
    CommonModule,
    SurveyIntroComponent,
    SurveyStepComponent,
    FeedbackStepComponent,
    SurveyResultComponent,
  ],
  template: `
    <div class="min-h-screen bg-gradient-to-br from-slate-50 to-blue-50">

      <!-- Loading state -->
      @if (view() === 'loading' || view() === 'analyzing') {
        <div class="flex flex-col items-center justify-center min-h-screen gap-4">
          <div class="w-12 h-12 border-4 border-blue-200 border-t-blue-600 rounded-full animate-spin"></div>
          <p class="text-slate-500 text-base font-medium">
            {{ view() === 'analyzing' ? '🤖 AI аналізує ваші відповіді...' : 'Завантаження...' }}
          </p>
          @if (view() === 'analyzing') {
            <p class="text-slate-400 text-sm">Зазвичай займає 10–20 секунд</p>
          }
        </div>
      }

      <!-- Error state -->
      @if (view() === 'error') {
        <div class="flex flex-col items-center justify-center min-h-screen gap-6 px-4">
          <span class="text-5xl">😕</span>
          <h2 class="text-xl font-bold text-slate-800">Не вдалося завантажити опитувальник</h2>
          <p class="text-slate-500 text-center">{{ errorMsg() }}</p>
          <button
            class="px-6 py-3 bg-blue-600 text-white rounded-xl font-medium hover:bg-blue-700 transition"
            (click)="router.navigate(['/'])"
          >На головну</button>
        </div>
      }

      <!-- Intro -->
      @if (view() === 'intro' && survey()) {
        <app-survey-intro
          [survey]="survey()!"
          (start)="onStart()"
        />
      }

      <!-- Step -->
      @if (view() === 'step' && survey() && currentStepDef()) {
        <app-survey-step
          [step]="currentStepDef()!"
          [totalSteps]="survey()!.steps.length"
          [isLastStep]="currentStepDef()!.stepNumber >= survey()!.steps.length"
          [existingAnswers]="getExistingAnswers()"
          (questionAnswered)="onQuestionAnswered($event)"
          (stepComplete)="onStepComplete($event)"
        />
      }

      <!-- Feedback -->
      @if (view() === 'feedback') {
        <app-feedback-step
          (feedbackComplete)="onFeedbackComplete($event)"
        />
      }

      <!-- Result -->
      @if (view() === 'result' && finalResult()) {
        <app-survey-result
          [aiResult]="finalResult()!"
          (restart)="onRestart()"
          (toDashboard)="router.navigate(['/dashboard'])"
        />
      }
    </div>
  `,
})
export class SurveyPageComponent implements OnInit {
  private route = inject(ActivatedRoute);
  readonly router = inject(Router);
  private api = inject(SurveyApiService);
  private sessionSvc = inject(SurveySessionService);
  private locale = inject(SurveyLocaleService);
  private destroyRef = inject(DestroyRef);

  // ─── State ───────────────────────────────────────────────────────────────
  view = signal<PageView>('loading');
  survey = signal<SurveyDef | null>(null);
  session = signal<SurveySession | null>(null);
  errorMsg = signal('');
  currentStepNumber = signal(1);
  finalResult = signal<AiStepResult | null>(null);

  // ─── Lifecycle ────────────────────────────────────────────────────────────

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        switchMap(params => {
          const name = params.get('name') ?? 'classic';
          return this.api.getSurveyByName(name).pipe(
            catchError(err => {
              this.errorMsg.set(err?.error?.message ?? err?.message ?? 'Невідома помилка');
              this.view.set('error');
              return of(null);
            })
          );
        })
      )
      .subscribe(def => {
        if (!def) return;
        this.survey.set(def);

        // Restore or create session
        const existing = this.sessionSvc.getSession(def.surveyType);
        const sess = existing ?? this.sessionSvc.getOrCreateSession(
          def.surveyType, def.id, def.steps.length, this.locale.currentLang
        );
        this.session.set(sess);

        // Resume from where we left off
        if (sess.currentStep === 'done') {
          const last = this.sessionSvc.getLatestResult(sess);
          this.finalResult.set(last);
          this.view.set('result');
        } else if (sess.currentStep === 'feedback') {
          this.view.set('feedback');
        } else {
          const stepNum = typeof sess.currentStep === 'number' ? sess.currentStep : 1;
          this.currentStepNumber.set(stepNum);
          // Show intro only on first visit (step 1 and no completed steps)
          this.view.set(sess.steps.length === 0 ? 'intro' : 'step');
        }
      });
  }

  // ─── Computed helpers ─────────────────────────────────────────────────────

  currentStepDef() {
    const def = this.survey();
    if (!def) return null;
    return def.steps.find(s => s.stepNumber === this.currentStepNumber()) ?? null;
  }

  getExistingAnswers(): StepAnswer[] {
    const sess = this.session();
    if (!sess) return [];
    return this.sessionSvc.getStepAnswers(sess, this.currentStepNumber());
  }

  // ─── Events ───────────────────────────────────────────────────────────────

  onStart(): void {
    const def = this.survey()!;
    const sess = this.session()!;

    // Call backend to start a new survey pass (marks previous ones Outdated)
    this.api.startSurvey({
      surveyId: def.id,
      surveyType: def.surveyType,
      language: sess.language,
    }).pipe(
      takeUntilDestroyed(this.destroyRef),
      catchError(err => {
        console.warn('Could not start survey on backend:', err);
        return of(null);
      })
    ).subscribe(res => {
      if (res?.success && res.userSurveyId) {
        const updated = this.sessionSvc.saveSession({ ...sess, userSurveyId: res.userSurveyId });
        this.session.set(updated);
      }
      this.view.set('step');
    });
  }

  /** Called on every "Наступне" click — saves partial answers for the current step. */
  onQuestionAnswered(answers: StepAnswer[]): void {
    const sess = this.session()!;
    if (!sess.userSurveyId) return; // not started yet

    const stepDef = this.currentStepDef()!;
    const enriched = answers
      .filter(a => a.answer?.trim())
      .map(a => {
        const q = stepDef.questions.find(q => q.id === a.questionId);
        return { ...a, questionText: q ? this.locale.resolve(q.translations) : '' };
      });

    if (!enriched.length) return;

    this.api.saveAnswer({
      userSurveyId: sess.userSurveyId,
      stepNumber: stepDef.stepNumber,
      answers: enriched,
    }).pipe(
      takeUntilDestroyed(this.destroyRef),
      catchError(err => { console.warn('Could not persist partial answers:', err); return of(null); })
    ).subscribe();
  }

  onStepComplete(answers: StepAnswer[]): void {
    const def = this.survey()!;
    const stepDef = this.currentStepDef()!;
    const sess = this.session()!;

    // Enrich answers with localized question text for AI context
    const enriched: StepAnswer[] = answers.map(a => {
      const q = stepDef.questions.find(q => q.id === a.questionId);
      return {
        ...a,
        questionText: q ? this.locale.resolve(q.translations) : '',
      };
    });

    // Save answers and advance session step pointer
    let updatedSess = this.sessionSvc.completeStep(sess, stepDef.stepNumber, enriched);
    this.session.set(updatedSess);

    // ── Persist all answers for this step ────────────────────────────────
    if (updatedSess.userSurveyId) {
      this.api.saveAnswer({
        userSurveyId: updatedSess.userSurveyId,
        stepNumber: stepDef.stepNumber,
        answers: enriched,
      }).pipe(
        takeUntilDestroyed(this.destroyRef),
        catchError(err => { console.warn('Could not persist step answers:', err); return of(null); })
      ).subscribe();
    }

    const skipAi = stepDef.questions.every(q => q.type === 'feedback');
    if (skipAi) {
      this.advanceAfterStep(updatedSess);
      return;
    }

    // Show AI loading screen
    this.view.set('analyzing');

    const prevResults = this.sessionSvc.getPreviousResults(updatedSess, stepDef.stepNumber);

    this.api.analyzeStep({
      surveyType: def.surveyType,
      stepNumber: stepDef.stepNumber,
      totalSteps: def.steps.length,
      language: updatedSess.language,
      answers: enriched,
      previousResults: prevResults,
      userSurveyId: updatedSess.userSurveyId,
    }).pipe(
      takeUntilDestroyed(this.destroyRef),
      catchError(err => {
        console.error('AI analysis failed', err);
        // On error: continue without AI result
        return of({ stepNumber: stepDef.stepNumber, resultJson: '{}', outputFormat: 'short', tokensUsed: null, success: false, error: err.message });
      })
    ).subscribe(res => {
      const aiResult: AiStepResult = {
        step: res.stepNumber,
        resultJson: res.resultJson,
        outputFormat: res.outputFormat,
        tokensUsed: res.tokensUsed ?? undefined,
        analyzedAt: new Date().toISOString(),
      };
      updatedSess = this.sessionSvc.saveAiResult(updatedSess, aiResult);
      this.session.set(updatedSess);
      this.advanceAfterStep(updatedSess);
    });
  }

  private advanceAfterStep(sess: SurveySession): void {
    if (sess.currentStep === 'feedback') {
      this.view.set('feedback');
    } else if (sess.currentStep === 'done') {
      this.finalResult.set(this.sessionSvc.getLatestResult(sess));
      this.view.set('result');
    } else {
      this.currentStepNumber.set(sess.currentStep as number);
      this.view.set('step');
    }
  }

  onFeedbackComplete(fb: { rating: number | null; comment: string }): void {
    let sess = this.sessionSvc.completeFeedback(this.session()!);
    this.session.set(sess);
    const last = this.sessionSvc.getLatestResult(sess);
    this.finalResult.set(last);
    this.view.set('result');
  }

  onRestart(): void {
    if (this.survey()) {
      const def = this.survey()!;
      this.sessionSvc.clearSession(def.surveyType);
      const sess = this.sessionSvc.getOrCreateSession(
        def.surveyType, def.id, def.steps.length, this.locale.currentLang
      );
      this.session.set(sess);
      this.currentStepNumber.set(1);
      this.finalResult.set(null);
      this.view.set('intro');
    }
  }
}
