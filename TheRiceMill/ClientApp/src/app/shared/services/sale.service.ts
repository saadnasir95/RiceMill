import { Injectable, EventEmitter } from '@angular/core';
import { Sale } from '../model/sale.model';
import { Observable } from 'rxjs';
import { SaleResponse } from '../model/sale-response.model';
import { TokenService } from './token.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CompanyService } from './company.service';

@Injectable({
  providedIn: 'root'
})
export class SaleService {
  saleEmitter = new EventEmitter<any>();
  apiUrl = environment.baseUrl + '/api/v1/sale';

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private companyService: CompanyService) { }

  getSaleList(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = '')
    : Observable<SaleResponse> {
    const params = new HttpParams()
      .set('CompanyId', this.companyService.getCompanyId().toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<SaleResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }

  addSale(sale: Sale): Observable<any> {
    sale.companyId = this.companyService.getCompanyId();
    return this.http.post(this.apiUrl, JSON.stringify(sale), { headers: this.tokenService.getHeaders() });
  }

  updateSale(sale: Sale): Observable<any> {
    sale.companyId = this.companyService.getCompanyId();
    return this.http.put(this.apiUrl, JSON.stringify(sale), { headers: this.tokenService.getHeaders() });
  }

  deleteSale(sale: Sale): Observable<any> {
    const params = new HttpParams()
      .set('SaleId', sale.id.toString());
    return this.http.delete(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
}
