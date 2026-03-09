import { Component, OnInit, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Store } from '@ngrx/store';
import { selectUser } from '../../store/auth/auth.selectors';
import { selectChildById, selectChildrenLoaded } from '../../store/family/family.selectors';
import { loadChildren } from '../../store/family/family.actions';
import { SurveyTabComponent } from './tabs/survey-tab/survey-tab';
import { SurveySessionService } from '../../features/survey/services/survey-session.service';
import { ObservationsTabComponent } from './tabs/observations-tab/observations-tab';
import { LocalRequestTabComponent } from './tabs/local-request-tab/local-request-tab';

export type Tab = 'surveyTab' | 'observations' | 'localRequest';

interface ProfileData {
  id: string;
  name: string;
  roleLabel: string;
  age?: number;
  gender?: string;
  email?: string;
  isChild: boolean;
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, TranslateModule, SurveyTabComponent, ObservationsTabComponent, LocalRequestTabComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private store = inject(Store);
  private sessionSvc = inject(SurveySessionService);

  // If there's an ID in the route, we are viewing a specific child/user
  routeId = this.route.snapshot.paramMap.get('id');

  // Selectors
  currentUser = this.store.selectSignal(selectUser);
  childrenLoaded = this.store.selectSignal(selectChildrenLoaded);
  
  // We use a computed signal to get the child if routeId is present
  child = this.routeId ? this.store.selectSignal(selectChildById(this.routeId)) : () => null;

  // Unified profile data
  profile = computed<ProfileData | null>(() => {
    if (this.routeId) {
      const c = this.child();
      if (!c) return null;
      return {
        id: c.id,
        name: c.name,
        roleLabel: 'childProfile.roleLabel',
        age: c.age,
        gender: c.gender,
        email: c.email,
        isChild: true
      };
    } else {
      const u = this.currentUser();
      if (!u) return null;
      
      let roleLabel = 'parentProfile.roleLabel';
      if (u.role === 'UserStudent') roleLabel = 'studentProfile.roleLabel';
      
      return {
        id: u.id,
        name: u.firstName || u.email,
        roleLabel,
        email: u.email,
        isChild: false
      };
    }
  });

  /** True if the relevant survey for this profile context has a completed session or any results in localStorage. */
  get hasResults(): boolean {
    const isChild = this.profile()?.isChild ?? false;
    const role = this.currentUser()?.role ?? '';
    const hasResultsFor = (type: string) =>
      this.sessionSvc.hasCompletedSession(type) || this.sessionSvc.hasAnyResults(type);
    if (isChild) {
      return hasResultsFor('parent-child-talents') || hasResultsFor('ab-test');
    }
    const surveyType = role === 'UserParent' ? 'parent' : 'ab-test';
    return hasResultsFor(surveyType);
  }

  activeTab: Tab = 'surveyTab';

  ngOnInit(): void {
    if (this.routeId && !this.childrenLoaded()) {
      this.store.dispatch(loadChildren());
    }
  }

  setTab(tab: Tab) {
    if ((tab === 'observations' || tab === 'localRequest') && !this.hasResults && this.profile()?.isChild) {
      // Only lock tabs for children without results. For parents/students, maybe they are always unlocked or have different logic.
      // For now, keep the same logic if it's a child.
      if (this.profile()?.isChild) {
         return;
      }
    }
    this.activeTab = tab;
  }

  editProfile() {
    const p = this.profile();
    if (p?.isChild) {
      this.router.navigate(['/child/edit', p.id]);
    } else {
      // TODO: Navigate to user edit profile if needed
    }
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }
}
