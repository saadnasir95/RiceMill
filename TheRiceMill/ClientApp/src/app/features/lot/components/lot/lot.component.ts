import { Component, OnInit, OnDestroy, ViewChild, TemplateRef, ChangeDetectorRef } from '@angular/core';
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
import { ProcessedMaterial } from '../../../../shared/model/processed-material.model';
import { GridOptions } from 'ag-grid-community';
import { LocalDatetimePipe } from '../../../../shared/pipes/local-datetime.pipe';
import { TemplateRendererComponent } from '../../../../shared/components/template-renderer/template-renderer.component';
import { RateCostModalComponent } from '../rate-cost-modal/rate-cost-modalcomponent';
import { RateCost } from '../../../../shared/model/create-rate-cost.model';
import { LotReceiptComponent } from '../lot-receipt/lot-receipt.component';
import { Stock } from '../../../../shared/model/stock-in.model';
import { timeStamp } from 'console';
import { Balance } from '../../../../shared/model/balance.model';

@Component({
  selector: 'app-lot',
  templateUrl: './lot.component.html',
  styleUrls: ['./lot.component.scss']
})
export class LotComponent implements OnInit, OnDestroy {
  @ViewChild('actionBtn') actionBtn: TemplateRef<any>;
  @ViewChild('rateCostActionBtn') rateCostActionBtn: TemplateRef<any>;
  @ViewChild(LotReceiptComponent) lotReceiptComponent: LotReceiptComponent;

  // tslint:disable-next-line: max-line-length
  displayedColumns: string[] = ['id', 'gatepassTime', 'boriQuantity', 'bagQuantity', 'totalKG', 'action'];
  processedMaterialDisplayedColumns: string[] = ['id', 'productName', 'boriQuantity', 'bagQuantity', 'totalKG', 'action'];
  stockOutColumns: string[] = ['id', 'gatepassTime', 'boriQuantity', 'bagQuantity', 'totalKG', 'action'];

  dataSource: MatTableDataSource<Stock>;
  processedMaterialDataSource: MatTableDataSource<ProcessedMaterial>;
  stockOutDataSource: MatTableDataSource<Stock>;

  stockInGridOptions: GridOptions;
  processedMaterialGridOptions: GridOptions;
  stockOutGridOptions: GridOptions;
  balanceOutGridOptions: GridOptions;
  rateCostGridOptions: GridOptions;

  lotYearIds: Array<string>;
  selectedYear = '';
  lot: Lot;
  stockInsList: Array<Stock>;
  stockOutsList: Stock[];
  processedMaterialList: ProcessedMaterial[];
  balanceList: Array<Balance> = [];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<LotModalComponent>;
  dialogRefRateCost: MatDialogRef<RateCostModalComponent>;

  gatepassSubscription: Subscription;
  companySubscription: Subscription;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) processedMaterialSort: MatSort;
  @ViewChild(MatPaginator) processedMaterialPaginator: MatPaginator;
  @ViewChild(MatSort) stockOutSort: MatSort;
  @ViewChild(MatPaginator) stockOutPaginator: MatPaginator;

  companyId = 0;
  lotIdSearch = '';
  advanceSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  constructor(
    private lotService: LotService,
    private matDialog: MatDialog,
    private notificationService: NotificationService,
    private companyService: CompanyService,
    private cdf: ChangeDetectorRef) { }

  ngOnInit() {
    this.getYears();
    this.dataSource = new MatTableDataSource();
    this.processedMaterialDataSource = new MatTableDataSource();
    this.stockOutDataSource = new MatTableDataSource();

    this.stockInGridOptions = {
      rowData: [],
      columnDefs: [
        {
          headerName: 'Id',
          field: 'id',
          width: 80
        },
        {
          headerName: 'Date',
          field: 'gatepassTime',
          valueFormatter: this.datePipe,
          width: 200
        },
        {
          headerName: 'Bori Quantity',
          field: 'boriQuantity',
          width: 200
        },
        {
          headerName: 'Bag Quantity',
          field: 'bagQuantity',
          width: 200
        },
        {
          headerName: 'Total KG',
          field: 'totalKG',
          width: 100
        },
        // {
        //   headerName: 'Add Processed Material',
        //   field: 'actionButton',
        //   cellRendererFramework: TemplateRendererComponent,
        //   cellRendererParams: {
        //     ngTemplate: this.actionBtn,
        //     width: 100
        //   }
        // },
      ],
      onGridReady: () => { },
      rowSelection: 'multiple',
      rowGroupPanelShow: 'always',
      pivotPanelShow: 'always',
      enableRangeSelection: true,
      defaultColDef: {
        resizable: true,
        filter: true
      }
    };

    this.processedMaterialGridOptions = {
      rowData: [],
      columnDefs: [
        {
          headerName: 'Id',
          field: 'id',
          hide: true,
          width: 80
        },
        {
          headerName: 'Item',
          field: 'product.name',
          width: 150
        },
        {
          headerName: 'Bori Qty',
          field: 'boriQuantity',
          width: 90
        },
        {
          headerName: 'Bag Qty',
          field: 'bagQuantity',
          width: 90
        },
        {
          headerName: 'Total KG',
          field: 'totalKG',
          width: 90
        },
        // {
        //   headerName: 'Action',
        //   field: '',
        // },
      ],
      onGridReady: () => { },
      rowSelection: 'multiple',
      rowGroupPanelShow: 'always',
      pivotPanelShow: 'always',
      enableRangeSelection: true,
      defaultColDef: {
        resizable: true,
        filter: true
      }
    };

    this.stockOutGridOptions = {
      rowData: [],
      columnDefs: [
        {
          headerName: 'Id',
          field: 'id',
          hide: true
          // width: 80
        },
        {
          headerName: 'Date',
          field: 'createdDate',
          valueFormatter: this.datePipe,
          hide: true
          // width: 80
        },
        {
          headerName: 'Item',
          field: 'product.name',
          width: 150
        },
        {
          headerName: 'Bori Qty',
          field: 'boriQuantity',
          width: 90
        },
        {
          headerName: 'Bag Qty',
          field: 'bagQuantity',
          width: 90        
        },
        {
          headerName: 'Total KG',
          field: 'totalKG',
          width: 90        
        },
      ],
      onGridReady: () => { },
      rowSelection: 'multiple',
      rowGroupPanelShow: 'always',
      pivotPanelShow: 'always',
      enableRangeSelection: true,
      defaultColDef: {
        resizable: true,
        filter: true
      }
    };

    this.balanceOutGridOptions = {
      rowData: [],
      columnDefs: [
        {
          headerName: 'Bori/Bags',
          field: 'bagQuantity',
          width: 75
        },
        {
          headerName: 'Bori/Bags',
          field: 'BoriQuantity',
          width: 75,
          hide: true
        },
        {
          headerName: 'KGS.',
          field: 'totalKG',
          width: 75
        },
      ],
      onGridReady: () => { },
      rowSelection: 'multiple',
      rowGroupPanelShow: 'always',
      pivotPanelShow: 'always',
      enableRangeSelection: true,
      defaultColDef: {
        resizable: true,
        filter: true
      }
    };

    this.rateCostGridOptions = {
      rowData: [],
      columnDefs: [
        {
          headerName: 'Id',
          field: 'id',
          hide: true
        },
        {
          headerName: 'Labour Unloading & Loading',
          field: 'labourUnloadingAndLoading',
          // width: 80
        },
        {
          headerName: 'Freight',
          field: 'freight',
          // width: 80
        },
        {
          headerName: 'Purchase Brokery',
          field: 'purchaseBrokery',
          // width: 100
        },
        {
          headerName: 'Total',
          field: 'total',
          // width: 100
        },
        {
          headerName: 'Rate/40 without Processing',
          field: 'ratePer40WithoutProcessing',
          // width: 100
        },
        {
          headerName: 'Processing Expense',
          field: 'processingExpense',
          // width: 100
        },
        {
          headerName: 'Bardana/Misc.',
          field: 'bardanaMisc',
          // width: 100
        },
        {
          headerName: 'Grand Total',
          field: 'grandTotal',
          // width: 100
        },
        {
          headerName: 'Rate/40 Less By Product',
          field: 'ratePer40LessByProduct',
        },
        {
          headerName: 'Sale Brokery',
          field: 'saleBrockery',
        },
        {
          headerName: 'Edit RateCost',
          field: 'rateCostActionBtn',
          cellRendererFramework: TemplateRendererComponent,
          cellRendererParams: {
            ngTemplate: this.rateCostActionBtn,
            width: 100
          }
        }
      ],
      onGridReady: () => { },
      rowSelection: 'multiple',
      rowGroupPanelShow: 'always',
      pivotPanelShow: 'always',
      enableRangeSelection: true,
      defaultColDef: {
        resizable: true,
        filter: true
      }
    };



    this.paginator.pageSize = 10;
    this.processedMaterialPaginator.pageSize = 10;
    this.stockOutPaginator.pageSize = 10;
    // this.getLotsList();
    this.gatepassSubscription = this.lotService.lotEmitter.subscribe(
      (data: any) => {
        this.paginator.pageIndex = 0;
        this.getLotHistory();
      }
    );

    this.companySubscription = this.companyService.companySubject.subscribe(
      (companyId: number) => {
        if (this.companyId !== companyId) {
          this.companyId = companyId;
          this.paginator.pageIndex = 0;
          if (this.lotIdSearch) {
            this.getLotHistory();
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

  printLotHistory() {
    this.lot.balances = this.balanceList;
    this.lotReceiptComponent.lot = this.lot;
    setTimeout(() => {
      this.notificationService.closeNotification();    
      var head = document.head || document.getElementsByTagName('head')[0];
      var style =  document.createElement('style');
      style.type = 'text/css';
      style.media = 'print';
    
      style.appendChild(document.createTextNode('@page { size: A4 landscape;}'));    
      head.appendChild(style);
      window.print();
    }, 500);
  }

  applyFilter(filterValue: string) {
    this.paginator.pageIndex = 0;
    this.lotIdSearch = filterValue.trim().toLowerCase();
    // this.getLotHistory();
  }

  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getLotHistory();
  }

  changePage() {
    this.getLotHistory();
  }

  addProcessedMaterial() {
    this.dialogRef = this.matDialog.open(LotModalComponent, {
      disableClose: true,
      width: '1400px',
      data: {
        lotId: this.lot.id,
        lotYear: this.lot.year
      }
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.populateLotData(this.lot.id, +this.lot.year, this.processedMaterialList);
  }

  openRateCostModal() {
    this.dialogRefRateCost = this.matDialog.open(RateCostModalComponent, {
      disableClose: true,
      width: '1400px',
      data: {
        lotId: this.lot.id,
        lotYear: this.lot.year
      }
    });
    this.dialogRefRateCost.componentInstance.modalRef = this.dialogRefRateCost;
  }

  editRateCost(rateCost: RateCost) {
    this.dialogRefRateCost = this.matDialog.open(RateCostModalComponent, {
      disableClose: true,
      width: '1400px',
      data: {
        lotId: this.lot.id,
        lotYear: this.lot.year
      }
    });
    this.dialogRefRateCost.componentInstance.modalRef = this.dialogRefRateCost;
    this.dialogRefRateCost.componentInstance.editRateCost(rateCost);
  }

  deleteGatepass(gatepass: Gatepass) {
    // this.dialogRef = this.matDialog.open(LotModalComponent, {
    //   disableClose: true,
    //   width: '400px'
    // });
    // this.dialogRef.componentInstance.modalRef = this.dialogRef;
    // this.dialogRef.componentInstance.deleteGatepass(gatepass);
  }

  getLotHistory() {
    if (!this.lotIdSearch || !this.selectedYear) {
      return;
    }

    this.lotService
      .getLot(this.paginator.pageSize, this.paginator.pageIndex, this.advanceSearch, this.sortDirection, this.sortOrderBy, this.lotIdSearch, this.selectedYear)
      .subscribe(
        (response: LotResponse) => {
          if (response.data) {
            this.lot = response.data;
            this.stockInsList = response.data.stockIns;
            this.stockOutsList = response.data.stockOuts;
            this.processedMaterialList = response.data.processedMaterials;
            this.dataSource.data = this.stockInsList;
            this.stockOutDataSource.data = this.stockOutsList;
            this.processedMaterialDataSource.data = this.processedMaterialList;

            debugger
            this.processedMaterialList.length > 0 && this.processedMaterialList.forEach((item,index) => {
              let balance = new Balance();
              if(this.stockOutsList[index].bagQuantity > 0){
                balance.bagQuantity = item.bagQuantity - this.stockOutsList[index].bagQuantity;
                balance.totalKG = item.totalKG - this.stockOutsList[index].totalKG;
              }
              this.balanceList.push(balance);
            })
            this.balanceOutGridOptions.api.setRowData(this.balanceList);
            // AG GRID
            this.stockInGridOptions.api.setRowData(this.stockInsList);
            this.stockOutGridOptions.api.setRowData(this.stockOutsList);
            this.processedMaterialGridOptions.api.setRowData(this.processedMaterialList);
            this.rateCostGridOptions.api.setRowData(response.data.rateCosts);
            
            this.calculateSum(this.stockInsList, this.stockInGridOptions);
            this.calculateSum(this.stockOutsList, this.stockOutGridOptions);
            this.calculateSum(this.processedMaterialList, this.processedMaterialGridOptions);
            this.calculateSum(this.balanceList, this.balanceOutGridOptions);
          } else {
            this.lot = new Lot();
            this.stockInsList = [];
            this.stockOutsList = [];
            this.processedMaterialList = [];
            this.dataSource.data = [];
            this.stockOutDataSource.data = [];
            this.processedMaterialDataSource.data = [];
            // AG GRID
            this.stockInGridOptions.api.setRowData([]);
            this.stockOutGridOptions.api.setRowData([]);
            this.processedMaterialGridOptions.api.setRowData([]);
            this.rateCostGridOptions.api.setRowData([]);
            this.calculateSum([], this.stockInGridOptions);
            this.calculateSum([], this.stockOutGridOptions);
            this.calculateSum([], this.processedMaterialGridOptions);
          }

          this.processedMaterialPaginator.length = response.count;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }

  getYears() {
    this.lotService
      .getYears()
      .subscribe(
        (response: any) => {
          if (response.data) {
            this.lotYearIds = response.data as Array<string>;
          }
        });
  }

  calculateSum(list: Array<any>, gridInstance: GridOptions) {
    let _bagQuantity = 0;
    let _boriQuantity = 0;
    let _totalKG = 0;
    list.forEach(item => {
      _bagQuantity += item.bagQuantity;
      _boriQuantity += item.boriQuantity;
      _totalKG += item.totalKG;
    });

    const result = [{
      boriQuantity: _boriQuantity.toFixed(2),
      bagQuantity: _bagQuantity.toFixed(2),
      totalKG: _totalKG.toFixed(2),
      actionButton: 'footer'
    }];

    gridInstance.api.setPinnedBottomRowData(result);

  }

  datePipe(date: any) {
    if (date.value) {
      return new LocalDatetimePipe().transform(date.value);
    }
  }

}
