/**
 * Question types matching CGG.Core.Enums.QuestionType on the backend.
 * These are the string values stored in the SurveyQuestion.QuestionType column.
 */
export type QuestionType =
  | 'single-choice'   // radio / button list (legacy: "select", "radio")
  | 'multiple-choice' // checkbox list (legacy: "multiselect")
  | 'text'            // short input (legacy: "input", "text")
  | 'textarea'        // multi-line text
  | 'scale'           // numeric range buttons 1–N (legacy: "number-buttons")
  | 'rating'          // star / emoji rating
  | 'feedback'        // special free-form feedback step (no AI analysis)
  | 'email'           // email input
  | 'number';         // plain numeric

/** Maps backend string values to our canonical QuestionType */
export function normalizeQuestionType(raw: string): QuestionType {
  const map: Record<string, QuestionType> = {
    'single-choice': 'single-choice',
    'select':        'single-choice',
    'radio':         'single-choice',
    'multiple-choice': 'multiple-choice',
    'multiselect':     'multiple-choice',
    'text':    'text',
    'input':   'text',
    'textarea': 'textarea',
    'scale':          'scale',
    'number-buttons': 'scale',
    'rating':   'rating',
    'feedback': 'feedback',
    'email':    'email',
    'number':   'number',
  };
  return map[raw?.toLowerCase()] ?? 'text';
}

/** A localized translation entry for a question or option */
export interface TranslationEntry {
  languageCode: string;
  text: string;
}

/** A selectable option inside a question */
export interface QuestionOption {
  id: string;
  value: string | null;
  sortOrder: number;
  translations: TranslationEntry[];
}

/** Condition controlling when a question is shown */
export interface VisibleIfCondition {
  field: string;      // purpose of the controlling question
  equals?: string;    // show if controlling answer === equals
  contains?: string;  // show if controlling answer (JSON array or string) contains value
}

/** A single survey question as returned by the API */
export interface SurveyQuestion {
  id: string;
  questionType: string;         // raw string from API
  type: QuestionType;           // normalized
  purpose: string | null;
  sortOrder: number;
  translations: TranslationEntry[];
  options: QuestionOption[];
  visibleIfJson?: string | null; // raw JSON from API
  visibleIf?: VisibleIfCondition; // parsed at load time
  // helpers resolved at load time
  min?: number;
  max?: number;
}

/** A group of questions belonging to one step */
export interface SurveyStepDef {
  id: string;
  stepNumber: number;
  isRequired: boolean;
  questions: SurveyQuestion[];
}

/** Full survey definition returned by GET /api/survey/item?name= */
export interface SurveyDef {
  id: string;
  surveyType: string;
  title: string;
  description: string | null;
  version: number;
  steps: SurveyStepDef[];
}
