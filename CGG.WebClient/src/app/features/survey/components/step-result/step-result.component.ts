import { Component, computed, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AiStepResult } from '../../models/survey-session.model';

interface ProfessionMatch {
  title: string;
  matchPercentage: number;
}

@Component({
  selector: 'app-step-result',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './step-result.component.html',
})
export class StepResultComponent {
  aiResult = input.required<AiStepResult>();
  stepNumber = input.required<number>();
  totalSteps = input.required<number>();
  continue = output<void>();

  matches = computed<ProfessionMatch[]>(() => {
    try {
      const parsed = JSON.parse(this.aiResult().resultJson ?? '{}');
      const list: ProfessionMatch[] = parsed.matches ?? [];
      return list
        .sort((a, b) => b.matchPercentage - a.matchPercentage)
        .slice(0, 5);
    } catch {
      return [];
    }
  });

  medal(index: number): string {
    return ['🥇', '🥈', '🥉'][index] ?? '▪️';
  }
}
