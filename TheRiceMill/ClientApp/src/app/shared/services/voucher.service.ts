import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CompanyService } from './company.service';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class VoucherService {
  voucherEmitter = new EventEmitter<boolean>();
  apiUrl = environment.baseUrl + '/api/v1/Vehicle';
  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private companyService: CompanyService
  ) { }
}
