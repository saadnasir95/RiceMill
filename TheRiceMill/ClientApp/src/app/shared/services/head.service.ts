import { Injectable, EventEmitter } from '@angular/core';
import { TokenService } from './token.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CompanyService } from './company.service';
import { HeadResponseModel } from '../model/head-response.model';
import { Head1, Head2, Head3, Head4, Head5 } from '../model/head.model';

@Injectable({
  providedIn: 'root'
})
export class HeadService {
  headEmitter = new EventEmitter<any>();
  private apiUrl = environment.baseUrl + '/api/v1/Head';

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private companyService: CompanyService) { }

  getAllHeads(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = '')
    : Observable<HeadResponseModel> {
    const params = new HttpParams()
      .set('CompanyId', this.companyService.getCompanyId().toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<HeadResponseModel>(this.apiUrl + '/GetAllHeads', { headers: this.tokenService.getHeaders(), params: params });
  }

  addHead1(head1: Head1): Observable<any> {
    head1.companyId = this.companyService.getCompanyId();
    return this.http.post(this.apiUrl + '/Head1', JSON.stringify(head1), { headers: this.tokenService.getHeaders() });
  }
  addHead2(head2: Head2): Observable<any> {
    return this.http.post(this.apiUrl + '/Head2', JSON.stringify(head2), { headers: this.tokenService.getHeaders() });
  }
  addHead3(head3: Head3): Observable<any> {
    return this.http.post(this.apiUrl + '/Head3', JSON.stringify(head3), { headers: this.tokenService.getHeaders() });
  }
  addHead4(head4: Head4): Observable<any> {
    return this.http.post(this.apiUrl + '/Head4', JSON.stringify(head4), { headers: this.tokenService.getHeaders() });
  }
  addHead5(head5: Head5): Observable<any> {
    return this.http.post(this.apiUrl + '/Head5', JSON.stringify(head5), { headers: this.tokenService.getHeaders() });
  }

  updateHead1(head1: Head1): Observable<any> {
    head1.companyId = this.companyService.getCompanyId();
    return this.http.put(this.apiUrl + '/Head1', JSON.stringify(head1), { headers: this.tokenService.getHeaders() });
  }
  updateHead2(head2: Head2): Observable<any> {
    return this.http.put(this.apiUrl + '/Head2', JSON.stringify(head2), { headers: this.tokenService.getHeaders() });
  }
  updateHead3(head3: Head3): Observable<any> {
    return this.http.put(this.apiUrl + '/Head3', JSON.stringify(head3), { headers: this.tokenService.getHeaders() });
  }
  updateHead4(head4: Head4): Observable<any> {
    return this.http.put(this.apiUrl + '/Head4', JSON.stringify(head4), { headers: this.tokenService.getHeaders() });
  }
  updateHead5(head5: Head5): Observable<any> {
    return this.http.put(this.apiUrl + '/Head5', JSON.stringify(head5), { headers: this.tokenService.getHeaders() });
  }

  deleteHead1(head1: Head1): Observable<any> {
    const params = new HttpParams()
      .set('id', head1.id.toString());
    return this.http.delete(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
}
