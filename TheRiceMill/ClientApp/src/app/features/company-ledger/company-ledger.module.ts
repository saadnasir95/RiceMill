import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompanyLedgerComponent } from './company-ledger/company-ledger.component';
import { RouterModule } from '@angular/router';
import { CompanyLedgerRoutingModule } from './company-ledger-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';

@NgModule({
  declarations: [CompanyLedgerComponent],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    NgxDaterangepickerMd.forRoot(),
    CompanyLedgerRoutingModule,
  ]
})
export class CompanyLedgerModule { }
