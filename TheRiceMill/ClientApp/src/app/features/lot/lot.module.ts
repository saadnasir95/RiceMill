import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GatepassRoutingModule } from './lot-routing.module';
import { LotComponent } from './components/lot/lot.component';
import { LotModalComponent } from './components/lot-modal/lot-modal.component';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatChipsModule, MatIconModule } from '@angular/material';

@NgModule({
  declarations: [LotComponent, LotModalComponent],
  imports: [
    MatChipsModule,
    MatIconModule,
    CommonModule,
    GatepassRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  entryComponents: [
    LotModalComponent
  ],
})
export class LotModule { }
