import {
  Component, Input, Output, EventEmitter, OnChanges, inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SurveyQuestion } from '../../models/question.model';
import { SurveyLocaleService } from '../../services/survey-locale.service';
import { SingleChoiceQuestionComponent } from '../question-types/single-choice/single-choice-question.component';
import { MultipleChoiceQuestionComponent } from '../question-types/multiple-choice/multiple-choice-question.component';
import { TextQuestionComponent } from '../question-types/text-question/text-question.component';
import { TextareaQuestionComponent } from '../question-types/textarea-question/textarea-question.component';
import { ScaleQuestionComponent } from '../question-types/scale-question/scale-question.component';
import { RatingQuestionComponent } from '../question-types/rating-question/rating-question.component';

/**
 * Dispatches a SurveyQuestion to the correct type-specific input component
 * and displays the question text above it.
 */
@Component({
  selector: 'app-question-renderer',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SingleChoiceQuestionComponent,
    MultipleChoiceQuestionComponent,
    TextQuestionComponent,
    TextareaQuestionComponent,
    ScaleQuestionComponent,
    RatingQuestionComponent,
  ],
  template: `
    <div class="bg-white rounded-2xl shadow-md p-6 md:p-8 flex flex-col gap-5">
      <!-- Question text -->
      <h3 class="text-lg md:text-xl font-semibold text-slate-800 leading-snug">
        {{ questionText }}
      </h3>

      <!-- Type-specific input -->
      @switch (question.type) {
        @case ('single-choice') {
          <app-single-choice-question
            [question]="question"
            [value]="value"
            (valueChange)="onChange($event)"
          />
        }
        @case ('multiple-choice') {
          <app-multiple-choice-question
            [question]="question"
            [value]="value"
            (valueChange)="onChange($event)"
          />
        }
        @case ('textarea') {
          <app-textarea-question
            [value]="value"
            [placeholder]="placeholder"
            (valueChange)="onChange($event)"
          />
        }
        @case ('scale') {
          <app-scale-question
            [value]="value"
            [min]="question.min ?? 1"
            [max]="question.max ?? 10"
            (valueChange)="onChange($event)"
          />
        }
        @case ('rating') {
          <app-rating-question
            [value]="value"
            (valueChange)="onChange($event)"
          />
        }
        @default {
          <!-- text, email, number -->
          <app-text-question
            [value]="value"
            [placeholder]="placeholder"
            (valueChange)="onChange($event)"
          />
        }
      }
    </div>
  `,
})
export class QuestionRendererComponent implements OnChanges {
  @Input() question!: SurveyQuestion;
  @Input() value: string = '';
  @Output() valueChange = new EventEmitter<string>();

  private locale = inject(SurveyLocaleService);
  questionText = '';
  placeholder = '';

  ngOnChanges(): void {
    if (this.question) {
      this.questionText = this.locale.resolve(this.question.translations);
      this.placeholder = this.locale.resolve(
        (this.question as any).placeholderTranslations ?? []
      );
    }
  }

  onChange(val: string): void {
    this.valueChange.emit(val);
  }
}
