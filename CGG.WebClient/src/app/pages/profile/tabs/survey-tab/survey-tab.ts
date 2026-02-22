import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-survey-tab',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './survey-tab.html',
  styleUrl: './survey-tab.scss',
})
export class SurveyTabComponent {
  hasResults = input<boolean>(false);
}
