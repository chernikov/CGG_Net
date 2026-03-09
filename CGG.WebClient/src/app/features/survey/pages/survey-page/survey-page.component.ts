import {
  Component, OnInit, inject, signal, DestroyRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { switchMap, catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';

import { SurveyApiService } from '../../services/survey-api.service';
import { SurveySessionService } from '../../services/survey-session.service';
import { SurveyLocaleService } from '../../services/survey-locale.service';
import { updateUserCredits } from '../../../../store/auth/auth.actions';

import { SurveyDef } from '../../models/question.model';
import { SurveySession, StepAnswer, AiStepResult } from '../../models/survey-session.model';

import { SurveyIntroComponent } from '../../components/survey-intro/survey-intro.component';
import { SurveyStepComponent } from '../../components/survey-step/survey-step.component';
import { FeedbackStepComponent } from '../../components/feedback-step/feedback-step.component';
import { SurveyResultComponent } from '../../components/survey-result/survey-result.component';
import { StepResultComponent } from '../../components/step-result/step-result.component';
import { BackHeaderComponent } from '../../../../shared/components/back-header/back-header.component';

type PageView = 'loading' | 'error' | 'intro' | 'step' | 'analyzing' | 'step-result' | 'ai-error' | 'feedback' | 'result';

@Component({
  selector: 'app-survey-page',
  standalone: true,
  imports: [
    CommonModule,
    SurveyIntroComponent,
    SurveyStepComponent,
    FeedbackStepComponent,
    SurveyResultComponent,
    StepResultComponent,
    BackHeaderComponent,
  ],
  templateUrl: './survey-page.component.html',
})
export class SurveyPageComponent implements OnInit {
  private route = inject(ActivatedRoute);
  readonly router = inject(Router);
  private store = inject(Store);
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
  stepResult = signal<AiStepResult | null>(null);
  aiErrorMsg = signal('');

  /** Where the back button navigates — child profile if childId is in URL, else dashboard. */
  backUrl = signal('/dashboard');
  backLabel = signal('На головну');

  // ─── Lifecycle ────────────────────────────────────────────────────────────

  ngOnInit(): void {
    const childId = this.route.snapshot.queryParamMap.get('childId');
    if (childId) {
      this.backUrl.set(`/child/${childId}`);
      this.backLabel.set('На профіль');
    }
    this.route.queryParamMap
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        switchMap(params => {
          const name = params.get('name') ?? 'classic';
          const isRetake = params.get('retake') === 'true';
          return this.api.getSurveyByName(name).pipe(
            catchError(err => {
              // If the survey definition can't be loaded but we have existing results,
              // show those results instead of the error screen.
              const existingSession = this.sessionSvc.getSession(name);
              if (existingSession && existingSession.results.length > 0) {
                const last = this.sessionSvc.getLatestResult(existingSession);
                this.session.set(existingSession);
                this.finalResult.set(last);
                this.view.set('result');
              } else {
                this.errorMsg.set(err?.error?.message ?? err?.message ?? 'Невідома помилка');
                this.view.set('error');
              }
              return of(null);
            }),
            map(def => def ? { def, isRetake } : null)
          );
        })
      )
      .subscribe(payload => {
        if (!payload) return;
        const { def, isRetake } = payload;
        this.survey.set(def);

        // If retake was requested, clear the old session now that we know the survey exists
        if (isRetake) {
          this.sessionSvc.clearSession(def.surveyType);
        }

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
          // Show result first — user can leave feedback from result screen
          const last = this.sessionSvc.getLatestResult(sess);
          this.finalResult.set(last);
          this.view.set('result');
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

  /** Persists autofill answers to localStorage without completing the step. */
  onAutofillApplied(answers: StepAnswer[]): void {
    const sess = this.session();
    if (!sess) return;
    const stepDef = this.currentStepDef()!;
    const enriched = answers
      .filter(a => a.answer?.trim())
      .map(a => {
        const q = stepDef.questions.find(q => q.id === a.questionId);
        return { ...a, questionText: q ? this.locale.resolve(q.translations) : '' };
      });
    const updated = this.sessionSvc.savePartialStepAnswers(sess, stepDef.stepNumber, enriched);
    this.session.set(updated);
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

    const analyzePayload = {
      surveyType: def.surveyType,
      stepNumber: stepDef.stepNumber,
      totalSteps: def.steps.length,
      language: updatedSess.language,
      answers: enriched,
      previousResults: prevResults,
      userSurveyId: updatedSess.userSurveyId,
    };

    console.log('[AI] POST /api/survey/analyze-step', {
      surveyType: def.surveyType,
      stepNumber: stepDef.stepNumber,
      totalSteps: def.steps.length,
      answersCount: enriched.length,
    });

    this.api.analyzeStep(analyzePayload).pipe(
      takeUntilDestroyed(this.destroyRef),
      catchError(err => {
        console.error('[AI] Error step', stepDef.stepNumber, err);
        if (err?.status === 402) {
          const required = err.error?.required ?? '?';
          const available = err.error?.available ?? '?';
          this.aiErrorMsg.set(`Недостатньо кредитів. Необхідно: ${required}, доступно: ${available}`);
        } else {
          this.aiErrorMsg.set(err?.error?.message ?? err?.message ?? 'Невідома помилка');
        }
        this.view.set('ai-error');
        return of(null);
      })
    ).subscribe(res => {
      if (!res) return;

      let parsed: unknown = null;
      try { parsed = JSON.parse(res.resultJson ?? '{}'); } catch { /* ignore */ }

      console.log('[AI] Response step', stepDef.stepNumber, {
        success: res.success !== false,
        outputFormat: res.outputFormat,
        tokensUsed: res.tokensUsed,
        resultJson: parsed,
      });

      if (res.creditsRemaining != null) {
        this.store.dispatch(updateUserCredits({ credits: res.creditsRemaining }));
      }

      if (res.success === false) {
        this.aiErrorMsg.set(res.error ?? 'AI аналіз не вдався');
        this.view.set('ai-error');
        return;
      }

      const aiResult: AiStepResult = {
        step: res.stepNumber,
        resultJson: res.resultJson,
        outputFormat: res.outputFormat,
        tokensUsed: res.tokensUsed ?? undefined,
        analyzedAt: new Date().toISOString(),
      };
      updatedSess = this.sessionSvc.saveAiResult(updatedSess, aiResult);
      this.session.set(updatedSess);

      const isFinalStep = stepDef.stepNumber >= def.steps.length;
      if (!isFinalStep) {
        this.stepResult.set(aiResult);
        this.view.set('step-result');
      } else {
        this.advanceAfterStep(updatedSess);
      }
    });
  }

  onRetryAi(): void {
    const sess = this.session()!;
    const stepNum = typeof sess.currentStep === 'number' ? sess.currentStep - 1 : 1;
    const answers = this.sessionSvc.getStepAnswers(sess, stepNum);
    this.currentStepNumber.set(stepNum);
    this.onStepComplete(answers);
  }

  onStepResultContinue(): void {
    this.stepResult.set(null);
    this.advanceAfterStep(this.session()!);
  }

  private advanceAfterStep(sess: SurveySession): void {
    if (sess.currentStep === 'feedback' || sess.currentStep === 'done') {
      this.finalResult.set(this.sessionSvc.getLatestResult(sess));
      this.view.set('result');

    } else {
      this.currentStepNumber.set(sess.currentStep as number);
      this.view.set('step');
    }
  }

  onLeaveFeedback(): void {
    this.view.set('feedback');
  }

  onFeedbackComplete(fb: { rating: number; comment: string }): void {
    const sess = this.sessionSvc.completeFeedback(this.session()!);
    this.session.set(sess);
    this.view.set('result');

    if (sess.userSurveyId) {
      this.api.saveFeedback({
        userSurveyId: sess.userSurveyId,
        rating: fb.rating,
        comment: fb.comment,
      }).pipe(
        takeUntilDestroyed(this.destroyRef),
        catchError(err => { console.warn('Could not save feedback:', err); return of(null); })
      ).subscribe();
    }
  }

  onRestart(): void {
    const def = this.survey();
    const sess = this.session();
    if (def) {
      // Normal case: survey definition is loaded — clear and restart
      this.sessionSvc.clearSession(def.surveyType);
      const newSess = this.sessionSvc.getOrCreateSession(
        def.surveyType, def.id, def.steps.length, this.locale.currentLang
      );
      this.session.set(newSess);
      this.currentStepNumber.set(1);
      this.finalResult.set(null);
      this.view.set('intro');
    } else if (sess) {
      // Fallback case: survey definition failed to load (404) but we showed cached results.
      // Navigate to the survey page with retake=true so it tries to reload the definition.
      // If it 404s again the cached results will be shown; if it succeeds the retake proceeds.
      this.router.navigate(['/survey'], { queryParams: { name: sess.surveyType, retake: 'true' } });
    }
  }
}
