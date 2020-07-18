import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NotificationService } from '../../../../shared/services/notification.service';
import { Subscription } from 'rxjs';
import { JwtToken } from '../../../../shared/model/jwt-token.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from '../../../../shared/services/auth.service';
import { Router } from '@angular/router';
import { TokenService } from '../../../../shared/services/token.service';
import { TranslateService } from '@ngx-translate/core';
import { SpinnerService } from '../../../../shared/services/spinner.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  public loginForm: FormGroup;
  private loginSubscription: Subscription;
  constructor(
    private authService: AuthService,
    private notificationService: NotificationService,
    private router: Router,
    private jwtHelperService: JwtHelperService,
    private tokenService: TokenService,
    private translationService: TranslateService,
    public spinner: SpinnerService
  ) {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['admin']);
    }
  }

  ngOnInit() {
    this.loginForm = new FormGroup({
      email: new FormControl(null, [Validators.required]),
      password: new FormControl(null, Validators.required)
    });
  }
  ngOnDestroy() {
    if (this.loginSubscription) {
      this.loginSubscription.unsubscribe();
    }
  }

  onSubmit() {
    // this.notificationService.infoNotifcation('Attempting login');
    if (this.loginForm.valid) {
      this.spinner.isLoading = true;
      const email = this.loginForm.value.email;
      const password = this.loginForm.value.password;
      this.loginSubscription = this.authService.login(email, password).subscribe(
        (data: JwtToken) => {
          localStorage.setItem('token', JSON.stringify(data));
          this.tokenService.jwtToken = data;
          this.tokenService.setUserInfo(this.jwtHelperService.decodeToken(data.id_token));
          this.router.navigate(['admin']);
          this.notificationService.successNotifcation('Login Successfull');
          this.spinner.isLoading = false;
        },
        (error: any) => {
          localStorage.clear();
          this.notificationService.errorNotifcation('Invalid Email or password');
          this.spinner.isLoading = false;
        }
      );
    }
  }

}
