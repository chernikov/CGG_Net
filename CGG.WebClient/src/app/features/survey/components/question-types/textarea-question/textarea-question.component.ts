import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-textarea-question',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './textarea-question.component.html',
})
export class TextareaQuestionComponent {
  @Input() value: string = '';
  @Input() placeholder: string = '';
  @Input() maxLength: number = 400;
  @Output() valueChange = new EventEmitter<string>();
}
