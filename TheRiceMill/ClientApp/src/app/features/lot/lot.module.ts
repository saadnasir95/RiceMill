import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LotRoutingModule } from './lot-routing.module';
import { LotComponent } from './components/lot/lot.component';
import { LotModalComponent } from './components/lot-modal/lot-modal.component';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatChipsModule, MatIconModule } from '@angular/material';
import { AgGridModule } from 'ag-grid-angular';
import { TemplateRendererComponent } from '../../shared/components/template-renderer/template-renderer.component';
import { RateCostModalComponent } from './components/rate-cost-modal/rate-cost-modalcomponent';
import { LotReceiptComponent } from './components/lot-receipt/lot-receipt.component';

@NgModule({
  declarations: [LotComponent, LotModalComponent,RateCostModalComponent,TemplateRendererComponent,LotReceiptComponent],
  imports: [
    MatChipsModule,
    MatIconModule,
    CommonModule,
    LotRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    AgGridModule.withComponents([]),
  ],
  entryComponents: [
    LotModalComponent,
    RateCostModalComponent,
    TemplateRendererComponent
  ],
})
export class LotModule { }
