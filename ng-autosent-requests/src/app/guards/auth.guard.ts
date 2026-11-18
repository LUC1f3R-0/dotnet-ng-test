import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { AuthService } from '../services/auth';

export const authGuard: CanActivateFn = (route, state) => {
  
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.me().pipe(
    // A successful /me response means the request was authenticated.
    map(() => true),

    // If /me and any refresh attempt fail, redirect to login.
    catchError(() =>
      of(
        router.createUrlTree(['/login'], {
          queryParams: {
            returnUrl: state.url
          }
        })
      )
    )
  );
};
