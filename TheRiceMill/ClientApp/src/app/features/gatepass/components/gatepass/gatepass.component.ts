import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource, MatDialogRef, MatSort, MatDialog, MatPaginator } from '@angular/material';
import { Subscription } from 'rxjs';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import { GatepassModalComponent } from '../gatepass-modal/gatepass-modal.component';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { GatepassResponse } from '../../../../shared/model/gatepass-response.model';
import { GatepassReceiptComponent } from '../gatepass-receipt/gatepass-receipt.component';
import { NotificationService } from '../../../../shared/services/notification.service';

@Component({
  selector: 'app-gatepass',
  templateUrl: './gatepass.component.html',
  styleUrls: ['./gatepass.component.scss']
})
export class GatepassComponent implements OnInit, OnDestroy {

  // tslint:disable-next-line: max-line-length
  displayedColumns: string[] = ['Id', 'Type', 'Party.Name', 'Vehicle.PlateNo', 'Product.Name', 'BagQuantity', 'BoriQuantity', 'WeightPerBag', 'NetWeight', 'Maund', 'DateTime', 'Action'];
  dataSource: MatTableDataSource<Gatepass>;
  gatepassList: Gatepass[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<GatepassModalComponent>;
  gatepassSubscription: Subscription;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(GatepassReceiptComponent) gatepassReceiptComponent: GatepassReceiptComponent;
  gatepassSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  constructor(
    private gatepassService: GatepassService,
    private matDialog: MatDialog,
    private notificationService: NotificationService) { }

  ngOnInit() {

    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getGatepassList();
    this.gatepassSubscription = this.gatepassService.gatepassEmitter.subscribe(
      (data: any) => {
        this.paginator.pageIndex = 0;
        this.getGatepassList();
        if (data instanceof Object) {
          this.printGatepass(data);
        }
      }
    );
  }

  ngOnDestroy() {
    this.gatepassSubscription.unsubscribe();
  }

  applyFilter(filterValue: string) {
    this.paginator.pageIndex = 0;
    this.gatepassSearch = filterValue.trim().toLowerCase();
    this.getGatepassList();
  }
  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getGatepassList();
  }
  changePage() {
    this.getGatepassList();
  }
  openModal() {
    this.dialogRef = this.matDialog.open(GatepassModalComponent, {
      disableClose: true,
      width: '1400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editGatepass(gatepass: Gatepass) {
    this.dialogRef = this.matDialog.open(GatepassModalComponent, {
      disableClose: true,
      width: '1400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editGatepass(gatepass);
  }
  printGatepass(gatepass: Gatepass) {
    this.gatepassReceiptComponent.gatepass = gatepass;
    setTimeout(() => {
      this.notificationService.closeNotification();
      window.print();
    }, 500);
  }
  deleteGatepass(gatepass: Gatepass) {
    this.dialogRef = this.matDialog.open(GatepassModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteGatepass(gatepass);
  }
  getGatepassList() {
    this.gatepassService
      .getGatepassList(this.paginator.pageSize, this.paginator.pageIndex, this.gatepassSearch, this.sortDirection, this.sortOrderBy)
      .subscribe(
        (response: GatepassResponse) => {
          this.gatepassList = response.data;
          this.dataSource.data = this.gatepassList;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }


}
