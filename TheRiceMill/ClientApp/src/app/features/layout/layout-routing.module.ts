import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component';
import { AuthGuard } from '../../shared/services/auth-guard.service';


const routes: Routes = [
  { path: 'admin', redirectTo: '/admin/gatepass', pathMatch: 'full' },
  {
    path: 'admin',
    component: LayoutComponent,
    // canActivateChild: [AuthGuard],
    children: [
      { path: 'gatepass', loadChildren: '../gatepass/gatepass.module#GatepassModule', data: { role: ['Administrator', 'GateKeeper'] } },
      { path: 'purchase', loadChildren: '../purchase/purchase.module#PurchaseModule', data: { role: ['Administrator'] } },
      { path: 'sale', loadChildren: '../sale/sale.module#SaleModule', data: { role: ['Administrator'] } },
      { path: 'vehicle', loadChildren: '../vehicle/vehicle.module#VehicleModule', data: { role: ['Administrator'] } },
      { path: 'party', loadChildren: '../party/party.module#PartyModule', data: { role: ['Administrator'] } },
      { path: 'settings', loadChildren: '../setting/setting.module#SettingModule', data: { role: ['Administrator', 'GateKeeper'] } },
      { path: 'product', loadChildren: '../product/product.module#ProductModule', data: { role: ['Administrator'] } },
      { path: 'ledger', loadChildren: '../ledger/ledger.module#LedgerModule', data: { role: ['Administrator'] } },
      { path: 'lot', loadChildren: '../lot/lot.module#LotModule', data: { role: ['Administrator'] } },
      { path: 'bank-transaction', loadChildren: '../bank-transaction/bank-transaction.module#BankTransactionModule', data: { role: ['Administrator'] } },
      { path: 'bank', loadChildren: '../bank/bank.module#BankModule', data: { role: ['Administrator'] } },
      { path: 'voucher', loadChildren: '../voucher/voucher.module#VoucherModule', data: { role: ['Administrator'] } },
      { path: 'head', loadChildren: '../head/head.module#HeadModule', data: { role: ['Administrator'] } }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
