import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth";
import { inject } from "@angular/core";
import { catchError, map, of } from "rxjs";

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.me().pipe(
    map(isLoggedIn =>
      isLoggedIn
        ? true
        : router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } })
    ),
    catchError(() => of(router.createUrlTree(['/login'])))
  );
};