import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeadRoutingModule } from './head-routing.module';
import { HeadComponent } from './components/head/head.component';
import { SharedModule } from '../../shared/shared.module';
import { HeadModalComponent } from './components/head-modal/head-modal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [HeadComponent, HeadModalComponent],
  imports: [
    CommonModule,
    HeadRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule
  ],
  entryComponents: [HeadModalComponent]
})
export class HeadModule { }
