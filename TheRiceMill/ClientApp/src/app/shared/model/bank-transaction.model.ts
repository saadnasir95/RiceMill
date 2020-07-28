import { Party } from './party.model';
import { BankAccount } from './bank-account.model';

export class BankTransaction {
  id: number;
  bankId: number;
  transactionAmount: number;
  transactionType: number;
  paymentType: number;
  bankAccountId: string;
  chequeNumber: string;
  transactionDate: string;
  partyId: number;
  party: Party;
  credit: number;
  debit: number;
  balance: number;
}
