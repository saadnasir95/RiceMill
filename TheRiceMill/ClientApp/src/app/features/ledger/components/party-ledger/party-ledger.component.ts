import { Component, OnInit, ViewChild } from '@angular/core';
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
export class PartyLedgerComponent implements OnInit {
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
  displayedColumns: string[] = ['createdDate', 'ledgerType', 'credit', 'debit', 'balance'];
  dataSource: MatTableDataSource<Ledger>;
  ledgerData: LedgerData;
  isLoadingData: Boolean = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    private ledgerService: LedgerService,
    private partyService: PartyService
  ) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 25;
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
