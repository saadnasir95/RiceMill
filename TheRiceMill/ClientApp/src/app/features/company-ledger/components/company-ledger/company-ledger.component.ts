import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { CompanyLedgerService } from '../../../../shared/services/company-ledger.service';
import { CompanyLedger } from '../../../../shared/model/company-ledger.model';
import { CompanyLedgerResponse, CompanyLedgerData } from '../../../../shared/model/company-ledger-response.model';
import { Company } from '../../../../shared/model/company.model';
import { CompanyResponse } from '../../../../shared/model/company-response.model';
import { CompanyService } from '../../../../shared/services/company.service';
import { trigger, state, transition, animate, style } from '@angular/animations';
import { LedgerType } from '../../../../shared/model/enums';
import { LedgerInfo } from '../../../../shared/model/ledger-info.model';
import { BankTransactionInfo } from '../../../../shared/model/bank-transaction-info.model';

@Component({
  selector: 'app-company-ledger',
  templateUrl: './company-ledger.component.html',
  styleUrls: ['./company-ledger.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class CompanyLedgerComponent implements OnInit {
  expandedTransactionId = 0;
  expandedLedgerType: LedgerType = LedgerType.Purchase;
  salePurchaseInfo = 0;
  ledgerInfo: LedgerInfo = {
    product: '',
    totalActualBagWeight: 0,
    totalMaund: 0,
    maundPrice: 0
  };
  bankTransactionInfo: BankTransactionInfo = {
    bank: '',
    accountNumber: '',
    chequeNumber: '',
    paymentType: 1
  };
  companyList: Company[];
  selectedCompanyID = 0;
  displayedColumns: string[] = ['createdDate', 'ledgerType', 'credit', 'debit', 'balance'];
  dataSource: MatTableDataSource<CompanyLedger>;
  ledgerData: CompanyLedgerData;
  isLoadingData: Boolean = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private ledgerService: CompanyLedgerService, private companyService: CompanyService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 25;
    this.companyService.getCompanies(100, 0).subscribe(
      (response: CompanyResponse) => {
        this.companyList = response.data;
        if (this.companyList.length > 0) {
          this.selectedCompanyID = this.companyList[0].id;
          this.getLedgerList();
        }
      }
    );
  }
  onCompanyChange() {
    this.getLedgerList();
  }

  changePage() {
    this.getLedgerList();
  }
  getLedgerList() {
    if (this.selectedCompanyID !== 0) {
      this.ledgerService
        .getCompanyLedger(this.selectedCompanyID, this.paginator.pageSize, this.paginator.pageIndex)
        .subscribe(
          (response: CompanyLedgerResponse) => {
            this.ledgerData = response.data;
            let previousBalance: number = this.ledgerData.previousBalance != null ? this.ledgerData.previousBalance : 0;
            this.ledgerData.ledgerResponses.forEach(element => {
              if (element.credit !== 0) {
                previousBalance += element.credit;
                element.balance = previousBalance;
              } else {
                previousBalance -= element.debit;
                element.balance = previousBalance;
              }
            });
            this.dataSource.data = this.ledgerData.ledgerResponses;
            this.paginator.length = response.count;
          },
          (error) => console.log(error)
        );
    }

  }
  getLedgerInfo(ledger: CompanyLedger) {
    if (this.expandedTransactionId === ledger.transactionId && this.expandedLedgerType === ledger.ledgerType) {
      this.expandedTransactionId = 0;
      this.expandedLedgerType = LedgerType.Purchase;
      this.salePurchaseInfo = 0;
    } else {
      if (ledger.ledgerType === LedgerType.Purchase || ledger.ledgerType === LedgerType.Sale) {
        this.salePurchaseInfo = 1;
      } else {
        this.salePurchaseInfo = 2;
      }
      this.expandedTransactionId = ledger.transactionId;
      this.expandedLedgerType = ledger.ledgerType;
      this.ledgerService.getLedgerDetails(ledger.ledgerType, ledger.transactionId)
        .subscribe(
          (response: any) => {
            if (this.salePurchaseInfo === 1) {
              this.ledgerInfo = response.data as LedgerInfo;
            } else {
              this.bankTransactionInfo = response.data as BankTransactionInfo;
            }

          },
          (error) => console.log(error)
        );
    }
  }

}
