import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { CompanyType } from '../model/enums';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  companySubject = new Subject<number>();
  // 1 = AbRiceMill, 2=GD trading
  private companyId = 1;
  constructor() { }

  setCompanyId(companyType: CompanyType) {
    this.companyId = +companyType;
    this.companySubject.next(+companyType);
  }
  getCompanyId() {
    return this.companyId;
  }
}
