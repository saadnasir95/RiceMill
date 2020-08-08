import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { PartyLedgerComponent } from './components/party-ledger/party-ledger.component';
import { LedgerRoutingModule } from './ledger-routing.module';

@NgModule({
  declarations: [PartyLedgerComponent],
  imports: [
    CommonModule,
    LedgerRoutingModule,
    SharedModule,
    FormsModule
  ]
})
export class LedgerModule { }
