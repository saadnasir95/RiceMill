import { Injectable, EventEmitter } from '@angular/core';
import { Company } from '../model/company.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TokenService } from './token.service';
import { Observable } from 'rxjs';
import { CompanyResponse } from '../model/company-response.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  companyEmitter = new EventEmitter<boolean>();
  apiUrl = environment.baseUrl + '/api/v1/Company';
  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getCompanies(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = '')
    : Observable<CompanyResponse> {
    const params = new HttpParams()
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<CompanyResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }

  addCompany(company: Company): Observable<any> {
    return this.http.post(this.apiUrl, JSON.stringify(company), { headers: this.tokenService.getHeaders() });
  }

  updateCompany(company: Company): Observable<any> {
    return this.http.put(this.apiUrl, JSON.stringify(company), { headers: this.tokenService.getHeaders() });
  }

  deleteCompany(company: Company): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${company.id}`, { headers: this.tokenService.getHeaders() });
  }

}
