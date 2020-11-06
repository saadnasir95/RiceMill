import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VoucherRoutingModule } from './voucher-routing.module';
import { VoucherComponent } from './components/voucher/voucher.component';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { VoucherModalComponent } from './components/voucher-modal/voucher-modal.component';
import { MatChipsModule } from '@angular/material';

@NgModule({
  declarations: [VoucherComponent, VoucherModalComponent],
  imports: [
    CommonModule,
    VoucherRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    MatChipsModule
  ],
  entryComponents: [VoucherModalComponent]
})
export class VoucherModule { }
