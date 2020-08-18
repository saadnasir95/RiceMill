import { Pipe, PipeTransform } from '@angular/core';
import { CompanyType } from '../model/enums';

@Pipe({
  name: 'companyType'
})
export class CompanyTypePipe implements PipeTransform {

  transform(value: number) {
    switch (value) {
      case CompanyType.ABRiceMill:
        return 'AB Rice Mill';
      case CompanyType.GDTrading:
        return 'Gd Trading';
    }
  }

}
