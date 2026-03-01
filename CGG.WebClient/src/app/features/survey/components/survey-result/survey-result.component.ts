import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
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
  imports: [CommonModule],
  template: `
    <div class="w-full max-w-3xl mx-auto px-4 py-8">
      <div class="text-center mb-8">
        <div class="text-5xl mb-4">🏆</div>
        <h2 class="text-2xl md:text-3xl font-bold text-slate-800 mb-2">Ваші результати</h2>
        @if (result?.overallPersonalityProfile) {
          <p class="text-slate-500 text-base leading-relaxed max-w-xl mx-auto">
            {{ result!.overallPersonalityProfile }}
          </p>
        }
      </div>

      <!-- Profession cards -->
      <div class="flex flex-col gap-5">
        @for (match of result?.matches ?? []; track match.title; let i = $index) {
          <div
            class="bg-white rounded-2xl shadow-md p-6 border-l-4 transition-shadow hover:shadow-lg"
            [class.border-yellow-400]="i === 0"
            [class.border-slate-300]="i > 0"
          >
            <div class="flex items-start justify-between gap-4 mb-4">
              <div class="flex items-center gap-3">
                @if (i === 0) {
                  <span class="text-2xl">🥇</span>
                } @else if (i === 1) {
                  <span class="text-2xl">🥈</span>
                } @else if (i === 2) {
                  <span class="text-2xl">🥉</span>
                } @else {
                  <span class="w-8 h-8 rounded-full bg-slate-100 flex items-center justify-center text-slate-600 font-bold text-sm">{{ i + 1 }}</span>
                }
                <h3 class="text-xl font-bold text-slate-800">{{ match.title }}</h3>
              </div>
              <!-- Match gauge -->
              <div class="shrink-0 text-right">
                <span class="text-2xl font-bold text-blue-600">{{ match.matchPercentage }}%</span>
                <div class="w-24 h-2 bg-slate-100 rounded-full mt-1">
                  <div class="h-full bg-blue-500 rounded-full" [style.width.%]="match.matchPercentage"></div>
                </div>
              </div>
            </div>

            <!-- Full format details -->
            @if (match.fitReasons?.length) {
              <details class="mt-3">
                <summary class="cursor-pointer text-sm font-semibold text-slate-600 hover:text-blue-600">
                  Чому це підходить
                </summary>
                <ul class="mt-2 pl-4 space-y-1">
                  @for (r of match.fitReasons; track r) {
                    <li class="text-sm text-slate-600 list-disc">{{ r }}</li>
                  }
                </ul>
              </details>
            }

            @if (match.strongSkills?.length) {
              <details class="mt-2">
                <summary class="cursor-pointer text-sm font-semibold text-slate-600 hover:text-blue-600">
                  Сильні сторони
                </summary>
                <div class="mt-2 flex flex-wrap gap-2">
                  @for (s of match.strongSkills; track s) {
                    <span class="px-3 py-1 bg-emerald-50 text-emerald-700 rounded-full text-xs font-medium">{{ s }}</span>
                  }
                </div>
              </details>
            }

            @if (match.skillsToImprove?.length) {
              <details class="mt-2">
                <summary class="cursor-pointer text-sm font-semibold text-slate-600 hover:text-blue-600">
                  Що розвинути
                </summary>
                <div class="mt-2 flex flex-wrap gap-2">
                  @for (s of match.skillsToImprove; track s) {
                    <span class="px-3 py-1 bg-amber-50 text-amber-700 rounded-full text-xs font-medium">{{ s }}</span>
                  }
                </div>
              </details>
            }

            @if (match.salaryRange) {
              <details class="mt-2">
                <summary class="cursor-pointer text-sm font-semibold text-slate-600 hover:text-blue-600">
                  💰 Зарплата
                </summary>
                <div class="mt-2 grid grid-cols-3 gap-2 text-center text-xs">
                  <div class="bg-slate-50 rounded-lg p-2">
                    <div class="text-slate-400 mb-1">Junior</div>
                    <div class="font-semibold text-slate-700">{{ match.salaryRange!.junior }}</div>
                  </div>
                  <div class="bg-slate-50 rounded-lg p-2">
                    <div class="text-slate-400 mb-1">Mid</div>
                    <div class="font-semibold text-slate-700">{{ match.salaryRange!.mid }}</div>
                  </div>
                  <div class="bg-slate-50 rounded-lg p-2">
                    <div class="text-slate-400 mb-1">Senior</div>
                    <div class="font-semibold text-slate-700">{{ match.salaryRange!.senior }}</div>
                  </div>
                </div>
              </details>
            }

            @if (match.nextSteps?.length) {
              <details class="mt-2">
                <summary class="cursor-pointer text-sm font-semibold text-slate-600 hover:text-blue-600">
                  🚀 Наступні кроки
                </summary>
                <ul class="mt-2 pl-4 space-y-1">
                  @for (step of match.nextSteps; track step) {
                    <li class="text-sm text-slate-600 list-disc">{{ step }}</li>
                  }
                </ul>
              </details>
            }
          </div>
        }
      </div>

      <!-- Actions -->
      <div class="flex gap-4 mt-10 justify-center">
        <button
          type="button"
          class="px-6 py-3 border-2 border-slate-200 rounded-xl text-slate-600 font-medium hover:bg-slate-50 transition"
          (click)="restart.emit()"
        >
          Пройти знову
        </button>
        <button
          type="button"
          class="px-6 py-3 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded-xl transition"
          (click)="toDashboard.emit()"
        >
          На головну →
        </button>
      </div>
    </div>
  `,
})
export class SurveyResultComponent implements OnChanges {
  @Input() aiResult!: AiStepResult;
  @Output() restart = new EventEmitter<void>();
  @Output() toDashboard = new EventEmitter<void>();

  result: FullResult | null = null;

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
