import { Pipe, PipeTransform } from '@angular/core';
import { TransactionType } from '../model/enums';

@Pipe({
  name: 'transactionType'
})
export class TransactionTypePipe implements PipeTransform {

  transform(value: number) {
    switch (value) {
      case TransactionType.Credit:
        return 'Credit';
      case TransactionType.Debit:
        return 'Debit';
    }
  }

}
