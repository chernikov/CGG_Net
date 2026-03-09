import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { SurveyDef, SurveyStepDef, SurveyQuestion, VisibleIfCondition, normalizeQuestionType } from '../models/question.model';
import { StepAnswer } from '../models/survey-session.model';

// ─── API response shapes ────────────────────────────────────────────────────

export interface AnalyzeStepRequest {
  surveyType: string;
  stepNumber: number;
  totalSteps: number;
  language: string;
  answers: StepAnswer[];
  previousResults: { step: number; resultJson: string }[];
  userSurveyId?: string | null;
}

export interface AnalyzeStepResponse {
  stepNumber: number;
  resultJson: string;
  outputFormat: string;
  tokensUsed: number | null;
  success: boolean;
  error: string | null;
}

export interface SaveStepRequest {
  userSurveyId: string;
  stepNumber: number;
  answers: StepAnswer[];
}

export interface SaveStepResponse {
  success: boolean;
  error: string | null;
}

export interface SaveFeedbackRequest {
  userSurveyId: string;
  rating: number;
  comment: string;
}

export interface SaveFeedbackResponse {
  success: boolean;
  error: string | null;
}

export interface StartSurveyRequest {
  surveyId: string;
  surveyType: string;
  language: string;
}

export interface StartSurveyResponse {
  userSurveyId: string;
  success: boolean;
  error: string | null;
}

// ─── Service ────────────────────────────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class SurveyApiService {
  private readonly api = environment.apiUrl;

  constructor(private http: HttpClient) {}

  /**
   * Load a survey definition by its type name (e.g. "classic").
   * Normalises question types and resolves scale min/max from option values.
   */
  getSurveyByName(name: string): Observable<SurveyDef> {
    return this.http
      .get<SurveyDef>(`${this.api}/survey/item`, { params: { name } })
      .pipe(map(survey => this.normalizeSurvey(survey)));
  }

  /**
   * Send a completed step's answers to the backend AI analysis endpoint.
   * Returns the structured AI result.
   */
  analyzeStep(request: AnalyzeStepRequest): Observable<AnalyzeStepResponse> {
    return this.http.post<AnalyzeStepResponse>(
      `${this.api}/survey/analyze-step`,
      request
    );
  }

  /**
   * Start a new survey pass — marks previous ones of the same type as Outdated.
   * Returns the UserSurveyId to pass to saveAnswer.
   */
  startSurvey(request: StartSurveyRequest): Observable<StartSurveyResponse> {
    return this.http.post<StartSurveyResponse>(`${this.api}/survey/start`, request);
  }

  /**
   * Idempotent upsert of answers for a step. Safe to call on every "Наступне" click.
   */
  saveAnswer(request: SaveStepRequest): Observable<SaveStepResponse> {
    return this.http.post<SaveStepResponse>(`${this.api}/survey/save-answer`, request);
  }

  /**
   * Save user feedback (rating + comment) for a completed survey.
   */
  saveFeedback(request: SaveFeedbackRequest): Observable<SaveFeedbackResponse> {
    return this.http.post<SaveFeedbackResponse>(`${this.api}/survey/feedback`, request);
  }

  // ─── Normalization helpers ────────────────────────────────────────────────

  private normalizeSurvey(raw: SurveyDef): SurveyDef {
    return {
      ...raw,
      steps: raw.steps.map(step => this.normalizeStep(step)),
    };
  }

  private normalizeStep(step: SurveyStepDef): SurveyStepDef {
    return {
      ...step,
      questions: step.questions.map(q => this.normalizeQuestion(q)),
    };
  }

  private normalizeQuestion(q: SurveyQuestion): SurveyQuestion {
    const type = normalizeQuestionType(q.questionType);
    let min: number | undefined;
    let max: number | undefined;

    // For scale questions, try to infer min/max from option values
    if (type === 'scale' && q.options?.length) {
      const nums = q.options
        .map(o => parseInt(o.value ?? '', 10))
        .filter(n => !isNaN(n));
      if (nums.length) {
        min = Math.min(...nums);
        max = Math.max(...nums);
      }
    }
    if (min === undefined) min = 1;
    if (max === undefined && type === 'scale') max = 10;

    let visibleIf: VisibleIfCondition | undefined;
    if (q.visibleIfJson) {
      try { visibleIf = JSON.parse(q.visibleIfJson) as VisibleIfCondition; } catch { /* ignore */ }
    }

    return { ...q, type, min, max, visibleIf };
  }
}
