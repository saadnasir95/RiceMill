import { Party } from "./party.model";
import { RateBasedOn } from "./enums";

export class Ledger {
  id: number;
  date: string;
  amount: number;
  balance: number;
  ledgerType: number;
  transactionType: number;
  transactionId: string;
  partyId: number;
  party: Party;
  Product:string;
  BoriQuantity:number;
  BagQuantity:number;
  TotalMaund:number;
  AdditionalCharges:number;
  Commission:number;
  GatepassIds:string;
  RateBasedOn:RateBasedOn; 
  Rate:number; 
}
