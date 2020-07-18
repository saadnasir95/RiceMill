import { Pipe, PipeTransform } from '@angular/core';
import { LedgerType } from '../model/enums';

@Pipe({
  name: 'ledgerType'
})
export class LedgerTypePipe implements PipeTransform {

  transform(value: number) {
    switch (value) {
      case LedgerType.BankTransaction:
        return 'Bank Transaction';
      case LedgerType.Purchase:
        return 'Purchase';
      case LedgerType.Sale:
        return 'Sale';
    }
  }

}
