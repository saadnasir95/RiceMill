import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Moment } from 'moment';
import moment = require('moment');
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { FormGroup, FormControl } from '@angular/forms';
import { trigger, state, transition, animate, style } from '@angular/animations';
import { LedgerType, RateBasedOn } from '../../../../shared/model/enums';
import { Ledger } from '../../../../shared/model/ledger.model';
import { LedgerData, LedgerResponse } from '../../../../shared/model/ledger-response.model';
import { BankTransactionInfo } from '../../../../shared/model/bank-transaction-info.model';
import { LedgerInfo } from '../../../../shared/model/ledger-info.model';
import { LedgerService } from '../../../../shared/services/ledger.service';
import { GridOptions } from 'ag-grid-community';
import { Subscription } from 'rxjs';
import { CompanyService } from '../../../../shared/services/company.service';
import { LocalCurrencyPipe } from '../../../../shared/pipes/local-currency.pipe';
import { LocalDatetimePipe } from '../../../../shared/pipes/local-datetime.pipe';
import { RateBasedOnPipe } from '../../../../shared/pipes/rate-based-on.pipe';


@Component({
  selector: 'app-company-ledger',
  templateUrl: './company-ledger.component.html',
  styleUrls: ['./company-ledger.component.scss'], animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class CompanyLedgerComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['createdDate', 'ledgerType', 'credit', 'debit', 'balance', 'product', 'gatepassIds', 'boriQuantity',
    'bagQuantity', 'totalMaund', 'rate', 'rateBasedOn', 'commission'];
  selected: { start: Moment, end: Moment };
  expandedLedgerType: LedgerType = LedgerType.Purchase;
  ledgerType = [{ id: '0', value: 'All' }, { id: '1', value: 'Sale' }, { id: '2', value: 'Purchase' }];
  dataSource: MatTableDataSource<Ledger>;
  ledgerData: LedgerData;
  isLoadingData: Boolean = false;
  ledgerForm: FormGroup;
  salePurchaseInfo = 0;
  ledgerInfo: LedgerInfo = null;
  gridOptions: GridOptions;
  bankTransactionInfo: BankTransactionInfo = {
    bank: '',
    accountNumber: '',
    chequeNumber: '',
    paymentType: 1
  };
  startDate: string;
  endDate: string;
  isLoading = false;
  expandedId = 0;
  companyId = 0;
  companySubscription: Subscription;
  ranges: any = {
    'Today': [moment(), moment()],
    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
    'This Month': [moment().startOf('month'), moment().endOf('month')],
    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
  };

  locale = {
    format: 'MM/DD/YYYY',
    direction: 'ltr', // could be rtl
    weekLabel: 'W',
    separator: ' To ', // default is ' - '
    cancelLabel: 'Cancel', // detault is 'Cancel'
    applyLabel: 'Okay', // detault is 'Apply'
    clearLabel: 'Clear', // detault is 'Clear'
    customRangeLabel: 'Custom range',
    daysOfWeek: moment.weekdaysMin(),
    monthNames: moment.monthsShort(),
    firstDay: 1 // first day is monday
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private ledgerService: LedgerService,
    private companyService: CompanyService) { }

  ngOnInit() {
    this.gridOptions = {
      rowData: [],
      columnDefs: [
        {
          headerName: 'Created Date',
          field: 'date',
          valueFormatter: this.datePipe
        },
        {
          headerName: 'Description',
          field: 'ledgerType',
        },
        {
          headerName: 'Product',
          field: 'product',
        },
        {
          headerName: 'GatepassIds',
          field: 'gatepassIds',
          width: 120,
        },
        {
          headerName: 'Party Name',
          field: 'party.name',
        },
        {
          headerName: 'Broker',
          field: 'broker',
        },
        {
          headerName: 'Lot No.',
          field: 'lotNumber',
        },
        {
          headerName: 'Inv No.',
          field: 'invoiceId',
        },
        {
          headerName: 'Vehicle No.',
          field: 'vehicleNo',
        },
        {
          headerName: 'Bilty Number',
          field: 'biltyNumber',
        },
        {
          headerName: 'Bori Quantity',
          field: 'boriQuantity',
          width: 120,
        },
        {
          headerName: 'Bag Quantity',
          field: 'bagQuantity',
          width: 120,
        },
        {
          headerName: 'Net Weight',
          field: 'netWeight',
        },
        {
          headerName: 'Total Maund',
          field: 'totalMaund',
          width: 120,
        },
        {
          headerName: 'Rate',
          field: 'rate',
          width: 100,
          valueFormatter: this.currencyPipe
        },
        {
          headerName: 'Freight',
          field: 'freight',
          width: 100,
          valueFormatter: this.currencyPipe
        },
        {
          headerName: 'Rate BasedOn',
          field: 'rateBasedOn',
          width: 120,
          valueFormatter: this.rateBasedOnPipe
        },
        {
          headerName: 'Brokery',
          field: 'commission',
          width: 120,
          valueFormatter: this.currencyPipe
        },
        {
          headerName: 'Credit',
          field: 'credit',
          width: 100,
          valueFormatter: this.currencyPipe
        },
        {
          headerName: 'Debit',
          field: 'debit',
          width: 100,
          valueFormatter: this.currencyPipe
        },
        {
          headerName: 'Balance',
          field: 'balance',
          width: 100,
          valueFormatter: this.currencyPipe
        },
      ],
      onGridReady: () => {
        this.getLedgerList();
      },
      rowSelection: 'multiple',
      rowGroupPanelShow: 'always',
      pivotPanelShow: 'always',
      enableRangeSelection: true,
      defaultColDef: {
        resizable: true,
        filter: true
      }
    };
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 25;
    this.buildForm();
    this.companySubscription = this.companyService.companySubject.subscribe(
      (companyId: number) => {
        if (this.companyId !== companyId) {
          this.companyId = companyId;
          this.paginator.pageIndex = 0;
          this.getLedgerList();
        }
      }
    );
  }
  
  rateBasedOnPipe(data: any){
    if(data.value){
      return new RateBasedOnPipe().transform(data.value);
    }
  }

  datePipe(date: any){
    if(date.value){
      return new LocalDatetimePipe().transform(date.value);
    }
  }

  currencyPipe(data: any){
    if(data.value){
      return new LocalCurrencyPipe().transform(data.value)
    }
  }
  
  ngOnDestroy() {
    if (this.companySubscription) {
      this.companySubscription.unsubscribe();
    }
  }
  onPaginationChanged(params) {
    console.log(params);
  }

  buildForm() {
    this.ledgerForm = new FormGroup({
      ledgerType: new FormControl('0'),
      date: new FormControl()
    });

    this.ledgerForm.get('ledgerType').valueChanges.subscribe(response => {
      if (response) {
        this.getLedgerList();
      }
    });

    this.ledgerForm.get('date').valueChanges.subscribe(response => {
      this.startDate = response.start ? moment(response.start).utc().format() : null;
      this.endDate = response.end ? moment(response.end).utc().format() : null;
      this.getLedgerList();
    });


  }

  changePage() {
    this.getLedgerList();
  }

  getLedgerList() {
    this.ledgerService
      .getCompanyLedger(this.paginator.pageSize, this.paginator.pageIndex,
        this.ledgerForm.get('ledgerType').value,
        this.startDate, this.endDate)
      .subscribe(
        (response: LedgerResponse) => {
          this.ledgerData = response.data;
          let previousBalance: number = this.ledgerData.previousBalance != null ? this.ledgerData.previousBalance : 0;
          let _credit = 0;
            let _debit = 0;
            let _balance = 0;
            let _bagQuantity = 0;
            let _boriQuantity = 0;
            let _brokery = 0;
            let _netWeight = 0;
            let _totalMaund = 0;
            
            const modifiedLedgerData = this.ledgerData.ledgerResponses.map(ledger => {
              previousBalance += ledger.amount;
              ledger.balance = previousBalance;
              _bagQuantity += ledger.bagQuantity;
              _boriQuantity += ledger.boriQuantity;
              _netWeight += ledger.netWeight;
              _brokery += ledger.commission;
              _totalMaund += ledger.totalMaund;
              if(ledger.amount > 0){
                ledger['credit'] = ledger.amount; 
                ledger['debit'] = '' ;
                _credit += ledger.amount;
                _balance = +ledger.balance;
              } else {
                ledger['debit'] = ledger.amount; 
                ledger['credit'] = '' ;
                _debit += ledger.amount;
                _balance = +ledger.balance;
              }
              return ledger
            })
            this.gridOptions.api.setRowData(modifiedLedgerData);

            const result = [{
              date: "",
              ledgerType:"",
              product:"",
              gatepassIds:"",
              name:"",
              broker:"",
              lotNumber:"",
              invoiceId:"",
              vehicleNo: "",
              biltyNumber:"",
              boriQuantity:_bagQuantity.toFixed(2),
              bagQuantity:_boriQuantity.toFixed(2),
              totalMaund:_totalMaund.toFixed(2),
              netWeight: _netWeight.toFixed(2),
              rate:"",
              rateBasedOn: "",
              brokery: _brokery.toFixed(2),
              debit: _debit == 0 ? '' : _debit,
              credit: _credit == 0 ? '' : _credit,
              balance: _balance,
            }];
            this.gridOptions.api.setPinnedBottomRowData(result);
          this.dataSource.data = modifiedLedgerData;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }

  getLedgerInfo(ledger: Ledger) {
    if (this.expandedId === ledger.id && this.expandedLedgerType === ledger.ledgerType) {
      this.expandedId = 0;
      this.expandedLedgerType = LedgerType.Purchase;
      this.salePurchaseInfo = 0;
    } else {
      this.isLoading = true;
      if (ledger.ledgerType === LedgerType.Purchase || ledger.ledgerType === LedgerType.Sale) {
        this.salePurchaseInfo = 1;
      } else {
        this.salePurchaseInfo = 2;
      }
      this.expandedId = ledger.id;
      this.expandedLedgerType = ledger.ledgerType;
      this.ledgerService.getLedgerDetails(ledger.ledgerType, ledger.id)
        .subscribe(
          (response: any) => {
            if (this.salePurchaseInfo === 1) {
              this.ledgerInfo = response.data as LedgerInfo;
            } else {
              this.bankTransactionInfo = response.data as BankTransactionInfo;
            }
            this.isLoading = false;
          },
          (error) => {
            console.log(error);
            this.isLoading = false;
          }
        );
    }
  }

}

