import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BankRoutingModule } from './bank-routing.module';
import { BankAccountComponent } from './components/bank-account/bank-account.component';
import { BankAccountModalComponent } from './components/bank-account-modal/bank-account-modal.component';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { BankComponent } from './components/bank/bank.component';
import { BankModalComponent } from './components/bank-modal/bank-modal.component';
import { BankLayoutComponent } from './components/bank-layout/bank-layout.component';

@NgModule({
  declarations: [BankAccountComponent, BankAccountModalComponent, BankComponent, BankModalComponent, BankLayoutComponent],
  imports: [
    CommonModule,
    BankRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  entryComponents: [
    BankAccountModalComponent,
    BankModalComponent
  ]
})
export class BankModule { }
