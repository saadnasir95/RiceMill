import { Pipe, PipeTransform } from '@angular/core';
import { SaleType } from '../model/enums';

@Pipe({
  name: 'saleType'
})
export class SaleTypePipe implements PipeTransform {

  transform(value: number) {
    switch (value) {
      case SaleType.Sale:
        return 'Sale';
      case SaleType.Gift:
        return 'Gift';
      case SaleType.Welfare:
        return 'Welfare';
    }
  }
}
