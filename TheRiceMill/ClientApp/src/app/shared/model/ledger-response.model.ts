import { Ledger } from './ledger.model';

export class LedgerResponse {
  count: number;
  data: LedgerData;
}

export class LedgerData {
  ledgerResponses: Ledger[];
  netBalance: number;
  previousBalance: number;
}
