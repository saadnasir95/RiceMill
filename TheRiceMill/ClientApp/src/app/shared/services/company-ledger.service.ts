import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TokenService } from './token.service';
import { CompanyLedgerResponse } from '../model/company-ledger-response.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CompanyLedgerService {
  apiUrl = environment.baseUrl + '/api/v1/Ledger';
  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getCompanyLedger(companyId: number, pageSize: number, pageIndex: number)
    : Observable<CompanyLedgerResponse> {
    const params = new HttpParams()
      .set('CompanyId', companyId.toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString());
    return this.http.get<CompanyLedgerResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
  getLedgerDetails(ledgerType: number, transactionId: number)
    : Observable<any> {
    const params = new HttpParams()
      .set('TransactionId', transactionId.toString())
      .set('LedgerType', ledgerType.toString());
    return this.http.get<any>(this.apiUrl + '/info', { headers: this.tokenService.getHeaders(), params: params });
  }
}
