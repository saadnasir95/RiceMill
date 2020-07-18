import { Component, OnInit, Input } from '@angular/core';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { Company } from '../../../../shared/model/company.model';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { Product } from '../../../../shared/model/product.model';

@Component({
  selector: 'app-gatepass-receipt',
  templateUrl: './gatepass-receipt.component.html',
  styleUrls: ['./gatepass-receipt.component.scss']
})
export class GatepassReceiptComponent implements OnInit {
  gatepass: Gatepass;
  constructor() { }

  ngOnInit() {
    this.gatepass = new Gatepass();
    this.gatepass.company = new Company();
    this.gatepass.vehicle = new Vehicle();
    this.gatepass.product = new Product();
  }

}
