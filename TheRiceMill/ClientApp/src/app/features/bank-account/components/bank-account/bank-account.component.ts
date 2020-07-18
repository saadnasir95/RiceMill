import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogRef } from '@angular/material';
import { Subscription } from 'rxjs';
import { BankAccount } from '../../../../shared/model/bank-account.model';
import { BankAccountModalComponent } from '../bank-account-modal/bank-account-modal.component';
import { BankAccountService } from '../../../../shared/services/bank-account.service';
import { BankAccountResponse } from '../../../../shared/model/bank-account-response.model';

@Component({
  selector: 'app-bank-account',
  templateUrl: './bank-account.component.html',
  styleUrls: ['./bank-account.component.scss']
})
export class BankAccountComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['bankId', 'accountNumber', 'currentBalance', 'Action'];
  dataSource: MatTableDataSource<BankAccount>;
  bankAccounts: BankAccount[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<BankAccountModalComponent>;
  bankAccountSubscription: Subscription;
  bankAccountSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private bankAccountService: BankAccountService, private matDialog: MatDialog) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getBankAccounts();
    this.bankAccountSubscription = this.bankAccountService.bankAccountEmitter.subscribe(
      () => {
        this.paginator.pageIndex = 0;
        this.getBankAccounts();
      }
    );
  }

  ngOnDestroy() {
    this.bankAccountSubscription.unsubscribe();
  }

  applyFilter(filterValue: string) {
    this.bankAccountSearch = filterValue.trim().toLowerCase();
    this.paginator.pageIndex = 0;
    this.getBankAccounts();
  }
  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getBankAccounts();
  }
  changePage() {
    this.getBankAccounts();
  }
  addBankAccount() {
    this.dialogRef = this.matDialog.open(BankAccountModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editBankAccount(bankAccount: BankAccount) {
    this.dialogRef = this.matDialog.open(BankAccountModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editBankAccount(bankAccount);
  }
  deleteBankAccount(bankAccount: BankAccount) {
    this.dialogRef = this.matDialog.open(BankAccountModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteBankAccount(bankAccount);
  }
  getBankAccounts() {
    this.bankAccountService
      .getBankAccounts(this.paginator.pageSize, this.paginator.pageIndex, this.bankAccountSearch, this.sortDirection, this.sortOrderBy)
      .subscribe(
        (response: BankAccountResponse) => {
          this.bankAccounts = response.data;
          this.dataSource.data = this.bankAccounts;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }
}
