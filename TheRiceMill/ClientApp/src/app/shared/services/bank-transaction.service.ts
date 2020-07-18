import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TokenService } from './token.service';
import { BankTransactionResponse } from '../model/bank-transaction-response';
import { Observable } from 'rxjs';
import { BankTransaction } from '../model/bank-transaction.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BankTransactionService {
  transactionEmitter = new EventEmitter<boolean>();
  apiUrl = environment.baseUrl + '/api/v1/BankTransaction';
  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getBankTransactions(bankAccountId: number, pageSize: number, pageIndex: number)
    : Observable<BankTransactionResponse> {
    const params = new HttpParams()
      .set('bankAccountId', bankAccountId.toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString());
    return this.http.get<BankTransactionResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }

  addbankTransaction(transaction: BankTransaction): Observable<any> {
    return this.http.post(this.apiUrl, JSON.stringify(transaction), { headers: this.tokenService.getHeaders() });
  }

  updateBankTransaction(transaction: BankTransaction): Observable<any> {
    return this.http.put(this.apiUrl, JSON.stringify(transaction), { headers: this.tokenService.getHeaders() });
  }

  deleteBankTransaction(transaction: BankTransaction): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${transaction.id}`, { headers: this.tokenService.getHeaders() });
  }
}
