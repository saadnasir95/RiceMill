import { Company } from './company.model';
import { Vehicle } from './vehicle.model';
import { Product } from './product.model';

export class Gatepass {
  id: number;
  dateTime: string;
  type: number;
  broker: string;
  companyId: number;
  company: Company;
  vehicleId: number;
  vehicle: Vehicle;
  productId: number;
  product: Product;
  bagQuantity: number;
  boriQuantity: number;
  weightPerBag: number;
  netWeight: number;
  maund: number;
  saleId: number;
  purchaseId: number;
}
