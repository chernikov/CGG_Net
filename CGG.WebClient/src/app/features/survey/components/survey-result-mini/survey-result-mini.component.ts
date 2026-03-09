import { Component, computed, inject, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { SurveySession } from '../../models/survey-session.model';
import { SurveySessionService } from '../../services/survey-session.service';

@Component({
  selector: 'app-survey-result-mini',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './survey-result-mini.component.html',
  styleUrl: './survey-result-mini.component.scss',
})
export class SurveyResultMiniComponent {
  session = input.required<SurveySession>();
  surveyType = input.required<string>();
  retake = output<void>();
  viewResults = output<void>();

  private sessionSvc = inject(SurveySessionService);

  topMatches = computed(() =>
    this.sessionSvc.getTopMatches(this.session(), 3)
  );
}
