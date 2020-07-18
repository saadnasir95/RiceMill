import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BankAccountRoutingModule } from './bank-account-routing.module';
import { BankAccountComponent } from './components/bank-account/bank-account.component';
import { BankAccountModalComponent } from './components/bank-account-modal/bank-account-modal.component';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [BankAccountComponent, BankAccountModalComponent],
  imports: [
    CommonModule,
    BankAccountRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  entryComponents: [
    BankAccountModalComponent
  ]
})
export class BankAccountModule { }
