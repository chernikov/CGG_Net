import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, catchError, exhaustMap, switchMap, tap } from 'rxjs/operators';
import { ApiService } from '../../core/services/api.service';
import { ChildProfile } from './family.state';
import * as FamilyActions from './family.actions';

@Injectable()
export class FamilyEffects {
  private actions$ = inject(Actions);
  private apiService = inject(ApiService);
  private router = inject(Router);

  loadChildren$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FamilyActions.loadChildren),
      switchMap(() =>
        this.apiService.get<ChildProfile[]>('family/children').pipe(
          map((children) => FamilyActions.loadChildrenSuccess({ children })),
          catchError((error) =>
            of(FamilyActions.loadChildrenFailure({
              error: error.error?.message || 'Failed to load children'
            }))
          )
        )
      )
    )
  );

  addChild$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FamilyActions.addChild),
      exhaustMap((action) =>
        this.apiService.post<ChildProfile>('family/children', {
          name: action.name,
          email: action.email,
          age: action.age,
          gender: action.gender
        }).pipe(
          map((child) => FamilyActions.addChildSuccess({ child })),
          catchError((error) =>
            of(FamilyActions.addChildFailure({
              error: error.error?.message || 'Failed to add child'
            }))
          )
        )
      )
    )
  );

  addChildSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(FamilyActions.addChildSuccess),
        tap(() => {
          this.router.navigate(['/dashboard']);
        })
      ),
    { dispatch: false }
  );
}
