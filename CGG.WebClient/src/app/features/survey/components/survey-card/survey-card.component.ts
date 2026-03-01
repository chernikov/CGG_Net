import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-survey-card',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  template: `
    <div class="survey-card">
      <div class="survey-card__emoji">{{ emoji() }}</div>
      <div class="survey-card__body">
        <h3 class="survey-card__title">{{ titleKey() | translate }}</h3>
        <p class="survey-card__desc">{{ descriptionKey() | translate }}</p>
        <span class="survey-card__time">
          ⏱ {{ estimatedMinutes() }} {{ 'surveys.minutes' | translate }}
        </span>
      </div>
      <button class="survey-card__btn" (click)="start.emit()">
        {{ 'surveys.start' | translate }}
      </button>
    </div>
  `,
  styles: [`
    .survey-card {
      display: flex;
      align-items: center;
      gap: 1rem;
      background: #fff;
      border: 1px solid #e5e7eb;
      border-radius: 12px;
      padding: 1.25rem 1.5rem;
      box-shadow: 0 1px 3px rgba(0,0,0,.06);
      transition: box-shadow .2s;
      max-width: 900px;
      width: 100%;
      margin: 0 auto;

      &:hover { box-shadow: 0 4px 12px rgba(0,0,0,.1); }
    }
    .survey-card__emoji {
      font-size: 2.5rem;
      flex-shrink: 0;
      line-height: 1;
    }
    .survey-card__body {
      flex: 1;
      min-width: 0;
    }
    .survey-card__title {
      font-size: 1rem;
      font-weight: 600;
      color: #111827;
      margin: 0 0 .25rem;
    }
    .survey-card__desc {
      font-size: .875rem;
      color: #6b7280;
      margin: 0 0 .3rem;
      line-height: 1.4;
    }
    .survey-card__time {
      font-size: .75rem;
      color: #9ca3af;
    }
    .survey-card__btn {
      flex-shrink: 0;
      background: #2563eb;
      color: #fff;
      border: none;
      border-radius: 8px;
      padding: .6rem 1.25rem;
      font-size: .875rem;
      font-weight: 500;
      cursor: pointer;
      transition: background .15s;
      white-space: nowrap;

      &:hover { background: #1d4ed8; }
    }

    @media (max-width: 480px) {
      .survey-card {
        flex-direction: column;
        align-items: flex-start;
      }
      .survey-card__btn { width: 100%; text-align: center; }
    }
  `],
})
export class SurveyCardComponent {
  emoji = input<string>('📋');
  titleKey = input.required<string>();
  descriptionKey = input.required<string>();
  estimatedMinutes = input<number>(10);
  start = output<void>();
}
