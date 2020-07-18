import { Injectable } from '@angular/core';
import { HttpHeaders, HttpParams, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { JwtToken } from '../model/jwt-token.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { TokenService } from './token.service';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginUrl = environment.baseUrl + '/connect/token';
  constructor(
    private httpClient: HttpClient,
    private jwtHelperService: JwtHelperService,
    private router: Router,
    private tokenService: TokenService) {
  }

  isAuthenticated(): boolean {
    if (this.tokenService.jwtToken && !this.jwtHelperService.isTokenExpired(this.tokenService.jwtToken.id_token)) {
      return true;
    } else {
      localStorage.clear();
      this.tokenService.jwtToken = null;
      this.tokenService.userInfo = null;
      this.router.navigate(['']);
      return false;
    }
  }

  login(email: string, password: string): Observable<JwtToken> {
    const header = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
    const params = new HttpParams()
      .append('username', email)
      .append('password', password)
      .append('grant_type', 'password')
      .append('granttype', 'password')
      .append('scope', 'openid email profile offline_access roles');
    const requestBody = params.toString();
    return this.httpClient.post<JwtToken>(this.loginUrl, requestBody, {
      headers: header
    }).pipe(
      catchError(this.handleError)
    );
  }
  logout() {
    localStorage.clear();
    this.tokenService.jwtToken = null;
    this.tokenService.userInfo = null;
    this.router.navigate(['']);
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    return throwError(
      'Something bad happened; please try again later.');
  }
}
