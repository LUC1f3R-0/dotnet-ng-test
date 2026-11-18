import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { AuthRefreshService } from '../services/auth-refresh';
import { inject } from '@angular/core';
import { environment } from '../../environments/environment';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const refreshService = inject(AuthRefreshService);
  
  const apiUrl = new URL(environment.baseUrl, window.location.origin);
  
  const requestUrl = new URL(req.url, window.location.origin);

  const apiPath = apiUrl.pathname.replace(/\/$/, '');

  // Only handle requests directed to our API.
  const isApiRequest = requestUrl.origin === apiUrl.origin &&
    (requestUrl.pathname === apiPath || requestUrl.pathname.startsWith(apiPath + '/'));
    
    if (!isApiRequest) {
      return next(req);
    }
  
    // These endpoints must not trigger automatic refresh.
    const excludedPaths = [
      `${apiPath}/auth/login`,
      `${apiPath}/auth/register`,
      `${apiPath}/auth/refresh`
    ];
  
    if (excludedPaths.includes(requestUrl.pathname)) {
      return next(req);
    }
  
    const requestTokenVersion = refreshService.version;
  
    return next(req).pipe(
      catchError((error: unknown) => {
  
        // Only try refreshing for HTTP 401 errors.
        if (!(error instanceof HttpErrorResponse) || error.status !== 401)
        {
          return throwError(() => error);
        }
  
        // Another request may already have refreshed the token.
        // Retry with the updated cookies.
        if (refreshService.version > requestTokenVersion)
        {
          return next(req);
        }
  
        // Request new cookies using the refresh token.
        return refreshService.refresh().pipe(
          // When refreshing succeeds, retry the original request.
          switchMap(() => next(req))
        );
      })
    );
};
