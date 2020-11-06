import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Subscription } from 'rxjs';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { Voucher } from '../../../../shared/model/voucher.model';
import { CompanyService } from '../../../../shared/services/company.service';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import { NotificationService } from '../../../../shared/services/notification.service';
import { VoucherService } from '../../../../shared/services/voucher.service';
import { VoucherModalComponent } from '../voucher-modal/voucher-modal.component';

@Component({
  selector: 'app-voucher',
  templateUrl: './voucher.component.html',
  styleUrls: ['./voucher.component.scss']
})
export class VoucherComponent implements OnInit, OnDestroy {

  // tslint:disable-next-line: max-line-length
  displayedColumns: string[] = ['Id', 'Type', 'Party.Name', 'Vehicle.PlateNo', 'Product.Name', 'BagQuantity', 'BoriQuantity', 'WeightPerBag', 'NetWeight', 'Maund', 'DateTime', 'Action'];
  dataSource: MatTableDataSource<Voucher>;
  voucherList: Voucher[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<VoucherModalComponent>;
  voucherSubscription: Subscription;
  companySubscription: Subscription;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  // @ViewChild(GatepassReceiptComponent) gatepassReceiptComponent: GatepassReceiptComponent;
  companyId = 0;
  voucherSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  constructor(
    private voucherService: VoucherService,
    private matDialog: MatDialog,
    private notificationService: NotificationService,
    private companyService: CompanyService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getVoucherList();
    this.voucherSubscription = this.voucherService.voucherEmitter.subscribe(
      (data: any) => {
        this.paginator.pageIndex = 0;
        this.getVoucherList();
        if (data instanceof Object) {
          this.printVoucher(data);
        }
      }
    );
    this.companySubscription = this.companyService.companySubject.subscribe(
      (companyId: number) => {
        if (this.companyId !== companyId) {
          this.companyId = companyId;
          this.paginator.pageIndex = 0;
          this.getVoucherList();
        }
      }
    );
  }

  ngOnDestroy() {
    if (this.voucherSubscription) {
      this.voucherSubscription.unsubscribe();
    }
    if (this.companySubscription) {
      this.companySubscription.unsubscribe();
    }
  }

  applyFilter(filterValue: string) {
    this.paginator.pageIndex = 0;
    this.voucherSearch = filterValue.trim().toLowerCase();
    this.getVoucherList();
  }
  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getVoucherList();
  }
  changePage() {
    this.getVoucherList();
  }
  openModal() {
    this.dialogRef = this.matDialog.open(VoucherModalComponent, {
      disableClose: true,
      width: '1400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editVoucher(voucher: Voucher) {
    this.dialogRef = this.matDialog.open(VoucherModalComponent, {
      disableClose: true,
      width: '1400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editVoucher(voucher);
  }

  printVoucher(voucher: Voucher) {
    // this.gatepassReceiptComponent.gatepass = gatepass;
    // setTimeout(() => {
    //   this.notificationService.closeNotification();
    //   window.print();
    // }, 500);
  }

  deleteVoucher(voucher: Voucher) {
    this.dialogRef = this.matDialog.open(VoucherModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteVoucher(voucher);
  }
  getVoucherList() {
    // this.voucherService
    //   .getGatepassList(this.paginator.pageSize, this.paginator.pageIndex, this.voucherSearch, this.sortDirection, this.sortOrderBy)
    //   .subscribe(
    //     (response: GatepassResponse) => {
    //       this.gatepassList = response.data;
          // this.dataSource.data = this.voucherList;
    //       this.paginator.length = response.count;
    //     },
    //     (error) => console.log(error)
    //   );
  }


}
