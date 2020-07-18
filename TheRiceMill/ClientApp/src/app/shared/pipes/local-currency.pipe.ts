import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'localCurrency'
})
export class LocalCurrencyPipe implements PipeTransform {

  transform(value: number) {
    if (value >= 0) {
      return `Rs ${value.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
    } else {
      return `Rs ${value.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
    }
  }

}
