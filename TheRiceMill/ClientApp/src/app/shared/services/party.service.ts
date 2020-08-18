import { Injectable, EventEmitter } from '@angular/core';
import { Party } from '../model/party.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TokenService } from './token.service';
import { Observable } from 'rxjs';
import { PartyResponse } from '../model/party-response.model';
import { environment } from '../../../environments/environment';
import { CompanyService } from './company.service';

@Injectable({
  providedIn: 'root'
})
export class PartyService {
  partyEmitter = new EventEmitter<boolean>();
  apiUrl = environment.baseUrl + '/api/v1/Party';
  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private companyService: CompanyService) { }

  getParties(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = '')
    : Observable<PartyResponse> {
    const params = new HttpParams()
      .set('CompanyId', this.companyService.getCompanyId().toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<PartyResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }

  addParty(party: Party): Observable<any> {
    party.companyId = this.companyService.getCompanyId();
    return this.http.post(this.apiUrl, JSON.stringify(party), { headers: this.tokenService.getHeaders() });
  }

  updateParty(party: Party): Observable<any> {
    party.companyId = this.companyService.getCompanyId();
    return this.http.put(this.apiUrl, JSON.stringify(party), { headers: this.tokenService.getHeaders() });
  }

  deleteParty(party: Party): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${party.id}`, { headers: this.tokenService.getHeaders() });
  }

}
