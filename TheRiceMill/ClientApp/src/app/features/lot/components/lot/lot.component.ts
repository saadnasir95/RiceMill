import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource, MatDialogRef, MatSort, MatDialog, MatPaginator } from '@angular/material';
import { Subscription } from 'rxjs';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import { LotModalComponent } from '../lot-modal/lot-modal.component';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { GatepassResponse } from '../../../../shared/model/gatepass-response.model';
import { NotificationService } from '../../../../shared/services/notification.service';
import { CompanyService } from '../../../../shared/services/company.service';
import { LotService } from '../../../../shared/services/lot.service';
import { LotResponse } from '../../../../shared/model/lot-response.model';
import { Lot } from '../../../../shared/model/lot.model';
import { StockIn } from '../../../../shared/model/stock-in.model';
import { ProcessedMaterial } from '../../../../shared/model/processed-material.model';

@Component({
  selector: 'app-lot',
  templateUrl: './lot.component.html',
  styleUrls: ['./lot.component.scss']
})
export class LotComponent implements OnInit, OnDestroy {

  // tslint:disable-next-line: max-line-length
  displayedColumns: string[] = ["id","gatepassTime","boriQuantity","bagQuantity","totalKG","action"];
  processedMaterialDisplayedColumns: string[] = ["id","productName","boriQuantity","bagQuantity","totalKG","action"];
  dataSource: MatTableDataSource<StockIn>;
  processedMaterialDataSource: MatTableDataSource<ProcessedMaterial>;

  lotList: Lot[];
  stockInsList: StockIn[];  
  processedMaterialList: ProcessedMaterial[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<LotModalComponent>;
  gatepassSubscription: Subscription;
  companySubscription: Subscription;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) processedMaterialSort: MatSort;
  @ViewChild(MatPaginator) processedMaterialPaginator: MatPaginator;

  companyId = 0;
  lotSearch = '';
  advanceSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  constructor(
    private lotService: LotService,
    private matDialog: MatDialog,
    private notificationService: NotificationService,
    private companyService: CompanyService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.processedMaterialDataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.processedMaterialPaginator.pageSize = 10;
    // this.getLotsList();
    this.gatepassSubscription = this.lotService.gatepassEmitter.subscribe(
      (data: any) => {
        this.paginator.pageIndex = 0;
        this.getLotsList();
      }
    );

    this.companySubscription = this.companyService.companySubject.subscribe(
      (companyId: number) => {
        if (this.companyId !== companyId) {
          this.companyId = companyId;
          this.paginator.pageIndex = 0;
          if(this.lotSearch){
            this.getLotsList();
          }
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
    this.lotSearch = filterValue.trim().toLowerCase();
    this.getLotsList();
  }
  
  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getLotsList();
  }
  
  changePage() {
    this.getLotsList();
  }

  addProcessedMaterial() {
    this.dialogRef = this.matDialog.open(LotModalComponent, {
      disableClose: true,
      width: '1400px',
      data: {
        lotId: this.lotSearch,
        lotYear: this.lotList[0].year
      }
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

  getLotsList() {
    this.lotService
      .getLotList(this.paginator.pageSize, this.paginator.pageIndex, this.advanceSearch, this.sortDirection, this.sortOrderBy, this.lotSearch)
      .subscribe(
        (response: LotResponse) => {
          this.lotList = response.data;
          this.stockInsList = response.data[0].stockIns;
          this.processedMaterialList = response.data[0].processedMaterials;
          this.dataSource.data = this.stockInsList;
          this.processedMaterialDataSource.data = this.processedMaterialList;
          this.processedMaterialPaginator.length = response.count;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }
}
