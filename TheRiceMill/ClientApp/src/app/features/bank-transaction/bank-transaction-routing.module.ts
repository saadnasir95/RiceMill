import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BankTransactionComponent } from './components/bank-transaction/bank-transaction.component';

const routes: Routes = [
  { path: '', component: BankTransactionComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BankRoutingModule { }
