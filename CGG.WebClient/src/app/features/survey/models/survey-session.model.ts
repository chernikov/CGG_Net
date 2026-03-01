/**
 * Survey session stored in localStorage under the key 'cgg_survey_session'.
 * The entire survey flow is managed client-side; the backend is called only
 * for AI analysis after each step.
 */

export interface StepAnswer {
  /** SurveyQuestion.Id (UUID) */
  questionId: string;
  /** Question text at time of answering – sent to AI for context */
  questionText: string;
  /**
   * For single/text/scale/rating/email/number → plain string value.
   * For multiple-choice → JSON array string e.g. '["A","B"]'
   */
  answer: string;
}

export interface CompletedStep {
  step: number;
  answers: StepAnswer[];
  completedAt: string; // ISO timestamp
}

export interface AiStepResult {
  step: number;
  /** Raw JSON string returned by the AI (short or full format) */
  resultJson: string;
  /** "short" | "full" */
  outputFormat: string;
  tokensUsed?: number;
  analyzedAt: string; // ISO timestamp
}

export type SurveyCurrentStep = number | 'feedback' | 'done';

export interface SurveySession {
  /** Unique session ID (UUID generated client-side) */
  sessionId: string;
  /** Survey type: "classic" | "gaming" | "ab-test" | etc. */
  surveyType: string;
  /** Survey definition ID (from API) */
  surveyId: string;
  /** UI language code */
  language: string;
  /** Current position in the survey */
  currentStep: SurveyCurrentStep;
  /** Total number of steps in the survey */
  totalSteps: number;
  /** Accumulates completed step answers */
  steps: CompletedStep[];
  /** Accumulates AI results per step */
  results: AiStepResult[];
  startedAt: string;
  updatedAt: string;
}

export function createEmptySession(
  surveyType: string,
  surveyId: string,
  totalSteps: number,
  language: string
): SurveySession {
  const now = new Date().toISOString();
  return {
    sessionId: crypto.randomUUID(),
    surveyType,
    surveyId,
    language,
    currentStep: 1,
    totalSteps,
    steps: [],
    results: [],
    startedAt: now,
    updatedAt: now,
  };
}
