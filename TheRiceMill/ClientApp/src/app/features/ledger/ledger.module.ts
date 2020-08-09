import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PartyLedgerComponent } from './components/party-ledger/party-ledger.component';
import { LedgerRoutingModule } from './ledger-routing.module';
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';
import { CompanyLedgerComponent } from './components/company-ledger/company-ledger.component';

@NgModule({
  declarations: [PartyLedgerComponent,CompanyLedgerComponent],
  imports: [
    ReactiveFormsModule,
    NgxDaterangepickerMd.forRoot(),
    CommonModule,
    LedgerRoutingModule,
    SharedModule,
    FormsModule
  ]
})
export class LedgerModule { }
