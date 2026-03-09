import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-survey-card',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './survey-card.component.html',
  styleUrl: './survey-card.component.scss',
})
export class SurveyCardComponent {
  emoji = input<string>('📋');
  titleKey = input.required<string>();
  descriptionKey = input.required<string>();
  estimatedMinutes = input<number>(10);
  start = output<void>();
}
