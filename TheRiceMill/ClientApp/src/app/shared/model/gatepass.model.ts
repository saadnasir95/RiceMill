import { Company } from './company.model';
import { Vehicle } from './vehicle.model';
import { Product } from './product.model';

export class Gatepass {
  id: number;
  checkDateTime: string;
  type: number;
  direction: number;
  biltyNumber: number;
  companyId: number;
  company: Company;
  vehicleId: number;
  vehicle: Vehicle;
  productId: number;
  product: Product;
  bagQuantity: number;
  bagWeight: number;
  kandaWeight: number;
  totalMaund: number;
}
