import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TranslationEntry } from '../models/question.model';

/**
 * Resolves localized text for survey questions and options.
 * Falls back from current language → "uk" → "en" → first available.
 */
@Injectable({ providedIn: 'root' })
export class SurveyLocaleService {
  constructor(private translate: TranslateService) {}

  get currentLang(): string {
    return this.translate.currentLang ?? this.translate.defaultLang ?? 'uk';
  }

  resolve(translations: TranslationEntry[]): string {
    if (!translations?.length) return '';
    const lang = this.currentLang;
    return (
      translations.find(t => t.languageCode === lang)?.text ??
      translations.find(t => t.languageCode === 'uk')?.text ??
      translations.find(t => t.languageCode === 'en')?.text ??
      translations[0]?.text ??
      ''
    );
  }
}
