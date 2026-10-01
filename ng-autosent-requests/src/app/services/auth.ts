import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Service()
export class AuthService {

  private http = inject(HttpClient);
  
  me(): Observable<boolean> {
    return this.http.get<boolean>(`${environment.baseUrl}/auth/me`);
  }
}