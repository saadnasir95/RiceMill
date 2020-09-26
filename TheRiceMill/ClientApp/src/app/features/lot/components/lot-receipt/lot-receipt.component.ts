import { Component, OnInit, Input } from '@angular/core';
import { Lot } from '../../../../shared/model/lot.model';

@Component({
  selector: 'app-lot-receipt',
  templateUrl: './lot-receipt.component.html',
  styleUrls: ['./lot-receipt.component.scss']
})
export class LotReceiptComponent implements OnInit {
  lot: Lot;
  constructor() { 
  }

  ngOnInit() {
    this.lot = new Lot();
  }

}
