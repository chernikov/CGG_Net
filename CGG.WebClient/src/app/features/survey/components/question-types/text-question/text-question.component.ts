import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-text-question',
  standalone: true,
  imports: [FormsModule],
  template: `
    <input
      type="text"
      class="w-full px-4 py-3 rounded-xl border-2 border-slate-300 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 outline-none text-base text-slate-800 bg-white transition"
      [placeholder]="placeholder"
      [(ngModel)]="value"
      (ngModelChange)="valueChange.emit($event)"
    />
  `,
})
export class TextQuestionComponent {
  @Input() value: string = '';
  @Input() placeholder: string = '';
  @Output() valueChange = new EventEmitter<string>();
}
