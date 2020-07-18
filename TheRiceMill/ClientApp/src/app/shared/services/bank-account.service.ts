import { Injectable, EventEmitter } from '@angular/core';
import { TokenService } from './token.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BankAccountResponse } from '../model/bank-account-response.model';
import { BankAccount } from '../model/bank-account.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BankAccountService {
  bankAccountEmitter = new EventEmitter<boolean>();
  apiUrl = environment.baseUrl + '/api/v1/BankAccount';
  constructor(private tokenService: TokenService, private http: HttpClient) { }

  getBankAccounts(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = ''):
    Observable<BankAccountResponse> {
    const params = new HttpParams()
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<BankAccountResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
  addBankAccount(bankAccount: BankAccount): Observable<any> {
    return this.http.post(this.apiUrl, JSON.stringify(bankAccount), { headers: this.tokenService.getHeaders() });
  }
  updateBankAccount(bankAccount: BankAccount): Observable<any> {
    return this.http.put(this.apiUrl, JSON.stringify(bankAccount), { headers: this.tokenService.getHeaders() });
  }
  deleteBankAccount(bankAccount: BankAccount): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${bankAccount.id}`, { headers: this.tokenService.getHeaders() });
  }
}
