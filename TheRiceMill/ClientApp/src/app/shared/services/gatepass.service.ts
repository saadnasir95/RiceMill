import { Injectable, EventEmitter } from '@angular/core';
import { Gatepass } from '../model/gatepass.model';
import { TokenService } from './token.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GatepassResponse } from '../model/gatepass-response.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GatepassService {
  gatepassEmitter = new EventEmitter<any>();
  apiUrl = environment.baseUrl + '/api/v1/Gatepass';

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getGatepassList(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = '')
    : Observable<GatepassResponse> {
    const params = new HttpParams()
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<GatepassResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }

  addGatepass(gatepass: Gatepass): Observable<any> {
    return this.http.post(this.apiUrl, JSON.stringify(gatepass), { headers: this.tokenService.getHeaders() });
  }

  updateGatepass(gatepass: Gatepass): Observable<any> {
    return this.http.put(this.apiUrl, JSON.stringify(gatepass), { headers: this.tokenService.getHeaders() });
  }

  deleteGatepass(gatepass: Gatepass): Observable<any> {
    const params = new HttpParams()
      .set('id', gatepass.id.toString());
    return this.http.delete(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
}
