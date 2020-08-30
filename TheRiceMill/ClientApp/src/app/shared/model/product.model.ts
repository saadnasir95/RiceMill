import { Base } from './base.model';

export class Product extends Base {
  id: number;
  name: string;
  isProcessedMaterial: boolean;
  createdDate: string;
}

