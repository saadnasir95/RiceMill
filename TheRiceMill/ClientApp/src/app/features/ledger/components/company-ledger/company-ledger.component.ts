import { Component, OnInit, ViewChild } from '@angular/core';
import { Moment } from 'moment';
import moment = require('moment');
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { FormGroup, FormControl } from '@angular/forms';
import { trigger, state, transition, animate, style } from '@angular/animations';
import { LedgerType } from '../../../../shared/model/enums';
import { Ledger } from '../../../../shared/model/ledger.model';
import { LedgerData, LedgerResponse } from '../../../../shared/model/ledger-response.model';
import { BankTransactionInfo } from '../../../../shared/model/bank-transaction-info.model';
import { LedgerInfo } from '../../../../shared/model/ledger-info.model';
import { LedgerService } from '../../../../shared/services/ledger.service';
import { GridOptions } from 'ag-grid-community';


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
export class CompanyLedgerComponent implements OnInit {
  displayedColumns: string[] = ['createdDate', 'ledgerType', 'credit', 'debit', 'balance','product','gatepassIds','boriQuantity',
  'bagQuantity','totalMaund','rate','rateBasedOn','commission'];
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

  constructor(private ledgerService: LedgerService) { }

  ngOnInit() {
    this.gridOptions = {
      rowData: [],
      columnDefs: [
      {
        headerName: "Created Date",
        field: 'date', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "Description",
        field: 'ledgerType', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "Credit",
        field: 'amount', 
        sortable: true, 
        filter: true 
      },
      {
        headerName: "Debit",
        field: 'amount', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "Balance",
        field: 'balance', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "Product",
        field: 'product', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "GatepassIds",
        field: 'gatepassIds', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "Bori Quantity",
        field: 'boriQuantity', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "Bag Quantity",   
        field: 'bagQuantity', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "Total Maund",
        field: 'totalMaund', 
        sortable: true, 
        filter: true 
      },
      {
        headerName: "Rate",
        field: 'rate', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "Rate BasedOn",   
        field: 'rateBasedOn', 
        sortable: true, 
        filter: true
      },
      {
        headerName: "Commission",
        field: 'commission', 
        sortable: true, 
        filter: true
      }],
      onGridReady: () => {
        this.getLedgerList();
      },
      rowSelection: 'multiple',
      rowGroupPanelShow: 'always',
      pivotPanelShow: 'always',
      enableRangeSelection: true    
    }
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 25;
    this.buildForm();
    // this.getLedgerList();
  }

  onPaginationChanged(params){
    console.log(params)
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
          this.ledgerData.ledgerResponses.forEach(element => {
            // if (element.credit !== 0) {
            //   previousBalance += element.credit;
            //   element.balance = previousBalance;
            // } else {
            //   previousBalance -= element.debit;
            //   element.balance = previousBalance;
            // }
            previousBalance += element.amount;
            element.balance = previousBalance;
          });
          this.gridOptions.api.setRowData(this.ledgerData.ledgerResponses);
          this.dataSource.data = this.ledgerData.ledgerResponses;
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

