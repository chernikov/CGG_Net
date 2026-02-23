import { createReducer, on } from '@ngrx/store';
import { FamilyState, initialFamilyState } from './family.state';
import * as FamilyActions from './family.actions';

export const familyReducer = createReducer(
  initialFamilyState,

  // Load Children
  on(FamilyActions.loadChildren, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(FamilyActions.loadChildrenSuccess, (state, { children }) => ({
    ...state,
    children,
    loading: false,
    loaded: true,
    error: null
  })),
  on(FamilyActions.loadChildrenFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Add Child
  on(FamilyActions.addChild, (state) => ({
    ...state,
    addingChild: true,
    addChildError: null
  })),
  on(FamilyActions.addChildSuccess, (state, { child }) => ({
    ...state,
    children: [...state.children, child],
    addingChild: false,
    addChildError: null
  })),
  on(FamilyActions.addChildFailure, (state, { error }) => ({
    ...state,
    addingChild: false,
    addChildError: error
  })),

  // Clear add child error
  on(FamilyActions.clearAddChildError, (state) => ({
    ...state,
    addChildError: null
  }))
);
