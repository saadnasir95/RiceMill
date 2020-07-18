import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BankRoutingModule } from './bank-transaction-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BankTransactionComponent } from './components/bank-transaction/bank-transaction.component';
import { BankTransactionModalComponent } from './components/bank-transaction-modal/bank-transaction-modal.component';

@NgModule({
  declarations: [BankTransactionComponent, BankTransactionModalComponent],
  imports: [
    CommonModule,
    BankRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule
  ],
  entryComponents: [
    BankTransactionModalComponent
  ]
})
export class BankTransactionModule { }
