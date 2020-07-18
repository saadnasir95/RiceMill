import { Component, OnInit } from '@angular/core';
import { Sale } from '../../../../shared/model/sale.model';
import { Company } from '../../../../shared/model/company.model';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { Product } from '../../../../shared/model/product.model';

@Component({
  selector: 'app-sale-receipt',
  templateUrl: './sale-receipt.component.html',
  styleUrls: ['./sale-receipt.component.scss']
})
export class SaleReceiptComponent implements OnInit {
  sale: Sale;
  constructor() { }

  ngOnInit() {
    this.sale = new Sale();
    this.sale.company = new Company();
    this.sale.vehicle = new Vehicle();
    this.sale.product = new Product();
  }

}
