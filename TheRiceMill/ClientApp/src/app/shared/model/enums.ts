export enum ProductType {
  Sale = 1,
  Purchase
}
export enum GateinDirection {
  Milling = 1,
  Stockpile,
  Outside
}

export enum GatepassType {
  Gateout = 1,
  Gatein
}

export enum TransactionType {
  Credit = 1,
  Debit,
}

export enum Bank {
  AskariBank = '1',
  AlliedBank = '2',
  BankAlfalah = '3',
  HabibBankLimited = '4',
  BankofPunjab = '5',
  UnitedBankLimited = '6',
  MeezanBank = '7',
  NationalBankofPakistan = '8',
  MCBBank = '9',
  StandardCharteredBank = '10'
}

export enum LedgerType {
  Sale = 1,
  Purchase,
  BankTransaction
}

export enum PaymentType {
  Cash = 1,
  Cheque = 2
}
