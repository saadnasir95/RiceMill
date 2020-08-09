import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CompanyLedgerComponent } from './company-ledger/company-ledger.component';

const routes: Routes = [
  { path: '', component: CompanyLedgerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompanyLedgerRoutingModule { }
