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
  getLedgerDetails(ledgerType: number, id: number)
    : Observable<any> {
    const params = new HttpParams()
      .set('Id', id.toString())
      .set('LedgerType', ledgerType.toString());
    return this.http.get<any>(this.apiUrl + '/info', { headers: this.tokenService.getHeaders(), params: params });
  }
}
