import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { switchMap, take } from 'rxjs/operators';
import * as AuthSelectors from '../../store/auth/auth.selectors';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const store = inject(Store);

  return store.select(AuthSelectors.selectToken).pipe(
    take(1),
    switchMap(token => {
      if (token) {
        req = req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        });
      }
      return next(req);
    })
  );
};
