import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TokenService } from './token.service';
import { ChangePassword } from '../model/change-password.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiUrl = environment.baseUrl + '/api/v1/User';
  constructor(private http: HttpClient, private tokenService: TokenService) { }

  changePassword(changePassword: ChangePassword): Observable<any> {
    return this.http.put(this.apiUrl + '/ChangePassword', JSON.stringify(changePassword), { headers: this.tokenService.getHeaders() });
  }
}
