import { PipeTransform, Pipe } from '@angular/core';
import { ProductType } from '../model/enums';

@Pipe({
  name: 'productType'
})
export class ProductTypePipe implements PipeTransform {
  transform(value: number) {
    switch (value) {
      case ProductType.Purchase:
        return 'Purchase';
      case ProductType.Sale:
        return 'Sale';
    }
  }
}
