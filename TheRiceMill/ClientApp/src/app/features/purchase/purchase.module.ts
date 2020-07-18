import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PurchaseRoutingModule } from './purchase-routing.module';
import { PurchaseComponent } from './components/purchase/purchase.component';
import { SharedModule } from '../../shared/shared.module';
import { PurchaseModalComponent } from './components/purchase-modal/purchase-modal.component';
import { ReactiveFormsModule } from '@angular/forms';
import { PurchaseReceiptComponent } from './components/purchase-receipt/purchase-receipt.component';

@NgModule({
  declarations: [PurchaseComponent, PurchaseModalComponent, PurchaseReceiptComponent],
  imports: [
    CommonModule,
    PurchaseRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  entryComponents: [PurchaseModalComponent]
})
export class PurchaseModule { }
