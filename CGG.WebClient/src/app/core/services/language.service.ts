import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

export type AppLanguage = 'uk' | 'en';

const STORAGE_KEY = 'app_lang';
const SUPPORTED: AppLanguage[] = ['uk', 'en'];

@Injectable({ providedIn: 'root' })
export class LanguageService {
  constructor(private translate: TranslateService) {
    const saved = localStorage.getItem(STORAGE_KEY) as AppLanguage | null;
    const lang = saved && SUPPORTED.includes(saved) ? saved : 'uk';
    this.translate.addLangs(SUPPORTED);
    this.translate.setDefaultLang('uk').subscribe();
    this.translate.use(lang).subscribe();
  }

  get current(): AppLanguage {
    return this.translate.currentLang as AppLanguage;
  }

  get supported(): AppLanguage[] {
    return SUPPORTED;
  }

  use(lang: AppLanguage): void {
    this.translate.use(lang).subscribe();
    localStorage.setItem(STORAGE_KEY, lang);
  }
}
