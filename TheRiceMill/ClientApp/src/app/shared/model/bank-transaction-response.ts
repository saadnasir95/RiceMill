import { BankTransaction } from './bank-transaction.model';

export class BankTransactionResponse {
    count: number;
    data: BankTransactionData;
}

export class BankTransactionData {
  transactions: BankTransaction[];
  netBalance: number;
  previousBalance: number;
}
