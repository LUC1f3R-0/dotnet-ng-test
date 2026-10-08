import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { AuthService } from '../services/auth';

export const guestGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.me().pipe(
    map(isLoggedIn => isLoggedIn ? router.parseUrl(route.queryParamMap.get('returnUrl') ?? '/dashboard') : true),
    catchError(() => of(true)) // request failed -> treat as not logged in
  );
};