import { Injectable, EventEmitter } from '@angular/core';
import { Product } from '../model/product.model';
import { TokenService } from './token.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ProductResponse } from '../model/product-response.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CompanyService } from './company.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  productEmitter = new EventEmitter<boolean>();
  apiUrl = environment.baseUrl + '/api/v1/Product';
  constructor(
    private tokenService: TokenService,
    private http: HttpClient,
    private companyService: CompanyService) { }

  getProducts(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = ''):
    Observable<ProductResponse> {
    const params = new HttpParams()
      .set('CompanyId', this.companyService.getCompanyId().toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<ProductResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
  addProduct(product: Product): Observable<any> {
    product.companyId = this.companyService.getCompanyId();
    return this.http.post(this.apiUrl, JSON.stringify(product), { headers: this.tokenService.getHeaders() });
  }
  updateProduct(product: Product): Observable<any> {
    product.companyId = this.companyService.getCompanyId();
    return this.http.put(this.apiUrl, JSON.stringify(product), { headers: this.tokenService.getHeaders() });
  }
  deleteProduct(product: Product): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${product.id}`, { headers: this.tokenService.getHeaders() });
  }
}
