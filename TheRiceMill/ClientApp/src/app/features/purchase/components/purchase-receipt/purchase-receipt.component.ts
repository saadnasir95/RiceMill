import { Component, OnInit } from '@angular/core';
import { Purchase } from '../../../../shared/model/purchase.model';
import { Company } from '../../../../shared/model/company.model';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { Product } from '../../../../shared/model/product.model';

@Component({
  selector: 'app-purchase-receipt',
  templateUrl: './purchase-receipt.component.html',
  styleUrls: ['./purchase-receipt.component.scss']
})
export class PurchaseReceiptComponent implements OnInit {
  purchase: Purchase;
  constructor() { }

  ngOnInit() {
    this.purchase = new Purchase();
    this.purchase.company = new Company();
    this.purchase.vehicle = new Vehicle();
    this.purchase.product = new Product();
    this.purchase.additionalCharges = [];
  }

}
