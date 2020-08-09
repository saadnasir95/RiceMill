import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PartyLedgerComponent } from './components/party-ledger/party-ledger.component';

const routes: Routes = [
  { path: 'party', component: PartyLedgerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LedgerRoutingModule { }
