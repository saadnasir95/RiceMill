import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingRoutingModule } from './setting-routing.module';
import { SettingComponent } from './components/setting/setting.component';
import { SharedModule } from '../../shared/shared.module';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [SettingComponent, ChangePasswordComponent],
  imports: [
    CommonModule,
    SettingRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  entryComponents: [ChangePasswordComponent]
})
export class SettingModule { }
