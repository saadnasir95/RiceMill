import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BankLayoutComponent } from './components/bank-layout/bank-layout.component';

const routes: Routes = [
  { path: '', component: BankLayoutComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BankRoutingModule { }
