import { createAction, props } from '@ngrx/store';
import { ChildProfile } from './family.state';

// Load Children
export const loadChildren = createAction('[Family] Load Children');

export const loadChildrenSuccess = createAction(
  '[Family] Load Children Success',
  props<{ children: ChildProfile[] }>()
);

export const loadChildrenFailure = createAction(
  '[Family] Load Children Failure',
  props<{ error: string }>()
);

// Add Child
export const addChild = createAction(
  '[Family] Add Child',
  props<{ name: string; email?: string; age: number; gender: string }>()
);

export const addChildSuccess = createAction(
  '[Family] Add Child Success',
  props<{ child: ChildProfile }>()
);

export const addChildFailure = createAction(
  '[Family] Add Child Failure',
  props<{ error: string }>()
);

// Clear add child error
export const clearAddChildError = createAction('[Family] Clear Add Child Error');
