import { Component, Input, Output, EventEmitter, OnChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveyQuestion } from '../../../models/question.model';
import { SurveyLocaleService } from '../../../services/survey-locale.service';

@Component({
  selector: 'app-single-choice-question',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './single-choice-question.component.html',
})
export class SingleChoiceQuestionComponent implements OnChanges {
  @Input() question!: SurveyQuestion;
  @Input() value: string = '';
  @Output() valueChange = new EventEmitter<string>();

  private locale = inject(SurveyLocaleService);
  options: { value: string; label: string }[] = [];

  ngOnChanges(): void {
    this.options = (this.question?.options ?? [])
      .sort((a, b) => a.sortOrder - b.sortOrder)
      .map(o => ({
        value: o.value ?? '',
        label: this.locale.resolve(o.translations),
      }));
  }

  select(val: string): void {
    this.valueChange.emit(val);
  }
}
