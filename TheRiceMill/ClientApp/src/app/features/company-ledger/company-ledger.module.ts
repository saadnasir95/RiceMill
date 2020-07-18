import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { CompanyLedgerComponent } from './components/company-ledger/company-ledger.component';
import { CompanyLedgerRoutingModule } from './company-ledger-routing.module';

@NgModule({
  declarations: [CompanyLedgerComponent],
  imports: [
    CommonModule,
    CompanyLedgerRoutingModule,
    SharedModule,
    FormsModule
  ]
})
export class CompanyLedgerModule { }
