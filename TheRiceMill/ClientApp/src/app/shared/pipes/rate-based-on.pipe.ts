import { Pipe, PipeTransform } from '@angular/core';
import { RateBasedOn } from '../model/enums';

@Pipe({
  name: 'rateBasedOn'
})
export class RateBasedOnPipe implements PipeTransform {

  transform(value: any): any {
    switch (value) {
      case RateBasedOn.Maund:
        return 'Maund';
      case RateBasedOn.Bag:
        return 'Bag';
      case RateBasedOn.Bori:
        return 'Bori';
    }
  }

}
