import { Component, Input, Output, EventEmitter, OnChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveyQuestion } from '../../../models/question.model';
import { SurveyLocaleService } from '../../../services/survey-locale.service';

@Component({
  selector: 'app-single-choice-question',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="flex flex-col gap-3 w-full">
      @for (option of options; track option.value) {
        <button
          type="button"
          class="w-full text-left px-5 py-4 rounded-2xl border-2 text-base font-medium transition-all duration-200"
          [class.border-blue-600]="value === option.value"
          [class.bg-blue-50]="value === option.value"
          [class.text-blue-900]="value === option.value"
          [class.border-slate-200]="value !== option.value"
          [class.bg-white]="value !== option.value"
          [class.text-slate-700]="value !== option.value"
          [class.hover:bg-slate-50]="value !== option.value"
          (click)="select(option.value)"
        >
          <span class="mr-2 inline-block w-5 h-5 rounded-full border-2 align-middle"
            [class.border-blue-600]="value === option.value"
            [class.bg-blue-600]="value === option.value"
            [class.border-slate-300]="value !== option.value"
          ></span>
          {{ option.label }}
        </button>
      }
    </div>
  `,
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
