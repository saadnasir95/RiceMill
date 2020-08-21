import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { Party } from '../../../../shared/model/party.model';
import { trigger, state, transition, animate, style } from '@angular/animations';
import { LedgerType } from '../../../../shared/model/enums';
import { LedgerInfo } from '../../../../shared/model/ledger-info.model';
import { BankTransactionInfo } from '../../../../shared/model/bank-transaction-info.model';
import { PartyService } from '../../../../shared/services/party.service';
import { PartyResponse } from '../../../../shared/model/party-response.model';
import { Ledger } from '../../../../shared/model/ledger.model';
import { LedgerData, LedgerResponse } from '../../../../shared/model/ledger-response.model';
import { LedgerService } from '../../../../shared/services/ledger.service';
import { GridOptions } from 'ag-grid-community';
import { CompanyService } from '../../../../shared/services/company.service';
import { Subscription } from 'rxjs';
import { LocalDatetimePipe } from '../../../../shared/pipes/local-datetime.pipe';
import { LocalCurrencyPipe } from '../../../../shared/pipes/local-currency.pipe';
import { RateBasedOnPipe } from '../../../../shared/pipes/rate-based-on.pipe';
import { LedgerTypePipe } from '../../../../shared/pipes/ledger-type.pipe';

@Component({
  selector: 'app-party-ledger',
  templateUrl: './party-ledger.component.html',
  styleUrls: ['./party-ledger.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class PartyLedgerComponent implements OnInit, OnDestroy {
  isLoading = false;
  expandedId = 0;
  expandedLedgerType: LedgerType = LedgerType.Purchase;
  salePurchaseInfo = 0;
  ledgerInfo: LedgerInfo = null;
  bankTransactionInfo: BankTransactionInfo = {
    bank: '',
    accountNumber: '',
    chequeNumber: '',
    paymentType: 1
  };
  partyList: Party[];
  selectedPartyId = 0;
  displayedColumns: string[] = ['createdDate', 'ledgerType', 'credit', 'debit', 'balance', 'product', 'gatepassIds', 'boriQuantity',
    'bagQuantity', 'totalMaund', 'rate', 'rateBasedOn', 'commission'];
  dataSource: MatTableDataSource<Ledger>;
  gridOptions: GridOptions;
  ledgerData: LedgerData;
  isLoadingData: Boolean = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  companyId = 0;
  companySubscription: Subscription;

  constructor(
    private ledgerService: LedgerService,
    private partyService: PartyService,
    private companyService: CompanyService
  ) {   }

  ngOnInit() {
    this.gridOptions = {
      rowData: [],
      columnDefs: [
        {
          headerName: 'Created Date',
          field: 'date',
          sortable: true,
          filter: true,
          valueFormatter: this.datePipe
        },
        {
          headerName: 'Description',
          field: 'ledgerType',
          sortable: true,
          filter: true
        },
        {
          headerName: 'Product',
          field: 'product',
          sortable: true,
          filter: true
        },
        {
          headerName: 'GatepassIds',
          field: 'gatepassIds',
          width: 120,
          sortable: true,
          filter: true
        },
        {
          headerName: 'Party Name',
          field: 'party.name',
          sortable: true,
          filter: true
        },
        {
          headerName: 'Broker',
          field: 'broker',
          sortable: true,
          filter: true
        },
        {
          headerName: 'Lot No.',
          field: 'lotNumber',
          sortable: true,
          filter: true
        },
        {
          headerName: 'Inv No.',
          field: 'invoiceId',
          sortable: true,
          filter: true
        },
        {
          headerName: 'Vehicle No.',
          field: 'vehicleNo',
          sortable: true,
          filter: true
        },
        {
          headerName: 'Bilty Number',
          field: 'biltyNumber',
          sortable: true,
          filter: true
        },
        {
          headerName: 'Bori Quantity',
          field: 'boriQuantity',
          width: 120,
          sortable: true,
          filter: true
        },
        {
          headerName: 'Bag Quantity',
          field: 'bagQuantity',
          width: 120,
          sortable: true,
          filter: true
        },
        {
          headerName: 'Net Weight',
          field: 'netWeight',
          sortable: true,
          filter: true
        },
        {
          headerName: 'Total Maund',
          field: 'totalMaund',
          width: 120,
          sortable: true,
          filter: true
        },
        {
          headerName: 'Rate',
          field: 'rate',
          sortable: true,
          width: 100,
          filter: true,
          valueFormatter: this.currencyPipe
        },
        {
          headerName: 'Rate BasedOn',
          field: 'rateBasedOn',
          width: 120,
          sortable: true,
          filter: true,
          valueFormatter: this.rateBasedOnPipe
        },
        {
          headerName: 'Commission',
          field: 'commission',
          width: 120,
          sortable: true,
          filter: true,
          valueFormatter: this.currencyPipe
        },
        {
          headerName: 'Credit',
          field: 'amount',
          width: 100,
          sortable: true,
          filter: true,
          valueFormatter: this.currencyPipe
        },
        {
          headerName: 'Debit',
          field: 'amount',
          width: 100,
          sortable: true,
          filter: true,
          valueFormatter: this.currencyPipe
        },
        {
          headerName: 'Balance',
          field: 'balance',
          width: 100,
          sortable: true,
          filter: true,
          valueFormatter: this.currencyPipe
        },
      ],
      onGridReady: () => {
        this.getParties();
      },
      rowSelection: 'multiple',
      rowGroupPanelShow: 'always',
      pivotPanelShow: 'always',
      enableRangeSelection: true,
    };
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 25;
    this.companySubscription = this.companyService.companySubject.subscribe(
      (companyId: number) => {
        if (this.companyId !== companyId) {
          this.companyId = companyId;
          this.paginator.pageIndex = 0;
          this.getParties();
        }
      }
    );
  }

  ledgerTypePipe(data: any){
    if(data.value){
      return new LedgerTypePipe().transform(data.value);
    }
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
  getParties() {
    this.partyService.getParties(100, 0).subscribe(
      (response: PartyResponse) => {
        this.partyList = response.data;
        if (this.partyList.length > 0) {
          this.selectedPartyId = this.partyList[0].id;
          this.getLedgerList();
        }
      }
    );
  }

  onPartyChange() {
    this.getLedgerList();
  }

  changePage() {
    this.getLedgerList();
  }

  getLedgerList() {
    if (this.selectedPartyId !== 0) {
      this.ledgerService
        .getPartyLedger(this.selectedPartyId, this.paginator.pageSize, this.paginator.pageIndex)
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
            let _credit = 0;
            let _debit = 0;
            let _balance = 0;
            let _bagQuantity = 0;
            let _boriQuantity = 0;
            let _netWeight = 0;
            let _totalMaund = 0;
            
            this.ledgerData.ledgerResponses.forEach(ledger => {
              debugger
              _credit += ledger.amount;
              _debit += ledger.amount;
              _balance += ledger.balance;
              _bagQuantity += ledger.bagQuantity;
              _boriQuantity += ledger.boriQuantity;
              _netWeight += ledger.netWeight;
              _totalMaund += ledger.totalMaund;

            })
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
              boriQuantity:_bagQuantity,
              bagQuantity:_boriQuantity,
              totalMaund:_totalMaund,
              netWeight: _netWeight,
              rate:"",
              rateBasedOn: "",
              commission:"",
              amount: _credit,
              // : _debit,
              balance: _balance,
            }];
            this.gridOptions.api.setPinnedBottomRowData(result);
            this.dataSource.data = this.ledgerData.ledgerResponses;
            this.paginator.length = response.count;
          },
          (error) => console.log(error)
        );
    }

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
