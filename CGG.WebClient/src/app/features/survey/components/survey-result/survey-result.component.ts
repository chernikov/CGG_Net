import { Component, Input, Output, EventEmitter, OnChanges, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { AiStepResult } from '../../models/survey-session.model';

interface ProfessionMatch {
  title: string;
  matchPercentage: number;
  fitReasons?: string[];
  strongSkills?: string[];
  skillsToImprove?: string[];
  salaryRange?: { junior: string; mid: string; senior: string };
  education?: { offline: string[]; online: string[] };
  nextSteps?: string[];
  personalityInsights?: string[];
}

interface FullResult {
  matches: ProfessionMatch[];
  overallPersonalityProfile?: string;
}

@Component({
  selector: 'app-survey-result',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './survey-result.component.html',
})
export class SurveyResultComponent implements OnChanges {
  @Input() aiResult!: AiStepResult;
  @Output() restart = new EventEmitter<void>();
  @Output() leaveFeedback = new EventEmitter<void>();
  @Output() toDashboard = new EventEmitter<void>();

  result: FullResult | null = null;
  showConfirm = signal(false);

  confirmRestart(): void {
    this.showConfirm.set(false);
    this.restart.emit();
  }

  ngOnChanges(): void {
    if (this.aiResult?.resultJson) {
      try {
        this.result = JSON.parse(this.aiResult.resultJson);
      } catch {
        this.result = null;
      }
    }
  }
}
