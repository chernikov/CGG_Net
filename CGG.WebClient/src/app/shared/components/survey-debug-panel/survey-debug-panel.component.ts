import {
  Component,
  DestroyRef,
  HostListener,
  OnInit,
  inject,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { interval } from 'rxjs';
import { SurveySessionService } from '../../../features/survey/services/survey-session.service';
import { SurveySession, AiStepResult } from '../../../features/survey/models/survey-session.model';

type DebugTab = 'overview' | 'raw' | 'autofill';

export interface AutofillProfileEntry {
  id: string;
  slug: string;
  icon: string;
  nameUk: string;
  nameEn: string;
  answers: Record<string, unknown>;
  sortOrder: number;
}

@Component({
  selector: 'app-survey-debug-panel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './survey-debug-panel.component.html',
  styleUrl: './survey-debug-panel.component.scss',
})
export class SurveyDebugPanelComponent implements OnInit {
  private readonly sessionService = inject(SurveySessionService);
  private readonly http = inject(HttpClient);
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);

  readonly isVisible = signal(false);
  readonly activeTab = signal<DebugTab>('overview');
  readonly session = signal<SurveySession | null>(null);
  readonly rawJson = signal('');
  readonly profiles = signal<AutofillProfileEntry[]>([]);
  readonly surveyType = signal('');
  readonly selectedProfileId = signal('');

  readonly tabs: { id: DebugTab; label: string }[] = [
    { id: 'overview', label: 'Overview' },
    { id: 'raw', label: 'Raw' },
    { id: 'autofill', label: 'Autofill' },
  ];

  ngOnInit(): void {
    // Read surveyType from URL query param
    this.route.queryParams
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(params => {
        const type = params['name'] ?? params['type'] ?? '';
        this.surveyType.set(type);
        if (type) {
          this.loadProfiles(type);
        }
      });

    // Refresh session data every 3 seconds
    interval(3000)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.refresh());
  }

  @HostListener('window:keydown', ['$event'])
  onKeydown(e: KeyboardEvent): void {
    const tag = (e.target as HTMLElement)?.tagName;
    if (tag === 'INPUT' || tag === 'TEXTAREA') return;
    if (
      (e.ctrlKey || e.metaKey) &&
      !e.altKey &&
      (e.code === 'Backquote' || e.key === '`' || e.key === '~')
    ) {
      e.preventDefault();
      this.isVisible.update(v => !v);
      if (this.isVisible()) this.refresh();
    }
  }

  setTab(tab: DebugTab): void {
    this.activeTab.set(tab);
    if (tab === 'raw') this.refresh();
  }

  onProfileChange(event: Event): void {
    this.selectedProfileId.set((event.target as HTMLSelectElement).value);
  }

  private refresh(): void {
    const type = this.surveyType();
    if (type) {
      this.session.set(this.sessionService.getSession(type));
    }
    try {
      const raw = localStorage.getItem('cgg_survey_sessions') ?? '{}';
      this.rawJson.set(JSON.stringify(JSON.parse(raw), null, 2));
    } catch {
      this.rawJson.set('(error parsing localStorage)');
    }
  }

  private loadProfiles(surveyType: string): void {
    if (!surveyType) return;
    this.http
      .get<AutofillProfileEntry[]>(`/api/survey-example?name=${surveyType}`)
      .subscribe({
        next: list => this.profiles.set(list),
        error: () => this.profiles.set([]),
      });
  }

  reloadSurveys(): void {
    this.http.post('/api/survey/reload', {}).subscribe({
      next: () => { this.loadProfiles(this.surveyType()); },
      error: () => {},
    });
  }

  formatStepResult(r: AiStepResult): string {
    try {
      const parsed = JSON.parse(r.resultJson ?? '{}');
      const matches: { title: string; matchPercentage: number }[] = parsed.matches ?? [];
      const top = matches
        .sort((a, b) => b.matchPercentage - a.matchPercentage)
        .slice(0, 3)
        .map(m => `${m.title} ${m.matchPercentage}%`)
        .join(', ');
      const tok = r.tokensUsed ?? '—';
      return top ? `${top} (${r.outputFormat}, ${tok} tok)` : `(no matches, ${tok} tok)`;
    } catch {
      return `(parse error, ${r.tokensUsed ?? '—'} tok)`;
    }
  }

  applyAutofill(): void {
    const slug = this.selectedProfileId();
    if (!slug) return;

    const profile = this.profiles().find(p => p.slug === slug);
    if (!profile) return;

    window.dispatchEvent(new CustomEvent('debugAutofill', { detail: profile.answers }));
  }
}
