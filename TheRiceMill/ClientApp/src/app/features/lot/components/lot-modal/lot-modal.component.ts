import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { GatePassType, ProductType } from '../../../../shared/model/enums';
import { MatDialogRef, MatAutocompleteSelectedEvent, MatChipInputEvent } from '@angular/material';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { Product } from '../../../../shared/model/product.model';
import { Party } from '../../../../shared/model/party.model';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import * as moment from 'moment';
import 'moment-timezone';
import { GatepassResponse } from '../../../../shared/model/gatepass-response.model';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
import { Purchase } from '../../../../shared/model/purchase.model';
import { AdditionalCharges } from '../../../../shared/model/additionalcharges.model';
import { ENTER, COMMA } from '@angular/cdk/keycodes';
import { ProcessedMaterial } from '../../../../shared/model/processed-material.model';
import { Lot } from '../../../../shared/model/lot.model';
@Component({
  selector: 'app-gatepass-modal',
  templateUrl: './lot-modal.component.html',
  styleUrls: ['./lot-modal.component.scss']
})
export class LotModalComponent implements OnInit {
  @ViewChild('gatepassInput') gatepassInput: ElementRef<HTMLInputElement>;

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = false;
  selectedRateOnText: string;
  selectedPartyId: number = 0;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  filteredGatepasses: Gatepass[];
  gatepasses: Gatepass[] = [];
  vehicleSuggestions: Vehicle[];
  partySuggestions: Party[];
  productSuggestions: Product[];
  totalKg = 0;
  commission = 0;
  basePrice = 0;

  lotForm: FormGroup = new FormGroup({
    date: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), Validators.required),
    processedMaterial: new FormArray([]),
  });

  public modalRef: MatDialogRef<LotModalComponent>;
  public isNew = true;
  public isDelete = false;
  private lot: Lot;

  constructor(
    // private purchaseService: PurchaseService,
    private notificationService: NotificationService,
    private gatepassService: GatepassService,
    public spinner: SpinnerService) {
  }

  ngOnInit() {
  //   this.lotForm.controls['gatepass'].valueChanges.subscribe(
  //     (response: string) => {
  //       this.gatepassService
  //       .getGatepassList(10, 0, response, 'false', '',true, GatePassType.InwardGatePass,this.selectedPartyId)
  //       .subscribe(
  //         (response: GatepassResponse) => {
  //           this.filteredGatepasses = response.data;
  //         }
  //   )
  // })

    this.lotForm.get('processedMaterial').valueChanges.subscribe(
      (value: Array<any>) => {
        this.totalKg = 0;
        if (value.length !== 0) {
          for (let i = 0; i < value.length; i++) {
            value[i].totalKg = +value[i].bagQuantity * +value[i].perKg;
            if (value[i].item) {
              this.totalKg += +value[i].totalKg;
            } else {
              this.totalKg -= +value[i].totalKg;
            }
          }

          this.lotForm.get('processedMaterial').setValue(value, { emitEvent: false });
        }
      }
    );
  }

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;
    // Add our gatepass
    // if ((value || '').trim()) {
    //   this.gatepasses.push(value.trim());
    // }

    // Reset the input value
    if (input) {
      input.value = '';
    }

    this.lotForm.controls['gatepass'].setValue(null);
  }

  remove(gatepass: Gatepass): void {
    let index = 0
    this.gatepasses.find((_gatepass, i) => {
      if (_gatepass.id == gatepass.id) {
        index = i
        return true
      }
    })

    if (index >= 0) {
      this.gatepasses.splice(index, 1);
      this.lotForm.get('weightPriceGroup.totalMaund').setValue(
        (this.lotForm.get('weightPriceGroup.totalMaund').value - gatepass.maund).toFixed(2)
      );

      this.lotForm.get('weightPriceGroup.boriQuantity').setValue(
        (this.lotForm.get('weightPriceGroup.boriQuantity').value - gatepass.boriQuantity).toFixed(2)
      );

      this.lotForm.get('weightPriceGroup.bagQuantity').setValue(
        (this.lotForm.get('weightPriceGroup.bagQuantity').value - gatepass.bagQuantity).toFixed(2)
      );
    }

    if(this.gatepasses.length === 0){
      this.selectedPartyId = 0
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    if (!this.isGatepassExists(event.option.value)) {
      this.gatepasses.push(event.option.value)
      this.lotForm.get('weightPriceGroup.totalMaund').setValue(
        +this.lotForm.get('weightPriceGroup.totalMaund').value + event.option.value.maund
      );

      this.lotForm.get('weightPriceGroup.boriQuantity').setValue(
        +this.lotForm.get('weightPriceGroup.boriQuantity').value + event.option.value.boriQuantity
      );

      this.lotForm.get('weightPriceGroup.bagQuantity').setValue(
        +this.lotForm.get('weightPriceGroup.bagQuantity').value + event.option.value.bagQuantity
      );
      this.selectedPartyId = event.option.value.party.id
    };
    this.gatepassInput.nativeElement.value = '';
    this.lotForm.controls['gatepass'].setValue(null);
  }

  isGatepassExists(gatepass: Gatepass): boolean {
    const findGatePass = this.gatepasses.find(_gatepass => _gatepass.id == gatepass.id)
    return findGatePass ? true : false
  }

  closeModal() {
    this.modalRef.close();
  }

  editPurchase(purchase: Purchase) {
    // this.isNew = false;
    // this.purchase = new Purchase();
    // Object.assign(this.purchase, purchase);
    // this.commission = this.purchase.commission;
    // this.gatepasses = this.purchase.gatepasses;
    // this.purchaseForm.patchValue({
    //   date: moment.utc(purchase.date).tz('Asia/Karachi').format().slice(0, 16),
    //   weightPriceGroup: {
    //     isMaundBasedRate: purchase.rateBasedOn.toString(),
    //     totalMaund: purchase.totalMaund,
    //     bagQuantity: purchase.bagQuantity,
    //     boriQuantity: purchase.boriQuantity,
    //     rate: purchase.rate,
    //     totalPrice: purchase.totalPrice,
    //     commission: purchase.commission
    //   }
    // }, { emitEvent: false });

    // if (this.purchase.additionalCharges.length >= 0) {
    //   for (let i = 0; i < this.purchase.additionalCharges.length; i++) {
    //     const formGroup = new FormGroup({
    //       id: new FormControl(this.purchase.additionalCharges[i].id, Validators.required),
    //       addPrice: new FormControl(this.purchase.additionalCharges[i].addPrice, Validators.required),
    //       task: new FormControl(this.purchase.additionalCharges[i].task, Validators.required),
    //       bagQuantity: new FormControl(this.purchase.additionalCharges[i].bagQuantity, [Validators.required, Validators.min(0)]),
    //       rate: new FormControl(this.purchase.additionalCharges[i].rate, [Validators.required, Validators.min(0)]),
    //       total: new FormControl(this.purchase.additionalCharges[i].total, [Validators.required, Validators.min(0)]),
    //     });
    //     (this.purchaseForm.get('additionalCharges') as FormArray).push(formGroup);
    //     if (this.purchase.additionalCharges[i].addPrice) {
    //       this.additionalCharges += this.purchase.additionalCharges[i].total;
    //     } else {
    //       this.additionalCharges -= this.purchase.additionalCharges[i].total;
    //     }
    //   }
    // }
  }

  deletePurchase(purchase: Purchase) {
    // this.isDelete = true;
    // this.purchase = new Purchase();
    // Object.assign(this.purchase, purchase);

  }

  delete() {
    // this.spinner.isLoading = true;
    // this.purchaseService.deletePurchase(this.purchase).subscribe(
    //   (data) => {
    //     this.spinner.isLoading = false;
    //     this.notificationService.successNotifcation('Purchase deleted successfully');
    //     this.purchaseService.purchaseEmitter.emit(true);
    //     this.modalRef.close();
    //   },
    //   (error) => {
    //     this.spinner.isLoading = false;
    //     console.log(error);
    //     this.notificationService.errorNotifcation('Something went wrong');
    //   });
  }

  // submit() {
  //   if (this.purchaseForm.valid) {
  //     if (this.gatepasses.length == 0) {
  //       return
  //     }

  //     this.spinner.isLoading = true;
  //     if (this.processedMaterial === undefined || this.purchase === null) {
  //       this.purchase = new ProcessedMaterial();
  //     }

  //     if (this.purchase.additionalCharges === undefined || this.purchase.additionalCharges === null) {
  //       this.purchase.additionalCharges = [];
  //     }

  //     this.purchase.date = moment(this.purchaseForm.value.date).utc().format();
  //     this.purchase.gatepassIds = this.gatepasses.map(gatepass => gatepass.id);
  //     this.purchase.rateBasedOn = this.purchaseForm.get('weightPriceGroup').value.isMaundBasedRate;
  //     this.purchase.totalMaund = this.purchaseForm.get('weightPriceGroup').value.totalMaund;
  //     this.purchase.bagQuantity = this.purchaseForm.get('weightPriceGroup').value.bagQuantity;
  //     this.purchase.boriQuantity = this.purchaseForm.get('weightPriceGroup').value.boriQuantity;
  //     this.purchase.rate = this.purchaseForm.get('weightPriceGroup').value.rate;
  //     this.purchase.totalPrice = this.getRateBasedOnTotal() + this.purchaseForm.get('weightPriceGroup.commission').value + this.additionalCharges;
  //     this.purchase.commission = this.purchaseForm.get('weightPriceGroup').value.commission;
  //     this.purchase.freight = this.purchaseForm.get('weightPriceGroup').value.freight;
  //     this.purchase.basePrice = this.getRateBasedOnTotal();  


  //     if (this.purchase.additionalCharges.length >= 0 && (this.purchaseForm.get('additionalCharges') as FormArray).length >= 0) {
  //       for (let i = 0; i < (this.purchaseForm.get('additionalCharges') as FormArray).length; i++) {
  //         this.purchase.additionalCharges[i].id = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.id;
  //         this.purchase.additionalCharges[i].addPrice = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.addPrice;
  //         this.purchase.additionalCharges[i].task = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.task;
  //         this.purchase.additionalCharges[i].bagQuantity = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.bagQuantity;
  //         this.purchase.additionalCharges[i].rate = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.rate;
  //         this.purchase.additionalCharges[i].total = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.total;
  //       }
  //     }

  //     if (this.isNew) {
  //       this.purchaseService.addPurchase(this.purchase).subscribe(
  //         (response: PurchaseResponse) => {
  //           this.spinner.isLoading = false;
  //           this.notificationService.successNotifcation('Purchase added successfully');
  //           this.modalRef.close();
  //           this.purchaseService.purchaseEmitter.emit(response.data);
  //         },
  //         (error) => {
  //           console.log(error);
  //           this.spinner.isLoading = false;
  //           this.notificationService.errorNotifcation('Something went wrong');
  //         });

  //     } else {
  //       this.purchaseService.updatePurchase(this.purchase).subscribe(
  //         (data) => {
  //           this.spinner.isLoading = false;
  //           this.notificationService.successNotifcation('Purchase updated successfully');
  //           this.purchaseService.purchaseEmitter.emit(true);
  //           this.modalRef.close();
  //         },
  //         (error) => {
  //           this.spinner.isLoading = false;
  //           console.log(error);
  //           this.notificationService.errorNotifcation('Something went wrong');
  //         });
  //     }
  //   }
  // }


  addCharges() {
    const formGroup = new FormGroup({
      id: new FormControl(0, Validators.required),
      item: new FormControl(null, Validators.required),
      boriQuantity: new FormControl(null, [Validators.required, Validators.min(0)]),
      bagQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      perKg: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalKg: new FormControl(null, [Validators.required, Validators.min(0)]),
    });
    (this.lotForm.get('processedMaterial') as FormArray).push(formGroup);
    if (this.lot === undefined || this.lot === null) {
      this.lot = new Lot();
    }
    if (this.lot.processedMaterial === undefined || this.lot.processedMaterial === null) {
      this.lot.processedMaterial = [];
    }
    this.lot.processedMaterial.push(new ProcessedMaterial());
  }

  deleteCharges(id: number) {
    (this.lotForm.get('processedMaterial') as FormArray).removeAt(id);
    this.lot.processedMaterial.splice(id, 1);
  }


}
