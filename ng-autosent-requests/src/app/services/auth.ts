import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { LoginDetails, RegisterDetails } from '../Models/Auth-Models';
import { APIResponse } from '../Models/reponse';

@Service()
export class AuthService {

  private http = inject(HttpClient);
  
  me(): Observable<boolean> {
    return this.http.get<boolean>(`${environment.baseUrl}/auth/me`);
  };
  
  register(register: RegisterDetails): Observable<APIResponse<null>> {
    return this.http.post<APIResponse<null>>(`${environment.baseUrl}/auth/register`, register);
  }

  login(login: LoginDetails) {
    
  }
}