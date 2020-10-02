import { Party } from './party.model';
import { Vehicle } from './vehicle.model';
import { Product } from './product.model';
import { Base } from './base.model';
import { Purchase } from './purchase.model';
import { Sale } from './sale.model';

export class Gatepass extends Base {
  id: number;
  dateTime: string;
  type: number;
  broker: string;
  partyId: number;
  party: Party;
  vehicleId: number;
  vehicle: Vehicle;
  productId: number;
  product: Product;
  bagQuantity: number;
  boriQuantity: number;
  weightPerBag: number;
  kandaWeight: number;
  emptyWeight: number;
  netWeight: number;
  maund: number;
  saleId: number;
  purchaseId: number;
  biltyNumber: string;
  lotId: number;
  lotYear: number;
  purchase: Purchase;
  sale: Sale;
}
