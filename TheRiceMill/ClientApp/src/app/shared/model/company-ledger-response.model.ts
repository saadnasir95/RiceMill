import { CompanyLedger } from './company-ledger.model';

export class CompanyLedgerResponse {
  count: number;
  data: CompanyLedgerData;
}

export class CompanyLedgerData {
  ledgerResponses: CompanyLedger[];
  totalCredit: number;
  totalDebit: number;
  previousBalance: number;
}
