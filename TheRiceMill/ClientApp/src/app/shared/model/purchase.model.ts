import { Company } from './company.model';
import { Vehicle } from './vehicle.model';
import { Product } from './product.model';
import { AdditionalCharges } from './additionalcharges.model';

export class Purchase {
  id: number;
  checkIn: string;
  direction: number;
  companyId: number;
  company: Company;
  vehicleId: number;
  vehicle: Vehicle;
  productId: number;
  product: Product;
  bagQuantity: number;
  expectedBagWeight: number;
  totalExpectedBagWeight: number;
  kandaWeight: number;
  expectedEmptyBagWeight: number;
  totalExpectedEmptyBagWeight: number;
  vibration: number;
  actualBagWeight: number;
  totalActualBagWeight: number;
  totalMaund: number;
  ratePerKg: number;
  ratePerMaund: number;
  basePrice: number;
  totalPrice: number;
  actualBags: number;
  bagWeight: number;
  percentCommission: number;
  commission: number;
  additionalCharges: AdditionalCharges[];
}
