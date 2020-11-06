import { BankAccount } from './bank-account.model';
import { Party } from './party.model';
import { Purchase } from './purchase.model';
import { Sale } from './sale.model';

export class VoucherDetail {
  partyId: number;
  party: Party;
  saleId: number;
  sale: Sale;
  purhaseId: number;
  purhcase: Purchase;
  debit: number;
  credit: number;
  remarks: string;
  bankAccountId: number;
  bankAccount: BankAccount;
}
