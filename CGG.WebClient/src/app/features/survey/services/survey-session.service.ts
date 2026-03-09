import { Injectable } from '@angular/core';
import {
  SurveySession,
  CompletedStep,
  AiStepResult,
  StepAnswer,
  SurveyCurrentStep,
  createEmptySession,
} from '../models/survey-session.model';

/** Key for multi-survey storage: { [surveyType]: SurveySession } */
const SESSIONS_KEY = 'cgg_survey_sessions';
/** Legacy single-session key kept for backward compat migration */
const SESSION_KEY = 'cgg_survey_session';

/**
 * Manages survey session lifecycle in localStorage.
 * Stores all survey type sessions as a map: { [surveyType]: SurveySession }.
 */
@Injectable({ providedIn: 'root' })
export class SurveySessionService {

  // ─── Multi-session storage ────────────────────────────────────────────────

  private loadAll(): Record<string, SurveySession> {
    try {
      const raw = localStorage.getItem(SESSIONS_KEY);
      if (raw) return JSON.parse(raw) as Record<string, SurveySession>;
      // Migrate legacy single session if present
      const legacy = localStorage.getItem(SESSION_KEY);
      if (legacy) {
        const s: SurveySession = JSON.parse(legacy);
        const map: Record<string, SurveySession> = { [s.surveyType]: s };
        localStorage.setItem(SESSIONS_KEY, JSON.stringify(map));
        localStorage.removeItem(SESSION_KEY);
        return map;
      }
    } catch { /* ignore */ }
    return {};
  }

  private saveAll(map: Record<string, SurveySession>): void {
    localStorage.setItem(SESSIONS_KEY, JSON.stringify(map));
  }

  getAllSessions(): SurveySession[] {
    return Object.values(this.loadAll());
  }

  // ─── Session access ───────────────────────────────────────────────────────

  getSession(surveyType: string): SurveySession | null {
    const session = this.loadAll()[surveyType] ?? null;
    if (!session) return null;
    // Expire after 24 h
    if (Date.now() - new Date(session.startedAt).getTime() > 24 * 60 * 60 * 1000) {
      this.clearSession(surveyType);
      return null;
    }
    return session;
  }

  getOrCreateSession(
    surveyType: string,
    surveyId: string,
    totalSteps: number,
    language: string
  ): SurveySession {
    return (
      this.getSession(surveyType) ??
      this.saveSession(createEmptySession(surveyType, surveyId, totalSteps, language))
    );
  }

  saveSession(session: SurveySession): SurveySession {
    session.updatedAt = new Date().toISOString();
    const map = this.loadAll();
    map[session.surveyType] = session;
    this.saveAll(map);
    return session;
  }

  clearSession(surveyType: string): void {
    const map = this.loadAll();
    delete map[surveyType];
    this.saveAll(map);
  }

  clearAllSessions(): void {
    localStorage.removeItem(SESSIONS_KEY);
  }

  /** True if a session exists with currentStep === 'done' */
  hasCompletedSession(surveyType: string): boolean {
    const s = this.getSession(surveyType);
    return s?.currentStep === 'done';
  }

  /** True if a session exists and has AI results (even incomplete survey) */
  hasAnyResults(surveyType: string): boolean {
    const s = this.getSession(surveyType);
    return (s?.results?.length ?? 0) > 0;
  }

  // ─── Step navigation ──────────────────────────────────────────────────────

  getCurrentStep(surveyType: string): SurveyCurrentStep {
    return this.getSession(surveyType)?.currentStep ?? 1;
  }

  completeStep(
    session: SurveySession,
    stepNumber: number,
    answers: StepAnswer[]
  ): SurveySession {
    const completedStep: CompletedStep = {
      step: stepNumber,
      answers,
      completedAt: new Date().toISOString(),
    };
    const idx = session.steps.findIndex(s => s.step === stepNumber);
    if (idx >= 0) session.steps[idx] = completedStep;
    else session.steps.push(completedStep);

    session.currentStep = stepNumber >= session.totalSteps ? 'feedback' : stepNumber + 1;
    return this.saveSession(session);
  }

  completeFeedback(session: SurveySession): SurveySession {
    session.currentStep = 'done';
    return this.saveSession(session);
  }

  // ─── AI results ───────────────────────────────────────────────────────────

  saveAiResult(session: SurveySession, result: AiStepResult): SurveySession {
    const entry: AiStepResult = { ...result, analyzedAt: new Date().toISOString() };
    const idx = session.results.findIndex(r => r.step === result.step);
    if (idx >= 0) session.results[idx] = entry;
    else session.results.push(entry);
    return this.saveSession(session);
  }

  getPreviousResults(session: SurveySession, beforeStep: number) {
    return session.results
      .filter(r => r.step < beforeStep)
      .map(r => ({ step: r.step, resultJson: r.resultJson }));
  }

  // ─── Utility ─────────────────────────────────────────────────────────────

  getStepAnswers(session: SurveySession, stepNumber: number): StepAnswer[] {
    return session.steps.find(s => s.step === stepNumber)?.answers ?? [];
  }

  getLatestResult(session: SurveySession): AiStepResult | null {
    if (!session.results.length) return null;
    return [...session.results].sort((a, b) => b.step - a.step)[0];
  }

  /** Saves partial answers for a step without marking it complete or advancing currentStep. */
  savePartialStepAnswers(session: SurveySession, stepNumber: number, answers: StepAnswer[]): SurveySession {
    const existing = session.steps.find(s => s.step === stepNumber);
    if (existing) {
      existing.answers = answers;
    } else {
      session.steps.push({ step: stepNumber, answers, completedAt: new Date().toISOString() });
    }
    return this.saveSession(session);
  }

  /** Returns top-N profession matches from the most recent full-format AI result. */
  getTopMatches(session: SurveySession, top = 3): { title: string; matchPercentage: number }[] {
    // Prefer full-format result (last step), fall back to any result
    const resultEntry =
      session.results.find(r => r.outputFormat === 'full') ??
      this.getLatestResult(session);
    if (!resultEntry) return [];
    try {
      const parsed = JSON.parse(resultEntry.resultJson ?? '{}');
      const matches: { title: string; matchPercentage: number }[] = parsed.matches ?? [];
      return matches
        .sort((a, b) => b.matchPercentage - a.matchPercentage)
        .slice(0, top);
    } catch {
      return [];
    }
  }
}
