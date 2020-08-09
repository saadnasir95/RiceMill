import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatButtonModule,
  MatCheckboxModule,
  MatCardModule,
  MatFormFieldModule,
  MatInputModule,
  MatSidenavModule,
  MatTableModule,
  MatSortModule,
  MatPaginatorModule,
  MatProgressSpinnerModule,
  MatTooltipModule,
  MatDialogModule,
  MatSelectModule,
  MatAutocompleteModule,
  MatRadioModule,
  MatDividerModule,
  MatButtonToggleModule
} from '@angular/material';
import { ModalComponent } from './components/modal/modal.component';
import { ProductTypePipe } from './pipes/product-type.pipe';
import { GateinDirectionPipe } from './pipes/gate-in-direction.pipe';
import { GatepassTypePipe } from './pipes/gatepass-type.pipe';
import { TransactionTypePipe } from './pipes/transaction-type.pipe';
import { BankPipe } from './pipes/bank.pipe';
import { LocalDatetimePipe } from './pipes/local-datetime.pipe';
import { TranslateModule } from '@ngx-translate/core';
import { LedgerTypePipe } from './pipes/ledger-type.pipe';
import { PaymentTypePipe } from './pipes/payment-type.pipe';
import { LocalCurrencyPipe } from './pipes/local-currency.pipe';
import { RateBasedOnPipe } from './pipes/rate-based-on.pipe';

@NgModule({
  declarations: [
    ModalComponent,
    ProductTypePipe,
    GateinDirectionPipe,
    GatepassTypePipe,
    TransactionTypePipe,
    BankPipe,
    LocalDatetimePipe,
    LedgerTypePipe,
    PaymentTypePipe,
    LocalCurrencyPipe,
    RateBasedOnPipe
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSidenavModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatDialogModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatRadioModule,
    MatCheckboxModule,
    MatDividerModule,
    TranslateModule,
    MatButtonToggleModule
  ],
  exports: [
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSidenavModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatDialogModule,
    ModalComponent,
    MatSelectModule,
    MatAutocompleteModule,
    MatRadioModule,
    ProductTypePipe,
    GateinDirectionPipe,
    GatepassTypePipe,
    MatCheckboxModule,
    MatDividerModule,
    BankPipe,
    TransactionTypePipe,
    LocalDatetimePipe,
    TranslateModule,
    LedgerTypePipe,
    PaymentTypePipe,
    LocalCurrencyPipe,
    MatButtonToggleModule,
    RateBasedOnPipe
  ],
  entryComponents: [ModalComponent]
})
export class SharedModule { }
