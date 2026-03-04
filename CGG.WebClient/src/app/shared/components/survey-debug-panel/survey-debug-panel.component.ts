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
import { SurveySession } from '../../../features/survey/models/survey-session.model';

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
  template: `
    <!-- Floating toggle button (when panel is hidden) -->
    @if (!isVisible()) {
      <button
        (click)="isVisible.set(true)"
        class="debug-fab"
        title="Debug panel (Ctrl+~)"
      >🛠️</button>
    }

    <!-- Overlay for Raw tab -->
    @if (isVisible() && activeTab() === 'raw') {
      <div class="debug-overlay" (click)="activeTab.set('overview')"></div>
    }

    <!-- Panel -->
    @if (isVisible()) {
      <div [class]="'debug-panel' + (activeTab() === 'raw' ? ' debug-panel--full' : '')">
        <!-- Header -->
        <div class="debug-header">
          <span class="debug-title">🛠️ Debug</span>
          <div class="debug-tabs">
            @for (tab of tabs; track tab.id) {
              <button
                [class]="'debug-tab' + (activeTab() === tab.id ? ' debug-tab--active' : '')"
                (click)="setTab(tab.id)"
              >{{ tab.label }}</button>
            }
          </div>
          <button class="debug-close" (click)="isVisible.set(false)">✕</button>
        </div>

        <!-- Overview tab -->
        @if (activeTab() === 'overview') {
          <div class="debug-body">
            @if (session()) {
              <table class="debug-table">
                <tbody>
                  <tr><td>surveyType</td><td>{{ session()!.surveyType }}</td></tr>
                  <tr><td>language</td><td>{{ session()!.language }}</td></tr>
                  <tr><td>currentStep</td><td>{{ session()!.currentStep }}</td></tr>
                  <tr><td>steps done</td><td>{{ session()!.steps.length }} / {{ session()!.totalSteps }}</td></tr>
                  <tr><td>AI results</td><td>{{ session()!.results.length }}</td></tr>
                  @for (r of session()!.results; track r.step) {
                    <tr>
                      <td>step {{ r.step }} tokens</td>
                      <td>{{ r.tokensUsed ?? '—' }}</td>
                    </tr>
                  }
                  <tr><td>sessionId</td><td class="debug-mono">{{ session()!.sessionId.slice(0, 8) }}…</td></tr>
                </tbody>
              </table>
            } @else {
              <p class="debug-empty">No session for <b>{{ surveyType() || '(unknown)' }}</b></p>
            }
          </div>
        }

        <!-- Raw tab -->
        @if (activeTab() === 'raw') {
          <div class="debug-body debug-body--raw">
            <pre class="debug-pre">{{ rawJson() }}</pre>
          </div>
        }

        <!-- Autofill tab -->
        @if (activeTab() === 'autofill') {
          <div class="debug-body">
            @if (profiles().length) {
              <div class="debug-autofill">
                <select
                  class="debug-select"
                  [value]="selectedProfileId()"
                  (change)="onProfileChange($event)"
                >
                  <option value="">— Select profile —</option>
                  @for (p of profiles(); track p.id) {
                    <option [value]="p.slug">{{ p.icon }} {{ p.nameUk }}</option>
                  }
                </select>
                <button
                  class="debug-btn"
                  [disabled]="!selectedProfileId()"
                  (click)="applyAutofill()"
                >Заповнити</button>
              </div>
            } @else {
              <p class="debug-empty">No profiles for <b>{{ surveyType() || '(unknown)' }}</b></p>
            }
          </div>
        }

        <div class="debug-footer">Ctrl+~ toggle</div>
      </div>
    }
  `,
  styles: [`
    :host { position: fixed; z-index: 9999; }

    .debug-fab {
      position: fixed; bottom: 16px; right: 16px;
      width: 42px; height: 42px; border-radius: 50%;
      background: #1e293b; color: #f8fafc; font-size: 18px;
      border: none; cursor: pointer; box-shadow: 0 2px 8px #0004;
      display: flex; align-items: center; justify-content: center;
    }

    .debug-overlay {
      position: fixed; inset: 0; background: #00000066; z-index: 9998;
    }

    .debug-panel {
      position: fixed; top: 12px; right: 12px; width: 300px;
      background: #0f172a; color: #e2e8f0; border-radius: 8px;
      box-shadow: 0 4px 24px #0008; font-size: 12px;
      font-family: monospace; z-index: 9999; display: flex; flex-direction: column;
      max-height: calc(100vh - 24px);
    }

    .debug-panel--full {
      width: 90vw; height: 90vh; top: 5vh; right: 5vw; left: auto;
    }

    .debug-header {
      display: flex; align-items: center; gap: 4px;
      padding: 6px 8px; border-bottom: 1px solid #334155;
      flex-shrink: 0;
    }

    .debug-title { font-weight: bold; color: #94a3b8; margin-right: 4px; }

    .debug-tabs { display: flex; gap: 2px; flex: 1; }

    .debug-tab {
      padding: 2px 8px; border-radius: 4px; border: none;
      background: transparent; color: #94a3b8; cursor: pointer; font-size: 11px;
    }
    .debug-tab--active { background: #1e40af; color: #fff; }
    .debug-tab:hover:not(.debug-tab--active) { background: #1e293b; }

    .debug-close {
      background: transparent; border: none; color: #64748b;
      cursor: pointer; padding: 2px 4px; font-size: 14px;
    }
    .debug-close:hover { color: #f87171; }

    .debug-body {
      padding: 8px; overflow-y: auto; flex: 1;
    }

    .debug-body--raw { padding: 0; }

    .debug-table { width: 100%; border-collapse: collapse; }
    .debug-table td { padding: 2px 4px; border-bottom: 1px solid #1e293b; }
    .debug-table td:first-child { color: #94a3b8; width: 50%; }
    .debug-table td:last-child { color: #f8fafc; word-break: break-all; }

    .debug-mono { font-family: monospace; }

    .debug-empty { color: #64748b; text-align: center; margin: 16px 0; }

    .debug-pre {
      white-space: pre-wrap; word-break: break-all;
      padding: 8px; margin: 0; color: #86efac; font-size: 11px;
      overflow-y: auto; height: 100%;
    }

    .debug-autofill { display: flex; flex-direction: column; gap: 8px; }

    .debug-select {
      background: #1e293b; color: #e2e8f0; border: 1px solid #334155;
      border-radius: 4px; padding: 4px 6px; font-size: 12px; width: 100%;
    }

    .debug-btn {
      background: #1d4ed8; color: #fff; border: none; border-radius: 4px;
      padding: 6px 12px; cursor: pointer; font-size: 12px;
    }
    .debug-btn:disabled { background: #334155; color: #64748b; cursor: not-allowed; }
    .debug-btn:not(:disabled):hover { background: #2563eb; }

    .debug-footer {
      padding: 4px 8px; color: #334155; font-size: 10px;
      border-top: 1px solid #1e293b; text-align: right; flex-shrink: 0;
    }
  `],
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
    this.http
      .get<AutofillProfileEntry[]>(`/api/survey-example?name=${surveyType}`)
      .subscribe({
        next: list => this.profiles.set(list),
        error: () => this.profiles.set([]),
      });
  }

  applyAutofill(): void {
    const slug = this.selectedProfileId();
    if (!slug) return;

    const profile = this.profiles().find(p => p.slug === slug);
    if (!profile) return;

    window.dispatchEvent(new CustomEvent('debugAutofill', { detail: profile.answers }));
  }
}
