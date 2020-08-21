import { Party } from "./party.model";
import { RateBasedOn } from "./enums";

export class Ledger {
  id: number;
  date: string;
  amount: number;
  netWeight: number;
  balance: number;
  ledgerType: number;
  transactionType: number;
  transactionId: string;
  partyId: number;
  party: Party;
  product:string;
  boriQuantity:number;
  bagQuantity:number;
  totalMaund:number;
  additionalCharges:number;
  commission:number;
  gatepassIds:string;
  rateBasedOn:RateBasedOn; 
  rate:number; 
}
