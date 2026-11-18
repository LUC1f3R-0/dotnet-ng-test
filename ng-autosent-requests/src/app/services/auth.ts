import { HttpClient } from '@angular/common/http';
import { inject, Service, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { LoginDetails, RegisterDetails } from '../Models/Auth-Models';
import { APIResponse } from '../Models/reponse';
import { User } from '../Models/User-Models';

@Service()
export class AuthService {

  private http = inject(HttpClient);

  readonly currentUser = signal<User | null>(null);
  
  me(): Observable<User> {
    console.log("front me running");
    return this.http.get<User>(`${environment.baseUrl}/auth/me`).pipe(
      tap(user => {
        this.currentUser.set(user);
      }),
      catchError(error => {
        this.currentUser.set(null);
        return throwError(() => error);
      })
    );
  }
  
  register(register: RegisterDetails): Observable<APIResponse<null>> {
    return this.http.post<APIResponse<null>>(`${environment.baseUrl}/auth/register`, register);
  }

  login(login: LoginDetails): Observable<APIResponse<User>> {
    return this.http.post<APIResponse<User>>(`${environment.baseUrl}/auth/login`, login,
      // { withCredentials: true }
    );
  }

  refresh(): Observable<APIResponse<null>> {
    return this.http.post<APIResponse<null>>(`${environment.baseUrl}/auth/refresh`, {});
  }
}