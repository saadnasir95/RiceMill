import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TokenService } from './token.service';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LedgerResponse } from '../model/ledger-response.model';

@Injectable({
  providedIn: 'root'
})
export class LedgerService {
  apiUrl = environment.baseUrl + '/api/v1/Ledger';
  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getPartyLedger(partyId: number, pageSize: number, pageIndex: number)
    : Observable<LedgerResponse> {
    const params = new HttpParams()
      .set('PartyId', partyId.toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString());
    return this.http.get<LedgerResponse>(this.apiUrl + '/GetPartyLedger', { headers: this.tokenService.getHeaders(), params: params });
  }


  getCompanyLedger(pageSize: number, pageIndex: number, ledgerType: number, fromDate: string, toDate: string)
    : Observable<LedgerResponse> {
    const data: any = {
      Page: (pageIndex + 1).toString(),
      PageSize: pageSize.toString(),
      LedgerType: ledgerType.toString(),
      FromDate: fromDate ? fromDate : null,
      ToDate: toDate ? toDate : null
    };
    // const params = new HttpParams()
    //   .set('Page', (pageIndex + 1).toString())
    //   .set('PageSize', pageSize.toString())
    //   .set('LedgerType', ledgerType.toString())
    //   .set('ToDate', toDate ? toDate : null)
    //   .set('FromDate', fromDate ? fromDate : null);
    // return this.http.get<LedgerResponse>(this.apiUrl + '/GetCompanyLedger', { headers: this.tokenService.getHeaders(), params: params });
    return this.http.post<LedgerResponse>(this.apiUrl + '/GetCompanyLedger', JSON.stringify(data), { headers: this.tokenService.getHeaders() });
  }

  getLedgerDetails(ledgerType: number, id: number)
    : Observable<any> {
    const params = new HttpParams()
      .set('Id', id.toString())
      .set('LedgerType', ledgerType.toString());
    return this.http.get<any>(this.apiUrl + '/info', { headers: this.tokenService.getHeaders(), params: params });
  }
}
