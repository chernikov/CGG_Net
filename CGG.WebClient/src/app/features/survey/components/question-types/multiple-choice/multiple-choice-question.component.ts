import { Component, Input, Output, EventEmitter, OnChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveyQuestion } from '../../../models/question.model';
import { SurveyLocaleService } from '../../../services/survey-locale.service';

@Component({
  selector: 'app-multiple-choice-question',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="flex flex-col gap-3 w-full">
      @for (option of options; track option.value) {
        <button
          type="button"
          class="w-full text-left px-5 py-4 rounded-2xl border-2 text-base font-medium transition-all duration-200 flex items-center gap-3"
          [class.border-emerald-500]="isSelected(option.value)"
          [class.bg-emerald-50]="isSelected(option.value)"
          [class.text-emerald-900]="isSelected(option.value)"
          [class.border-slate-200]="!isSelected(option.value)"
          [class.bg-white]="!isSelected(option.value)"
          [class.text-slate-700]="!isSelected(option.value)"
          (click)="toggle(option.value)"
        >
          <span class="inline-flex items-center justify-center w-5 h-5 rounded border-2 shrink-0"
            [class.border-emerald-500]="isSelected(option.value)"
            [class.bg-emerald-500]="isSelected(option.value)"
            [class.border-slate-300]="!isSelected(option.value)"
          >
            @if (isSelected(option.value)) {
              <svg class="w-3 h-3 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="3">
                <path stroke-linecap="round" stroke-linejoin="round" d="M5 13l4 4L19 7"/>
              </svg>
            }
          </span>
          {{ option.label }}
        </button>
      }
    </div>
  `,
})
export class MultipleChoiceQuestionComponent implements OnChanges {
  @Input() question!: SurveyQuestion;
  /** JSON array string: '["A","B"]' */
  @Input() value: string = '[]';
  @Output() valueChange = new EventEmitter<string>();

  private locale = inject(SurveyLocaleService);
  options: { value: string; label: string }[] = [];
  private selected: Set<string> = new Set();

  ngOnChanges(): void {
    this.options = (this.question?.options ?? [])
      .sort((a, b) => a.sortOrder - b.sortOrder)
      .map(o => ({
        value: o.value ?? '',
        label: this.locale.resolve(o.translations),
      }));
    try {
      this.selected = new Set(JSON.parse(this.value || '[]'));
    } catch {
      this.selected = new Set();
    }
  }

  isSelected(val: string): boolean {
    return this.selected.has(val);
  }

  toggle(val: string): void {
    if (this.selected.has(val)) {
      this.selected.delete(val);
    } else {
      this.selected.add(val);
    }
    this.valueChange.emit(JSON.stringify([...this.selected]));
  }
}
