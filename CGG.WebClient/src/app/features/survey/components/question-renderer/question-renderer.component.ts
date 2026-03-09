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
  templateUrl: './question-renderer.component.html',
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
