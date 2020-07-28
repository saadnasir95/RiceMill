import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PartyRoutingModule } from './party-routing.module';
import { PartyComponent } from './components/party/party.component';
import { PartyModalComponent } from './components/party-modal/party-modal.component';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [PartyComponent, PartyModalComponent],
  entryComponents: [PartyModalComponent],
  imports: [
    CommonModule,
    PartyRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ]
})
export class PartyModule { }
