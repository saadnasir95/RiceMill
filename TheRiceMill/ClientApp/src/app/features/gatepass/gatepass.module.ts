import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GatepassRoutingModule } from './gatepass-routing.module';
import { GatepassComponent } from './components/gatepass/gatepass.component';
import { GatepassModalComponent } from './components/gatepass-modal/gatepass-modal.component';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { GatepassReceiptComponent } from './components/gatepass-receipt/gatepass-receipt.component';

@NgModule({
  declarations: [GatepassComponent, GatepassModalComponent, GatepassReceiptComponent],
  imports: [
    CommonModule,
    GatepassRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  entryComponents: [
    GatepassModalComponent
  ],
})
export class GatepassModule { }
