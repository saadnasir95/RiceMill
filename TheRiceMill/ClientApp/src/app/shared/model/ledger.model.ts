import { Party } from "./party.model";

export class Ledger {
  id: number;
  createdDate: string;
  amount: number;
  balance: number;
  ledgerType: number;
  transactionType: number;
  transactionId: string;
  partyId: number;
  party: Party;
}
