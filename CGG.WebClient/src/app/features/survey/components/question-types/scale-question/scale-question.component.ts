import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-scale-question',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="w-full">
      <div class="flex flex-wrap gap-2 justify-center">
        @for (n of buttons; track n) {
          <button
            type="button"
            class="w-12 h-12 rounded-xl border-2 text-base font-bold transition-all duration-200"
            [class.border-blue-600]="selected === n"
            [class.bg-blue-600]="selected === n"
            [class.text-white]="selected === n"
            [class.border-slate-200]="selected !== n"
            [class.bg-white]="selected !== n"
            [class.text-slate-700]="selected !== n"
            [class.hover:bg-slate-100]="selected !== n"
            (click)="select(n)"
          >{{ n }}</button>
        }
      </div>
      <div class="flex justify-between text-xs text-slate-400 mt-2 px-1">
        <span>{{ min }}</span>
        <span>{{ max }}</span>
      </div>
    </div>
  `,
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
