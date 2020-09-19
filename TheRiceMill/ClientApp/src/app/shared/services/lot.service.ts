import { Injectable, EventEmitter } from '@angular/core';
import { Gatepass } from '../model/gatepass.model';
import { TokenService } from './token.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GatepassResponse } from '../model/gatepass-response.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CompanyService } from './company.service';
import { LotResponse } from '../model/lot-response.model';
import { ProcessedMaterial, CreateProcessedMaterial } from '../model/processed-material.model';
import { CreateRateCost } from '../model/create-rate-cost.model';

@Injectable({
  providedIn: 'root'
})
export class LotService {
  lotEmitter = new EventEmitter<any>();
  apiUrl = environment.baseUrl + '/api/v1/Lot';

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private companyService: CompanyService) { }

  getLotList(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = '', lotId: string, lotYear: string)
    : Observable<LotResponse> {
    const params = new HttpParams()
      .set('CompanyId', this.companyService.getCompanyId().toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '')
      .set('lotId', lotId.toString())
      .set('lotYear', lotYear.toString())
    return this.http.get<LotResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }

  getYears(): Observable<any> {
    return this.http.get(this.apiUrl + '/Years', { headers: this.tokenService.getHeaders() });
  }

  createProcessedMaterial(processedMaterial: CreateProcessedMaterial): Observable<any> {
    return this.http.post(this.apiUrl, JSON.stringify(processedMaterial), { headers: this.tokenService.getHeaders() });
  }

  createRateCost(createRateCost: CreateRateCost): Observable<any> {
    return this.http.post(this.apiUrl+"/RateCost", JSON.stringify(createRateCost), { headers: this.tokenService.getHeaders() });
  }

  updateRateCost(createRateCost: CreateRateCost): Observable<any> {
    return this.http.put(this.apiUrl+"/RateCost", JSON.stringify(createRateCost), { headers: this.tokenService.getHeaders() });
  }

  updateGatepass(gatepass: Gatepass): Observable<any> {
    gatepass.companyId = this.companyService.getCompanyId();
    return this.http.put(this.apiUrl, JSON.stringify(gatepass), { headers: this.tokenService.getHeaders() });
  }

  deleteGatepass(gatepass: Gatepass): Observable<any> {
    const params = new HttpParams()
      .set('id', gatepass.id.toString());
    return this.http.delete(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }
}
