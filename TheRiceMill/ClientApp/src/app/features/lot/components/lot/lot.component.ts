import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource, MatDialogRef, MatSort, MatDialog, MatPaginator } from '@angular/material';
import { Subscription } from 'rxjs';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import { LotModalComponent } from '../lot-modal/lot-modal.component';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { GatepassResponse } from '../../../../shared/model/gatepass-response.model';
import { NotificationService } from '../../../../shared/services/notification.service';
import { CompanyService } from '../../../../shared/services/company.service';

@Component({
  selector: 'app-lot',
  templateUrl: './lot.component.html',
  styleUrls: ['./lot.component.scss']
})
export class LotComponent implements OnInit, OnDestroy {

  // tslint:disable-next-line: max-line-length
  displayedColumns: string[] = ['Id', 'Type', 'Party.Name', 'Vehicle.PlateNo', 'Product.Name', 'BagQuantity', 'BoriQuantity', 'WeightPerBag', 'NetWeight', 'Maund', 'DateTime', 'Action'];
  dataSource: MatTableDataSource<Gatepass>;
  gatepassList: Gatepass[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<LotModalComponent>;
  gatepassSubscription: Subscription;
  companySubscription: Subscription;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  companyId = 0;
  gatepassSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  constructor(
    private gatepassService: GatepassService,
    private matDialog: MatDialog,
    private notificationService: NotificationService,
    private companyService: CompanyService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getGatepassList();
    this.gatepassSubscription = this.gatepassService.gatepassEmitter.subscribe(
      (data: any) => {
        this.paginator.pageIndex = 0;
        this.getGatepassList();
      }
    );
    this.companySubscription = this.companyService.companySubject.subscribe(
      (companyId: number) => {
        if (this.companyId !== companyId) {
          this.companyId = companyId;
          this.paginator.pageIndex = 0;
          this.getGatepassList();
        }
      }
    );
  }

  ngOnDestroy() {
    if (this.gatepassSubscription) {
      this.gatepassSubscription.unsubscribe();
    }
    if (this.companySubscription) {
      this.companySubscription.unsubscribe();
    }
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
    this.dialogRef = this.matDialog.open(LotModalComponent, {
      disableClose: true,
      width: '1400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editGatepass(gatepass: Gatepass) {
    // this.dialogRef = this.matDialog.open(LotModalComponent, {
    //   disableClose: true,
    //   width: '1400px'
    // });
    // this.dialogRef.componentInstance.modalRef = this.dialogRef;
    // this.dialogRef.componentInstance.editGatepass(gatepass);
  }

  deleteGatepass(gatepass: Gatepass) {
    // this.dialogRef = this.matDialog.open(LotModalComponent, {
    //   disableClose: true,
    //   width: '400px'
    // });
    // this.dialogRef.componentInstance.modalRef = this.dialogRef;
    // this.dialogRef.componentInstance.deleteGatepass(gatepass);
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