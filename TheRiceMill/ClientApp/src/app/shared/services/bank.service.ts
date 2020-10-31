import { Injectable, EventEmitter } from '@angular/core';
import { TokenService } from './token.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BankResponse } from '../model/bank-response.model';
import { Bank } from '../model/bank.model';

@Injectable({
  providedIn: 'root'
})
export class BankService {
  bankEmitter = new EventEmitter<boolean>();
  apiUrl = environment.baseUrl + '/api/v1/Bank';
  constructor(private tokenService: TokenService, private http: HttpClient) { }

  getBanks(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = ''):
    Observable<BankResponse> {
    const params = new HttpParams()
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<BankResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
  addBank(bank: Bank): Observable<any> {
    return this.http.post(this.apiUrl, JSON.stringify(bank), { headers: this.tokenService.getHeaders() });
  }
  updateBank(bank: Bank): Observable<any> {
    return this.http.put(this.apiUrl, JSON.stringify(bank), { headers: this.tokenService.getHeaders() });
  }
  deleteBank(bank: Bank): Observable<any> {
    const params = new HttpParams()
      .set('BankId', bank.id.toString());
    return this.http.delete(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
}
