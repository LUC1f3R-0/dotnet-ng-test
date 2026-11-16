import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../environments/environment';

export const credentialsInterceptor: HttpInterceptorFn = (req, next) => {
  const apiUrl = new URL(environment.baseUrl, window.location.origin);
  const requestUrl = new URL(req.url, window.location.origin);

  if (requestUrl.origin == apiUrl.origin && requestUrl.pathname.startsWith(apiUrl.pathname.replace(/\/$/, '') + '/'))
  {
    req = req.clone({
      withCredentials: true
    });
  }
    
  return next(req);
};
