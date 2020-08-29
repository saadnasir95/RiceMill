import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource, MatDialogRef, MatSort, MatDialog, MatPaginator } from '@angular/material';
import { Subscription } from 'rxjs';
import { Sale } from '../../../../shared/model/sale.model';
import { SaleModalComponent } from '../sale-modal/sale-modal.component';
import { SaleService } from '../../../../shared/services/sale.service';
import { SaleResponse } from '../../../../shared/model/sale-response.model';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SaleReceiptComponent } from '../sale-receipt/sale-receipt.component';
import { CompanyService } from '../../../../shared/services/company.service';

@Component({
  selector: 'app-sale',
  templateUrl: './sale.component.html',
  styleUrls: ['./sale.component.scss']
})
export class SaleComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['Id', 'createdDate', 'totalMaund', 'boriQuantity', 'bagQuantity', 'brokery','freight', 'rate', 'totalPrice', 'totalGatepasses', 'Action'];
  dataSource: MatTableDataSource<Sale>;
  saleList: Sale[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<SaleModalComponent>;
  saleSubscription: Subscription;
  companySubscription: Subscription;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(SaleReceiptComponent) saleReceiptComponent: SaleReceiptComponent;
  saleSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  companyId = 0;
  constructor(
    private saleService: SaleService,
    private matDialog: MatDialog,
    private notificationService: NotificationService,
    private companyService: CompanyService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getSaleList();
    this.saleSubscription = this.saleService.saleEmitter.subscribe(
      (data) => {
        this.paginator.pageIndex = 0;
        this.getSaleList();
        if (data instanceof Object) {
          this.printSale(data);
        }
      }
    );
    this.companySubscription = this.companyService.companySubject.subscribe(
      (companyId: number) => {
        if (this.companyId !== companyId) {
          this.companyId = companyId;
          this.paginator.pageIndex = 0;
          this.getSaleList();
        }
      }
    );
  }

  ngOnDestroy() {
    if (this.saleSubscription) {
      this.saleSubscription.unsubscribe();
    }
    if (this.companySubscription) {
      this.companySubscription.unsubscribe();
    }
  }

  applyFilter(filterValue: string) {
    this.paginator.pageIndex = 0;
    this.saleSearch = filterValue.trim().toLowerCase();
    this.getSaleList();
  }

  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getSaleList();
  }

  changePage() {
  }

  openModal() {
    this.dialogRef = this.matDialog.open(SaleModalComponent, {
      disableClose: true,
      width: '1400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }

  editSale(sale: Sale) {
    this.dialogRef = this.matDialog.open(SaleModalComponent, {
      disableClose: true,
      width: '1400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editSale(sale);
  }

  printSale(sale: Sale) {
    this.saleReceiptComponent.sale = sale;
    setTimeout(() => {
      this.notificationService.closeNotification();
      window.print();
    }, 500);
  }

  deleteSale(sale: Sale) {
    this.dialogRef = this.matDialog.open(SaleModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteSale(sale);
  }

  getSaleList() {
    this.saleService
      .getSaleList(this.paginator.pageSize, this.paginator.pageIndex, this.saleSearch, this.sortDirection, this.sortOrderBy)
      .subscribe(
        (response: SaleResponse) => {
          this.saleList = response.data;
          this.dataSource.data = this.saleList;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }

}
