import { Component, OnInit, computed, inject, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SurveySessionService } from '../../../../features/survey/services/survey-session.service';
import { SurveyCardComponent } from '../../../../features/survey/components/survey-card/survey-card.component';
import { SurveyResultMiniComponent } from '../../../../features/survey/components/survey-result-mini/survey-result-mini.component';
import { UserRole } from '../../../../store/auth/auth.state';

export type SurveyContext = {
  surveyType: string;
  emoji: string;
  titleKey: string;
  descriptionKey: string;
  estimatedMinutes: number;
};

const SURVEY_CONTEXTS: Record<string, SurveyContext> = {
  'ab-test': {
    surveyType: 'ab-test',
    emoji: '🎓',
    titleKey: 'surveys.abTest.title',
    descriptionKey: 'surveys.abTest.description',
    estimatedMinutes: 15,
  },
  'parent': {
    surveyType: 'parent',
    emoji: '👪',
    titleKey: 'surveys.parent.title',
    descriptionKey: 'surveys.parent.description',
    estimatedMinutes: 10,
  },
  'parent-child-talents': {
    surveyType: 'parent-child-talents',
    emoji: '⭐',
    titleKey: 'surveys.parentChildTalents.title',
    descriptionKey: 'surveys.parentChildTalents.description',
    estimatedMinutes: 10,
  },
};

@Component({
  selector: 'app-survey-tab',
  standalone: true,
  imports: [CommonModule, TranslateModule, SurveyCardComponent, SurveyResultMiniComponent],
  templateUrl: './survey-tab.html',
  styleUrl: './survey-tab.scss',
})
export class SurveyTabComponent implements OnInit {
  /** True when displaying a child's profile (viewed by a parent). */
  isChild = input<boolean>(false);
  /** Role of the currently logged-in user (e.g. 'UserParent', 'UserStudent', 'UserChild'). */
  userRole = input<string>('');
  /** Route ID of the child when viewing a child's profile. */
  childId = input<string | null>(null);
  /** Emits whenever the has-results state changes (used by parent to lock tabs). */
  hasResultsChange = output<boolean>();

  private sessionSvc = inject(SurveySessionService);
  private router = inject(Router);

  /** Which survey types to display in this context. */
  surveyTypes = computed<SurveyContext[]>(() => {
    const types: string[] = [];
    if (this.isChild()) {
      // Parent viewing child profile
      types.push('parent-child-talents');
      types.push('ab-test');
    } else {
      const role = this.userRole();
      if (Number(role) === UserRole.UserParent) {
        types.push('parent');
        types.push('ab-test');
      } else {
        // UserChild, UserStudent, or any other role
        types.push('ab-test');
      }
    }
    return types.map(t => SURVEY_CONTEXTS[t]).filter(Boolean);
  });

  /** True if at least one of the relevant surveys has a completed session. */
  hasResults = computed(() =>
    this.surveyTypes().some(ctx => this.sessionSvc.hasCompletedSession(ctx.surveyType))
  );

  ngOnInit(): void {
    // Notify the parent component of the initial state
    this.hasResultsChange.emit(this.hasResults());
  }

  isCompleted(surveyType: string): boolean {
    return this.sessionSvc.hasCompletedSession(surveyType);
  }

  getSession(surveyType: string) {
    return this.sessionSvc.getSession(surveyType);
  }

  startSurvey(surveyType: string): void {
    const params: Record<string, string> = { name: surveyType };
    if (this.childId()) params['childId'] = this.childId()!;
    this.router.navigate(['/survey'], { queryParams: params });
  }

  retakeSurvey(surveyType: string): void {
    this.sessionSvc.clearSession(surveyType);
    this.startSurvey(surveyType);
  }

  viewFullResults(surveyType: string): void {
    const params: Record<string, string> = { name: surveyType, view: 'results' };
    if (this.childId()) params['childId'] = this.childId()!;
    this.router.navigate(['/survey'], { queryParams: params });
  }
}
