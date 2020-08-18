import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource, MatDialogRef, MatSort, MatDialog, MatPaginator } from '@angular/material';
import { Subscription } from 'rxjs';
import { PurchaseModalComponent } from '../purchase-modal/purchase-modal.component';
import { Purchase } from '../../../../shared/model/purchase.model';
import { PurchaseService } from '../../../../shared/services/purchase.service';
import { PurchaseResponse } from '../../../../shared/model/purchase-response.model';
import { PurchaseReceiptComponent } from '../purchase-receipt/purchase-receipt.component';
import { NotificationService } from '../../../../shared/services/notification.service';
import { CompanyService } from '../../../../shared/services/company.service';

@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.scss']
})
export class PurchaseComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['Id', 'createdDate', 'totalMaund', 'boriQuantity', 'bagQuantity', 'brokery', 'rate', 'totalPrice', 'totalGatepasses', 'Action'];
  dataSource: MatTableDataSource<Purchase>;
  purchaseList: Purchase[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<PurchaseModalComponent>;
  purchaseSubscription: Subscription;
  companySubscription: Subscription;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(PurchaseReceiptComponent) purchaseReceiptComponent: PurchaseReceiptComponent;
  purchaseSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  companyId = 0;
  constructor(
    private purchaseService: PurchaseService,
    private matDialog: MatDialog,
    private notificationService: NotificationService,
    private companyService: CompanyService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getPurchaseList();
    this.purchaseSubscription = this.purchaseService.purchaseEmitter.subscribe(
      (data) => {
        this.paginator.pageIndex = 0;
        this.getPurchaseList();
        if (data instanceof Object) {
          this.printPurchase(data);
        }
      }
    );
    this.companySubscription = this.companyService.companySubject.subscribe(
      (companyId: number) => {
        if (this.companyId !== companyId) {
          this.companyId = companyId;
          this.paginator.pageIndex = 0;
          this.getPurchaseList();
        }
      }
    );
  }

  ngOnDestroy() {
    if (this.purchaseSubscription) {
      this.purchaseSubscription.unsubscribe();
    }
    if (this.companySubscription) {
      this.companySubscription.unsubscribe();
    }
  }

  applyFilter(filterValue: string) {
    this.paginator.pageIndex = 0;
    this.purchaseSearch = filterValue.trim().toLowerCase();
    this.getPurchaseList();
  }

  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getPurchaseList();
  }

  changePage() {
    this.getPurchaseList();
  }

  openModal() {
    this.dialogRef = this.matDialog.open(PurchaseModalComponent, {
      disableClose: true,
      width: '1400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }

  editPurchase(purchase: Purchase) {
    this.dialogRef = this.matDialog.open(PurchaseModalComponent, {
      disableClose: true,
      width: '1400px'
    });

    const _purchase = JSON.parse(JSON.stringify(purchase));
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editPurchase(_purchase);
  }
  printPurchase(purchase: Purchase) {
    this.purchaseReceiptComponent.purchase = purchase;
    setTimeout(() => {
      this.notificationService.closeNotification();
      window.print();
    }, 500);
  }
  deletePurchase(purchase: Purchase) {
    this.dialogRef = this.matDialog.open(PurchaseModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deletePurchase(purchase);
  }

  getPurchaseList() {
    this.purchaseService
      .getPurchaseList(this.paginator.pageSize, this.paginator.pageIndex, this.purchaseSearch, this.sortDirection, this.sortOrderBy)
      .subscribe(
        (response: PurchaseResponse) => {
          this.purchaseList = response.data;
          this.dataSource.data = this.purchaseList;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }

}
