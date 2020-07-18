import { Pipe, PipeTransform } from '@angular/core';
import { Bank } from '../model/enums';

@Pipe({
  name: 'bank'
})
export class BankPipe implements PipeTransform {

  transform(value: any) {
    switch (value) {
      case Bank.AskariBank:
        return 'Askari Bank';
      case Bank.BankAlfalah:
        return 'Bank Alfalah';
      case Bank.BankofPunjab:
        return 'Bank of Punjab';
      case Bank.AlliedBank:
        return 'Allied Bank';
      case Bank.HabibBankLimited:
        return 'HBL';
      case Bank.MCBBank:
        return 'MCB';
      case Bank.MeezanBank:
        return 'Meezan Bank';
      case Bank.NationalBankofPakistan:
        return 'NBP';
      case Bank.StandardCharteredBank:
        return 'Standard Chartered';
      case Bank.UnitedBankLimited:
        return 'UBL';
    }
  }

}
