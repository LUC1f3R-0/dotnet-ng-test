import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';

import { AuthService } from '../services/auth';

export const guestGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.me().pipe(
    // User is authenticated: redirect away from login/register.
    map(() => router.createUrlTree(['/dashboard'])),

    // No authenticated session: allow login/register page.
    catchError(() => of(true))
  );
};
