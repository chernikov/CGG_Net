import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { SurveyDef } from '../../models/question.model';

const SURVEY_TYPE_TO_KEY: Record<string, string> = {
  'ab-test':              'abTest',
  'parent':               'parent',
  'parent-child-talents': 'parentChildTalents',
};

@Component({
  selector: 'app-survey-intro',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './survey-intro.component.html',
})
export class SurveyIntroComponent {
  @Input() survey!: SurveyDef;
  @Output() start = new EventEmitter<void>();

  get titleKey(): string {
    const k = SURVEY_TYPE_TO_KEY[this.survey?.surveyType] ?? 'abTest';
    return `surveys.${k}.title`;
  }

  get descriptionKey(): string {
    const k = SURVEY_TYPE_TO_KEY[this.survey?.surveyType] ?? 'abTest';
    return `surveys.${k}.description`;
  }
}
