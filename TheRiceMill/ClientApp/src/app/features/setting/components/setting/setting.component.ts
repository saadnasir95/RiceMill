import { Component, OnInit } from '@angular/core';
import { ChangePasswordComponent } from '../change-password/change-password.component';
import { MatDialogRef, MatDialog } from '@angular/material';
import { TokenService } from '../../../../shared/services/token.service';
import { UserInfo } from '../../../../shared/model/user-info.model';

@Component({
  selector: 'app-setting',
  templateUrl: './setting.component.html',
  styleUrls: ['./setting.component.scss']
})
export class SettingComponent implements OnInit {
  dialogRef: MatDialogRef<ChangePasswordComponent>;
  userInfo: UserInfo;
  constructor(private matDialog: MatDialog, private tokenService: TokenService) { }

  ngOnInit() {
    this.userInfo = this.tokenService.userInfo;
  }

  changePassword() {
    this.dialogRef = this.matDialog.open(ChangePasswordComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }

}
