import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { RegisterDetails } from '../Models/Auth-Models';
import { APIResponse } from '../Models/reponse';

@Service()
export class AuthService {

  private http = inject(HttpClient);
  
  me(): Observable<boolean> {
    return this.http.get<boolean>(`${environment.baseUrl}/auth/me`);
  };
  
  register(register: RegisterDetails): Observable<APIResponse<null>> {
    console.log("method ran")
    return this.http.post<APIResponse<null>>(`${environment.baseUrl}/auth/register`, register);
  }
}