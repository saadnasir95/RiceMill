import { Injectable, EventEmitter } from '@angular/core';
import { Purchase } from '../model/purchase.model';
import { Observable } from 'rxjs';
import { PurchaseResponse } from '../model/purchase-response.model';
import { TokenService } from './token.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CompanyService } from './company.service';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {
  purchaseEmitter = new EventEmitter<any>();
  apiUrl = environment.baseUrl + '/api/v1/Purchase';

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private companyService: CompanyService) { }

  getPurchaseList(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = '')
    : Observable<PurchaseResponse> {
    const params = new HttpParams()
      .set('CompanyId', this.companyService.getCompanyId().toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<PurchaseResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }

  addPurchase(purchase: Purchase): Observable<any> {
    purchase.companyId = this.companyService.getCompanyId();
    return this.http.post(this.apiUrl, JSON.stringify(purchase), { headers: this.tokenService.getHeaders() });
  }

  updatePurchase(purchase: Purchase): Observable<any> {
    purchase.companyId = this.companyService.getCompanyId();
    return this.http.put(this.apiUrl, JSON.stringify(purchase), { headers: this.tokenService.getHeaders() });
  }

  deletePurchase(purchase: Purchase): Observable<any> {
    const params = new HttpParams()
      .set('PurchaseId', purchase.id.toString());
    return this.http.delete(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
}
