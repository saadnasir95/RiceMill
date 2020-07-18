import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivate, CanActivateChild } from '@angular/router';
import { JwtToken } from '../model/jwt-token.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(private authService: AuthService, private tokenService: TokenService) { }

  canActivate(route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    return this.authService.isAuthenticated();
  }
  canActivateChild(route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    let isAuthorized = false;
    if (this.authService.isAuthenticated()) {
      const userRoles: string[] = this.tokenService.userInfo.roles.split(',');
      const routeRoles: String[] = route.data.role;
      userRoles.forEach((userRole) => {
        routeRoles.forEach((routeRole) => {
          if (userRole === routeRole) {
            isAuthorized = true;
          }
        });
      });
    }
    return isAuthorized;
  }

}
