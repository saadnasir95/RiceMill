import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { ChangePassword } from '../../../../shared/model/change-password.model';
import { UserService } from '../../../../shared/services/user.service';
import { NotificationService } from '../../../../shared/services/notification.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SpinnerService } from '../../../../shared/services/spinner.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  public changePasswordForm: FormGroup;
  changePassword: ChangePassword;
  public modalRef: MatDialogRef<ChangePasswordComponent>;
  constructor(
    private userService: UserService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
    this.changePasswordForm = new FormGroup({
      currentPassword: new FormControl(null, Validators.required),
      // tslint:disable-next-line: quotemark
      newPassword: new FormControl(null, [Validators.required, Validators.pattern("^(?=.*[a-z])(?=.*[$&+,:;=?@#|'<>.^*()%!-])(?=.*[A-Z])(?=.*[0-9]).{8,20}")]),
      confirmPassword: new FormControl(null, [Validators.required])
    }, this.matchPasswordValidation);
  }

  closeModal() {
    this.modalRef.close();
  }

  submit() {
    // tslint:disable-next-line: quotemark
    if (this.changePasswordForm.valid) {
      this.spinner.isLoading = true;
      if (new RegExp("^(?=.*[a-z])(?=.*[$&+,:;=?@#|'<>.^*()%!-])(?=.*[A-Z])(?=.*[0-9]).{8,20}").test(this.changePasswordForm.value.currentPassword)) {
        this.changePassword = new ChangePassword();
        this.changePassword.currentPassword = this.changePasswordForm.value.currentPassword;
        this.changePassword.newPassword = this.changePasswordForm.value.newPassword;
        this.changePassword.confirmPassword = this.changePasswordForm.value.confirmPassword;
        this.userService.changePassword(this.changePassword).subscribe(
          () => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Password Changed Successfully');
            this.closeModal();
          },
          (error: HttpErrorResponse) => {
            this.spinner.isLoading = false;
            this.notificationService.errorNotifcation(error.error[Object.keys(error.error)[1]]);
          }
        );
      } else {
        this.notificationService.errorNotifcation('Incorrect Password');
      }
    }
  }
  matchPasswordValidation(control: AbstractControl): { [s: string]: boolean } {
    const control1 = control.get('newPassword');
    const control2 = control.get('confirmPassword');
    if (control1.value !== control2.value && control1.dirty && control2.dirty && !control1.errors && !control2.errors) {
      return { matchPassword: true };
    }
    return null;
  }
}
