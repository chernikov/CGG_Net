import { Component, Input, Output, EventEmitter, OnChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveyQuestion } from '../../../models/question.model';
import { SurveyLocaleService } from '../../../services/survey-locale.service';

@Component({
  selector: 'app-multiple-choice-question',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './multiple-choice-question.component.html',
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
