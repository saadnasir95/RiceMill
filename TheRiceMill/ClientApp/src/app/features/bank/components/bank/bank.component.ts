import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogRef } from '@angular/material';
import { Subscription } from 'rxjs';
import { Bank } from '../../../../shared/model/bank.model';
import { BankModalComponent } from '../bank-modal/bank-modal.component';
import { BankService } from '../../../../shared/services/bank.service';
import { BankResponse } from '../../../../shared/model/bank-response.model';

@Component({
  selector: 'app-bank',
  templateUrl: './bank.component.html',
  styleUrls: ['./bank.component.scss']
})
export class BankComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['Id', 'Name', 'Action'];
  dataSource: MatTableDataSource<Bank>;
  banks: Bank[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<BankModalComponent>;
  bankSubscription: Subscription;
  bankSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private bankService: BankService, private matDialog: MatDialog) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getBanks();
    this.bankSubscription = this.bankService.bankEmitter.subscribe(
      () => {
        this.paginator.pageIndex = 0;
        this.getBanks();
      }
    );
  }

  ngOnDestroy() {
    this.bankSubscription.unsubscribe();
  }

  applyFilter(filterValue: string) {
    this.bankSearch = filterValue.trim().toLowerCase();
    this.paginator.pageIndex = 0;
    this.getBanks();
  }
  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getBanks();
  }
  changePage() {
    this.getBanks();
  }
  addBank() {
    this.dialogRef = this.matDialog.open(BankModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editBank(bank: Bank) {
    this.dialogRef = this.matDialog.open(BankModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editBank(bank);
  }
  deleteBank(bank: Bank) {
    this.dialogRef = this.matDialog.open(BankModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteBank(bank);
  }
  getBanks() {
    this.bankService
      .getBanks(this.paginator.pageSize, this.paginator.pageIndex, this.bankSearch, this.sortDirection, this.sortOrderBy)
      .subscribe(
        (response: BankResponse) => {
          this.banks = response.data;
          this.dataSource.data = this.banks;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }
}
