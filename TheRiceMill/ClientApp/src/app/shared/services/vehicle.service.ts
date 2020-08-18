import { Injectable, EventEmitter } from '@angular/core';
import { Vehicle } from '../model/vehicle.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VehicleResponse } from '../model/vehicle-response.model';
import { TokenService } from './token.service';
import { environment } from '../../../environments/environment';
import { CompanyService } from './company.service';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  vehicleEmitter = new EventEmitter<boolean>();
  apiUrl = environment.baseUrl + '/api/v1/Vehicle';
  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private companyService: CompanyService) { }

  getVehicles(pageSize: number, pageIndex: number, search = '', sortDirection = 'false', orderBy = '')
    : Observable<VehicleResponse> {
    const params = new HttpParams()
      .set('CompanyId', this.companyService.getCompanyId().toString())
      .set('Page', (pageIndex + 1).toString())
      .set('PageSize', pageSize.toString())
      .set('search', search + '')
      .set('isDescending', sortDirection)
      .set('orderBy', orderBy + '');
    return this.http.get<VehicleResponse>(this.apiUrl, { headers: this.tokenService.getHeaders(), params: params });
  }

  addVehicle(vehicle: Vehicle): Observable<any> {
    vehicle.companyId = this.companyService.getCompanyId();
    return this.http.post(this.apiUrl, JSON.stringify(vehicle), { headers: this.tokenService.getHeaders() });
  }

  updateVehicle(vehicle: Vehicle): Observable<any> {
    vehicle.companyId = this.companyService.getCompanyId();
    return this.http.put(this.apiUrl, JSON.stringify(vehicle), { headers: this.tokenService.getHeaders() });
  }

  deleteVehicle(vehicle: Vehicle): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${vehicle.id}`, { headers: this.tokenService.getHeaders() });
  }

}
