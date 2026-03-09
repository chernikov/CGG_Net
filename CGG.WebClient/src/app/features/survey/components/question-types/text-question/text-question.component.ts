import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-text-question',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './text-question.component.html',
})
export class TextQuestionComponent {
  @Input() value: string = '';
  @Input() placeholder: string = '';
  @Output() valueChange = new EventEmitter<string>();
}
