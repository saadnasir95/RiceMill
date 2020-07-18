import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GatepassComponent } from './components/gatepass/gatepass.component';

const routes: Routes = [
  { path: '', component: GatepassComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GatepassRoutingModule { }
