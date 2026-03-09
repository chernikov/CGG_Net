import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-scale-question',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './scale-question.component.html',
})
export class ScaleQuestionComponent implements OnChanges {
  @Input() value: string = '';
  @Input() min: number = 1;
  @Input() max: number = 10;
  @Output() valueChange = new EventEmitter<string>();

  buttons: number[] = [];
  selected: number | null = null;

  ngOnChanges(): void {
    this.buttons = Array.from({ length: this.max - this.min + 1 }, (_, i) => this.min + i);
    this.selected = this.value ? parseInt(this.value, 10) : null;
  }

  select(n: number): void {
    this.selected = n;
    this.valueChange.emit(String(n));
  }
}
