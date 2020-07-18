import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource, MatDialogRef, MatPaginator, MatDialog } from '@angular/material';
import { BankTransaction } from '../../../../shared/model/bank-transaction.model';
import { BankTransactionModalComponent } from '../bank-transaction-modal/bank-transaction-modal.component';
import { Subscription } from 'rxjs';
import { BankTransactionResponse, BankTransactionData } from '../../../../shared/model/bank-transaction-response';
import { BankTransactionService } from '../../../../shared/services/bank-transaction.service';
import { BankAccountService } from '../../../../shared/services/bank-account.service';
import { BankAccountResponse } from '../../../../shared/model/bank-account-response.model';
import { BankAccount } from '../../../../shared/model/bank-account.model';

@Component({
  selector: 'app-bank-transaction',
  templateUrl: './bank-transaction.component.html',
  styleUrls: ['./bank-transaction.component.scss']
})
export class BankTransactionComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['transactionDate', 'company.Name', 'transactionType', 'paymentType', 'credit', 'debit', 'balance', 'Action'];
  dataSource: MatTableDataSource<BankTransaction>;
  transactionList: BankTransaction[];
  bankAccountList: BankAccount[];
  selectedBankAccountId = 0;
  transactionData: BankTransactionData;
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<BankTransactionModalComponent>;
  transactionSubscription: Subscription;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    private bankService: BankTransactionService,
    private matDialog: MatDialog,
    private bankAccountService: BankAccountService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.bankAccountService.getBankAccounts(100, 0).subscribe(
      (response: BankAccountResponse) => {
        this.bankAccountList = response.data;
        if (this.bankAccountList.length > 0) {
          this.selectedBankAccountId = this.bankAccountList[0].id;
          this.getTransactionList();
        }
      }
    );
    this.transactionSubscription = this.bankService.transactionEmitter.subscribe(
      () => {
        this.paginator.pageIndex = 0;
        this.getTransactionList();
      }
    );
  }
  onBankAccountChange() {
    this.getTransactionList();
  }
  ngOnDestroy() {
    this.transactionSubscription.unsubscribe();
  }

  changePage() {
    this.getTransactionList();
  }
  openModal() {
    this.dialogRef = this.matDialog.open(BankTransactionModalComponent, {
      disableClose: true,
      width: '800px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editTransaction(transaction: BankTransaction) {
    this.dialogRef = this.matDialog.open(BankTransactionModalComponent, {
      disableClose: true,
      width: '800px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editTransaction(transaction);
  }
  deleteTransaction(transaction: BankTransaction) {
    this.dialogRef = this.matDialog.open(BankTransactionModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteTransaction(transaction);
  }
  getTransactionList() {
    if (this.selectedBankAccountId !== 0) {
      this.bankService
        .getBankTransactions(this.selectedBankAccountId, this.paginator.pageSize, this.paginator.pageIndex)
        .subscribe(
          (response: BankTransactionResponse) => {
            this.transactionData = response.data;
            let previousBalance: number = this.transactionData.previousBalance != null ? this.transactionData.previousBalance : 0;
            this.transactionData.transactions.forEach(element => {
              if (element.credit !== 0) {
                previousBalance += element.credit;
                element.balance = previousBalance;
                element.transactionAmount = element.credit;
              } else {
                previousBalance -= element.debit;
                element.balance = previousBalance;
                element.transactionAmount = element.debit;
              }
            });
            this.dataSource.data = this.transactionData.transactions;
            this.paginator.length = response.count;
          },
          (error) => console.log(error)
        );
    }
  }


}
