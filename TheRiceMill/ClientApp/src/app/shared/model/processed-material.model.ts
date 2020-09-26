import { Product } from './product.model';

export class ProcessedMaterial {
  id: number;
  productId: number;
  product: Product;
  boriQuantity: number;
  bagQuantity: number;
  perKG: number;
  totalKG: number;
}

export class CreateProcessedMaterial {
  lotId: number;
  lotYear: number;
  companyId: number;
  processedMaterials: Array<ProcessedMaterial>;
}

