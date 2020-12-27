import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeadRoutingModule } from './head-routing.module';
import { HeadComponent } from './components/head/head.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [HeadComponent],
  imports: [
    CommonModule,
    HeadRoutingModule,
    SharedModule
  ]
})
export class HeadModule { }
