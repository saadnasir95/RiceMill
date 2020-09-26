import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LotComponent } from './components/lot/lot.component';

const routes: Routes = [
  { path: '', component: LotComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LotRoutingModule { }
