import { Injectable } from '@angular/core';
import { JwtToken } from '../model/jwt-token.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpHeaders } from '@angular/common/http';
import { UserInfo } from '../model/user-info.model';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  jwtToken: JwtToken;
  userInfo: UserInfo;

  constructor(private jwtHelperService: JwtHelperService) {
    const jwtToken: JwtToken = JSON.parse(localStorage.getItem('token'));
    if (jwtToken && !this.jwtHelperService.isTokenExpired(jwtToken.id_token)) {
      this.jwtToken = jwtToken;
      this.setUserInfo(this.jwtHelperService.decodeToken(jwtToken.id_token));
    }
  }

  getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + this.jwtToken.access_token
    });
  }
  setUserInfo(userInfo: UserInfo) {
    this.userInfo = userInfo;
    this.userInfo.roles = userInfo.roles.toString();
  }
}
