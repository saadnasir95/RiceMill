import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PartyLedgerComponent } from './components/party-ledger/party-ledger.component';
import { CompanyLedgerComponent } from './components/company-ledger/company-ledger.component';

const routes: Routes = [
{ path: 'party', component: PartyLedgerComponent },
{ path: 'company', component: CompanyLedgerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LedgerRoutingModule { }
