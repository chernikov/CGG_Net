import { Component, computed, inject, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { SurveySession } from '../../models/survey-session.model';
import { SurveySessionService } from '../../services/survey-session.service';

@Component({
  selector: 'app-survey-result-mini',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  template: `
    <div class="result-mini">
      <h4 class="result-mini__heading">{{ 'surveys.topMatches' | translate }}</h4>

      <ul class="result-mini__list">
        @for (match of topMatches(); track match.title) {
          <li class="result-mini__item">
            <span class="result-mini__label">{{ match.title }}</span>
            <div class="result-mini__bar-wrap">
              <div class="result-mini__bar" [style.width.%]="match.matchPercentage"></div>
            </div>
            <span class="result-mini__pct">{{ match.matchPercentage }}%</span>
          </li>
        }
        @empty {
          <li class="result-mini__empty">{{ 'surveys.noResults' | translate }}</li>
        }
      </ul>

      <div class="result-mini__actions">
        <button class="result-mini__btn result-mini__btn--secondary" (click)="retake.emit()">
          {{ 'surveys.retake' | translate }}
        </button>
        <button class="result-mini__btn result-mini__btn--primary" (click)="viewResults.emit()">
          {{ 'surveys.viewResults' | translate }}
        </button>
      </div>
    </div>
  `,
  styles: [`
    .result-mini {
      background: #fff;
      border: 1px solid #e5e7eb;
      border-radius: 12px;
      padding: 1.25rem 1.5rem;
      box-shadow: 0 1px 3px rgba(0,0,0,.06);
    }
    .result-mini__heading {
      font-size: .9375rem;
      font-weight: 600;
      color: #111827;
      margin: 0 0 1rem;
    }
    .result-mini__list {
      list-style: none;
      margin: 0 0 1rem;
      padding: 0;
      display: flex;
      flex-direction: column;
      gap: .6rem;
    }
    .result-mini__item {
      display: grid;
      grid-template-columns: 1fr auto auto;
      align-items: center;
      gap: .5rem;
    }
    .result-mini__label {
      font-size: .875rem;
      color: #374151;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
    .result-mini__bar-wrap {
      width: 100px;
      height: 6px;
      background: #e5e7eb;
      border-radius: 99px;
      overflow: hidden;
    }
    .result-mini__bar {
      height: 100%;
      background: #2563eb;
      border-radius: 99px;
      transition: width .4s ease;
    }
    .result-mini__pct {
      font-size: .75rem;
      color: #6b7280;
      min-width: 32px;
      text-align: right;
    }
    .result-mini__empty {
      font-size: .875rem;
      color: #9ca3af;
    }
    .result-mini__actions {
      display: flex;
      gap: .75rem;
      flex-wrap: wrap;
    }
    .result-mini__btn {
      border: none;
      border-radius: 8px;
      padding: .5rem 1rem;
      font-size: .875rem;
      font-weight: 500;
      cursor: pointer;
      transition: background .15s;

      &--primary {
        background: #2563eb;
        color: #fff;
        &:hover { background: #1d4ed8; }
      }
      &--secondary {
        background: #f3f4f6;
        color: #374151;
        &:hover { background: #e5e7eb; }
      }
    }
  `],
})
export class SurveyResultMiniComponent {
  session = input.required<SurveySession>();
  surveyType = input.required<string>();
  retake = output<void>();
  viewResults = output<void>();

  private sessionSvc = inject(SurveySessionService);

  topMatches = computed(() =>
    this.sessionSvc.getTopMatches(this.session(), 3)
  );
}
