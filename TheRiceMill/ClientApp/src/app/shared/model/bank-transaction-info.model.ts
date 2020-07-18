import { PaymentType } from './enums';

export class BankTransactionInfo {
  bank: string;
  accountNumber: string;
  chequeNumber: string;
  paymentType: PaymentType;
}
