import { Party } from './party.model';
import { Vehicle } from './vehicle.model';
import { Product } from './product.model';
import { AdditionalCharges } from './additionalcharges.model';

export class Sale {
  id: number;
  checkOut: string;
  partyId: number;
  party: Party;
  vehicleId: number;
  vehicle: Vehicle;
  productId: number;
  product: Product;
  bagQuantity: number;
  bagWeight: number;
  expectedBagWeight: number;
  totalExpectedBagWeight: number;
  kandaWeight: number;
  expectedEmptyBagWeight: number;
  totalExpectedEmptyBagWeight: number;
  actualBagWeight: number;
  totalActualBagWeight: number;
  totalMaund: number;
  ratePerKg: number;
  ratePerMaund: number;
  basePrice: number;
  totalPrice: number;
  biltyNumber: string;
  commission: number;
  percentCommission: number;
  additionalCharges: AdditionalCharges[];
}
