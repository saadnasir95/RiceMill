import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';

import { VehicleRoutingModule } from './vehicle-routing.module';
import { VehicleComponent } from './components/vehicle/vehicle.component';
import { VehicleModalComponent } from './components/vehicle-modal/vehicle-modal.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [VehicleComponent, VehicleModalComponent],
  entryComponents: [VehicleModalComponent],
  imports: [
    CommonModule,
    VehicleRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ]
})
export class VehicleModule { }
