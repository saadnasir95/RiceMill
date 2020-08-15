import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SaleRoutingModule } from './sale-routing.module';
import { SaleComponent } from './components/sale/sale.component';
import { SharedModule } from '../../shared/shared.module';
import { SaleModalComponent } from './components/sale-modal/sale-modal.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SaleReceiptComponent } from './components/sale-receipt/sale-receipt.component';
import { MatIconModule, MatChipsModule } from '@angular/material';

@NgModule({
  declarations: [SaleComponent, SaleModalComponent, SaleReceiptComponent],
  imports: [
    CommonModule,
    MatChipsModule,
    MatIconModule,
    SaleRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  entryComponents: [
    SaleModalComponent
  ]
})
export class SaleModule { }
