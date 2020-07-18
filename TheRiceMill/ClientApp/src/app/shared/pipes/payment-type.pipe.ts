import { Pipe, PipeTransform } from '@angular/core';
import { PaymentType } from '../model/enums';

@Pipe({
  name: 'paymentType'
})
export class PaymentTypePipe implements PipeTransform {

  transform(value: number) {
    switch (value) {
      case PaymentType.Cash:
        return 'Cash';
      case PaymentType.Cheque:
        return 'Cheque';
    }
  }

}
