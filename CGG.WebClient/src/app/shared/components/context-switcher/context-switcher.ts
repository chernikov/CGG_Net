import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { UserTokenContext } from '../../../store/auth/auth.state';
import {
  selectCanSwitchContext,
  selectActiveContext,
  selectAvailableContexts,
  selectSwitchContextLoading
} from '../../../store/auth/auth.selectors';
import * as AuthActions from '../../../store/auth/auth.actions';

@Component({
  selector: 'app-context-switcher',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './context-switcher.html',
  styleUrl: './context-switcher.scss'
})
export class ContextSwitcher {
  private store = inject(Store);

  canSwitch$ = this.store.select(selectCanSwitchContext);
  activeContext$ = this.store.select(selectActiveContext);
  availableContexts$ = this.store.select(selectAvailableContexts);
  loading$ = this.store.select(selectSwitchContextLoading);

  contextIcon(type: string): string {
    switch (type) {
      case 'Family': return '🏠';
      case 'School': return '🏫';
      default:       return '⚙️';
    }
  }

  contextLabel(ctx: UserTokenContext): string {
    if (ctx.contextName) return ctx.contextName;
    if (ctx.type === 'System') return 'Система';
    return ctx.type;
  }

  switchTo(ctx: UserTokenContext): void {
    this.store.dispatch(AuthActions.switchContext({
      contextType: ctx.type,
      contextId: ctx.contextId
    }));
  }

  isActive(ctx: UserTokenContext, active: UserTokenContext | null): boolean {
    return !!active &&
      ctx.type === active.type &&
      ctx.contextId === active.contextId;
  }
}
