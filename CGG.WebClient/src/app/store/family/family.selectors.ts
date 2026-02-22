import { createFeatureSelector, createSelector } from '@ngrx/store';
import { FamilyState } from './family.state';

export const selectFamilyState = createFeatureSelector<FamilyState>('family');

export const selectChildren = createSelector(
  selectFamilyState,
  (state) => state.children
);

export const selectChildrenLoading = createSelector(
  selectFamilyState,
  (state) => state.loading
);

export const selectChildrenLoaded = createSelector(
  selectFamilyState,
  (state) => state.loaded
);

export const selectFamilyError = createSelector(
  selectFamilyState,
  (state) => state.error
);

export const selectAddingChild = createSelector(
  selectFamilyState,
  (state) => state.addingChild
);

export const selectAddChildError = createSelector(
  selectFamilyState,
  (state) => state.addChildError
);
