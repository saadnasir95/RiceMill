import { Company } from './company.model';
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
  companyId: number;
  company: Company;
  credit: number;
  debit: number;
  balance: number;
}
