import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { GateinDirection, ProductType, RateBasedOn } from '../../../../shared/model/enums';
import { MatDialogRef, MatAutocompleteSelectedEvent, MatChipInputEvent, MatSlideToggleChange } from '@angular/material';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { Product } from '../../../../shared/model/product.model';
import { Party } from '../../../../shared/model/party.model';
import { PartyService } from '../../../../shared/services/party.service';
import { VehicleService } from '../../../../shared/services/vehicle.service';
import { ProductService } from '../../../../shared/services/product.service';
import { Purchase } from '../../../../shared/model/purchase.model';
import { PurchaseService } from '../../../../shared/services/purchase.service';
import { AdditionalCharges } from '../../../../shared/model/additionalcharges.model';
import { PartyResponse } from '../../../../shared/model/party-response.model';
import { ProductResponse } from '../../../../shared/model/product-response.model';
import { VehicleResponse } from '../../../../shared/model/vehicle-response.model';
import * as moment from 'moment';
import 'moment-timezone';
import { NotificationService } from '../../../../shared/services/notification.service';
import { PurchaseResponse } from '../../../../shared/model/purchase-response.model';
import { SpinnerService } from '../../../../shared/services/spinner.service';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import { GatepassResponse } from '../../../../shared/model/gatepass-response.model';
import { Gatepass } from '../../../../shared/model/gatepass.model';

@Component({
  selector: 'app-purchase-modal',
  templateUrl: './purchase-modal.component.html',
  styleUrls: ['./purchase-modal.component.scss']
})
export class PurchaseModalComponent implements OnInit {
  @ViewChild('gatepassInput') gatepassInput: ElementRef<HTMLInputElement>;

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = false;
  selectedRateOnText: string 
  separatorKeysCodes: number[] = [ENTER, COMMA];
  filteredGatepasses: Gatepass[];
  gatepasses: Gatepass[] = [];
  vehicleSuggestions: Vehicle[];
  partySuggestions: Party[];
  productSuggestions: Product[];
  additionalCharges = 0;
  commission = 0;
  basePrice = 0;

  purchaseForm: FormGroup = new FormGroup({
    date: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), Validators.required),
    additionalCharges: new FormArray([]),
    gatepass: new FormControl(),
    // Credit Debit Form Control
    // credit: new FormControl(0, [Validators.required, Validators.min(0)]),
    // debit: new FormControl(0, [Validators.required, Validators.min(0)]),
    // direction: new FormControl(null, Validators.required),
    weightPriceGroup: new FormGroup({
      isMaundBasedRate: new FormControl("1"),
      bagQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      boriQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      // bagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      // kandaWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      // expectedBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      // totalExpectedBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      // emptyBagWeight: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(2)]),
      // totalEmptyBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      // actualBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      // totalActualBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalMaund: new FormControl(0, [Validators.required, Validators.min(0)]),
      // vibration: new FormControl(0, [Validators.required, Validators.min(0)]),
      // ratePerKg: new FormControl(0, [Validators.required, Validators.min(0)]),
      rate: new FormControl(0, [Validators.required, Validators.min(0)]),
      commission: new FormControl(0, [Validators.required, Validators.min(0)]),
      // percentCommission: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalPrice: new FormControl(0, [Validators.required, Validators.min(0)]),
      // actualBags: new FormControl(0, [Validators.required, Validators.min(0)]),
    })
  });
  public GateinDirectionTypes = [
    { text: 'Milling', value: +GateinDirection.Milling },
    { text: 'Outside', value: +GateinDirection.Outside },
    { text: 'Stockpile', value: +GateinDirection.Stockpile },
  ];
  public modalRef: MatDialogRef<PurchaseModalComponent>;
  public isNew = true;
  public isDelete = false;
  private purchase: Purchase;

  constructor(
    private purchaseService: PurchaseService,
    private partyService: PartyService,
    private vehicleService: VehicleService,
    private productService: ProductService,
    private notificationService: NotificationService,
    private gatepassService: GatepassService,
    public spinner: SpinnerService) {
     }

  ngOnInit() {
    this.purchaseForm.controls['gatepass'].valueChanges.subscribe(
      (response: string) => {
        this.gatepassService
        .getGatepassList(10, 0, response, 'false', '',true)
        .subscribe(
          (response: GatepassResponse) => {
            this.filteredGatepasses = response.data;
          }
    )
  })

  this.purchaseForm.get('weightPriceGroup.isMaundBasedRate').valueChanges.subscribe(
    (response: string) => {
      if(+response == RateBasedOn.Maund){
        this.selectedRateOnText = "Maund";
      } 
      else if(+response == RateBasedOn.Bag) {
        this.selectedRateOnText = "Bag"; 
      }
  })

    this.purchaseForm.get('additionalCharges').valueChanges.subscribe(
      (value: Array<any>) => {
        this.additionalCharges = 0;
        if (value.length !== 0) {
          for (let i = 0; i < value.length; i++) {
            value[i].total = Math.round(+value[i].bagQuantity * +value[i].rate);
            if (value[i].addPrice) {
              this.additionalCharges += +value[i].total;
            } else {
              this.additionalCharges -= +value[i].total;
            }
          }

          this.purchaseForm.get('additionalCharges').setValue(value, { emitEvent: false });
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

    this.purchaseForm.controls['gatepass'].setValue(null);
  }

  remove(gatepass: Gatepass): void {
    let index = 0     
    this.gatepasses.find((_gatepass,i) => {
      if(_gatepass.id == gatepass.id){
        index = i
        return true
      }
    })

    if (index >= 0) {
      this.gatepasses.splice(index, 1);
      this.purchaseForm.get('weightPriceGroup.totalMaund').setValue(
        (this.purchaseForm.get('weightPriceGroup.totalMaund').value - gatepass.maund).toFixed(2)
      );

      this.purchaseForm.get('weightPriceGroup.boriQuantity').setValue(
        (this.purchaseForm.get('weightPriceGroup.boriQuantity').value - gatepass.boriQuantity).toFixed(2)
      );

      this.purchaseForm.get('weightPriceGroup.bagQuantity').setValue(
        (this.purchaseForm.get('weightPriceGroup.bagQuantity').value - gatepass.bagQuantity).toFixed(2)
      );
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    if(!this.isGatepassExists(event.option.value)){
      this.gatepasses.push(event.option.value)
      this.purchaseForm.get('weightPriceGroup.totalMaund').setValue(
        +this.purchaseForm.get('weightPriceGroup.totalMaund').value + event.option.value.maund
      );

      this.purchaseForm.get('weightPriceGroup.boriQuantity').setValue(
        +this.purchaseForm.get('weightPriceGroup.boriQuantity').value + event.option.value.boriQuantity
      );

      this.purchaseForm.get('weightPriceGroup.bagQuantity').setValue(
        +this.purchaseForm.get('weightPriceGroup.bagQuantity').value + event.option.value.bagQuantity
      );
    };
    // this.updateRateType(RateBasedOn.Maund);
    this.gatepassInput.nativeElement.value = '';
    this.purchaseForm.controls['gatepass'].setValue(null);
  }

  isGatepassExists(gatepass: Gatepass): boolean{
    const findGatePass = this.gatepasses.find(_gatepass => _gatepass.id == gatepass.id)
    return findGatePass ? true : false 
  }

  closeModal() {
    this.modalRef.close();
  }

  editPurchase(purchase: Purchase) {
    this.isNew = false;
    this.purchase = new Purchase();
    Object.assign(this.purchase, purchase);
    this.commission = this.purchase.commission;
    this.gatepasses = this.purchase.gatepasses;
    // this.updateRateType(this.purchase.rateBasedOn);
    // this.basePrice = this.purchase.basePrice;
    this.purchaseForm.patchValue({
      date: moment.utc(purchase.date).tz('Asia/Karachi').format().slice(0, 16),
      // direction: purchase.direction,
      weightPriceGroup: {
        // bagQuantity: purchase.bagQuantity,
        // bagWeight: purchase.bagWeight,
        // kandaWeight: purchase.kandaWeight,
        // emptyBagWeight: purchase.expectedEmptyBagWeight,
        // totalEmptyBagWeight: purchase.totalExpectedEmptyBagWeight,
        // expectedBagWeight: purchase.expectedBagWeight,
        // totalExpectedBagWeight: purchase.totalExpectedBagWeight,
        // actualBagWeight: purchase.actualBagWeight,
        // totalActualBagWeight: purchase.totalActualBagWeight,
        // vibration: purchase.vibration,
        // actualBags: purchase.actualBags,
        // percentCommission: purchase.percentCommission,
        // ratePerKg: purchase.ratePerKg,
        isMaundBasedRate: purchase.rateBasedOn.toString(),
        totalMaund: purchase.totalMaund,
        bagQuantity: purchase.bagQuantity,
        boriQuantity: purchase.boriQuantity,
        rate: purchase.rate,
        totalPrice: purchase.totalPrice,
        commission: purchase.commission
      }
    }, { emitEvent: false });

    if (this.purchase.additionalCharges.length >= 0) {
      for (let i = 0; i < this.purchase.additionalCharges.length; i++) {
        const formGroup = new FormGroup({
          id: new FormControl(this.purchase.additionalCharges[i].id, Validators.required),
          addPrice: new FormControl(this.purchase.additionalCharges[i].addPrice, Validators.required),
          task: new FormControl(this.purchase.additionalCharges[i].task, Validators.required),
          bagQuantity: new FormControl(this.purchase.additionalCharges[i].bagQuantity, [Validators.required, Validators.min(0)]),
          rate: new FormControl(this.purchase.additionalCharges[i].rate, [Validators.required, Validators.min(0)]),
          total: new FormControl(this.purchase.additionalCharges[i].total, [Validators.required, Validators.min(0)]),
        });
        (this.purchaseForm.get('additionalCharges') as FormArray).push(formGroup);
        if (this.purchase.additionalCharges[i].addPrice) {
          this.additionalCharges += this.purchase.additionalCharges[i].total;
        } else {
          this.additionalCharges -= this.purchase.additionalCharges[i].total;
        }
      }
    }
  }

  deletePurchase(purchase: Purchase) {
    this.isDelete = true;
    this.purchase = new Purchase();
    Object.assign(this.purchase, purchase);

  }

  delete() {
    this.spinner.isLoading = true;
    this.purchaseService.deletePurchase(this.purchase).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('Purchase deleted successfully');
        this.purchaseService.purchaseEmitter.emit(true);
        this.modalRef.close();
      },
      (error) => {
        this.spinner.isLoading = false;
        console.log(error);
        this.notificationService.errorNotifcation('Something went wrong');
      });
  }

  submit(){
    if (this.purchaseForm.valid) {
      if(this.gatepasses.length == 0){
        return
      }

      this.spinner.isLoading = true;
      if (this.purchase === undefined || this.purchase === null) {
        this.purchase = new Purchase();
      }
     
      if (this.purchase.additionalCharges === undefined || this.purchase.additionalCharges === null) {
        this.purchase.additionalCharges = [];
      }

      this.purchase.date = moment(this.purchaseForm.value.date).utc().format(); 
      this.purchase.gatepassIds = this.gatepasses.map(gatepass => gatepass.id);
      this.purchase.rateBasedOn = this.purchaseForm.get('weightPriceGroup').value.isMaundBasedRate;
      this.purchase.totalMaund = this.purchaseForm.get('weightPriceGroup').value.totalMaund;
      this.purchase.bagQuantity = this.purchaseForm.get('weightPriceGroup').value.bagQuantity;
      this.purchase.boriQuantity = this.purchaseForm.get('weightPriceGroup').value.boriQuantity;
      this.purchase.rate = this.purchaseForm.get('weightPriceGroup').value.rate;
      this.purchase.totalPrice = this.getRateBasedOnTotal() + this.purchaseForm.get('weightPriceGroup.commission').value + this.additionalCharges;
      this.purchase.commission = this.purchaseForm.get('weightPriceGroup').value.commission;

      if (this.purchase.additionalCharges.length >= 0 && (this.purchaseForm.get('additionalCharges') as FormArray).length >= 0) {
        for (let i = 0; i < (this.purchaseForm.get('additionalCharges') as FormArray).length; i++) {
          this.purchase.additionalCharges[i].id = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.id;
          this.purchase.additionalCharges[i].addPrice = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.addPrice;
          this.purchase.additionalCharges[i].task = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.task;
          this.purchase.additionalCharges[i].bagQuantity = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.bagQuantity;
          this.purchase.additionalCharges[i].rate = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.rate;
          this.purchase.additionalCharges[i].total = (this.purchaseForm.get('additionalCharges') as FormArray).at(i).value.total;
        }
      }

      if (this.isNew) {
        this.purchaseService.addPurchase(this.purchase).subscribe(
          (response: PurchaseResponse) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Purchase added successfully');
            this.modalRef.close();
            this.purchaseService.purchaseEmitter.emit(response.data);
          },
          (error) => {
            console.log(error);
            this.spinner.isLoading = false;
            this.notificationService.errorNotifcation('Something went wrong');
          });

      } else {
        this.purchaseService.updatePurchase(this.purchase).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Purchase updated successfully');
            this.purchaseService.purchaseEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          });
      }
    }
  }

  selectedParty(event: MatAutocompleteSelectedEvent) {
    this.purchaseForm.get('partyGroup').setValue({
      name: event.option.value.name,
      phoneNumber: event.option.value.phoneNumber,
      address: event.option.value.address
    }, { emitEvent: false });
    if (this.purchase === undefined || this.purchase === null) {
      this.purchase = new Purchase();
    }
    if (this.purchase.party === undefined || this.purchase.party === null) {
      this.purchase.party = new Party();
    }
    this.purchase.partyId = event.option.value.id;
    this.purchase.party = event.option.value;
  }

  selectedProduct(event: MatAutocompleteSelectedEvent) {
    this.purchaseForm.get('productGroup').setValue({
      name: event.option.value.name,
    }, { emitEvent: false });
    this.purchaseForm.get('weightPriceGroup.ratePerMaund').setValue(event.option.value.price);
    if (this.purchase === undefined || this.purchase === null) {
      this.purchase = new Purchase();
    }
    if (this.purchase.product === undefined || this.purchase.product === null) {
      this.purchase.product = new Product();
    }
    this.purchase.productId = event.option.value.id;
    this.purchase.product = event.option.value;
  }
  selectedVehicle(event: MatAutocompleteSelectedEvent) {
    this.purchaseForm.get('vehicleGroup').setValue({
      plateNo: event.option.value.plateNo
    }, { emitEvent: false });
    if (this.purchase === undefined || this.purchase === null) {
      this.purchase = new Purchase();
    }
    if (this.purchase.vehicle === undefined || this.purchase.vehicle === null) {
      this.purchase.vehicle = new Vehicle();
    }
    this.purchase.vehicleId = event.option.value.id;
    this.purchase.vehicle = event.option.value;
  }
  addCharges() {
    const formGroup = new FormGroup({
      id: new FormControl(0, Validators.required),
      addPrice: new FormControl(true, Validators.required),
      task: new FormControl(null, Validators.required),
      bagQuantity: new FormControl(null, [Validators.required, Validators.min(0)]),
      rate: new FormControl(null, [Validators.required, Validators.min(0)]),
      total: new FormControl(null, [Validators.required, Validators.min(0)]),
    });
    (this.purchaseForm.get('additionalCharges') as FormArray).push(formGroup);
    if (this.purchase === undefined || this.purchase === null) {
      this.purchase = new Purchase();
    }
    if (this.purchase.additionalCharges === undefined || this.purchase.additionalCharges === null) {
      this.purchase.additionalCharges = [];
    }
    this.purchase.additionalCharges.push(new AdditionalCharges());
  }
  
  deleteCharges(id: number) {
    (this.purchaseForm.get('additionalCharges') as FormArray).removeAt(id);
    this.purchase.additionalCharges.splice(id, 1);
  }

  getRateBasedOnTotal():number{
    return this.purchaseForm.get('weightPriceGroup.isMaundBasedRate').value == '1' ? 
    +this.purchaseForm.get('weightPriceGroup.rate').value * +this.purchaseForm.get('weightPriceGroup.totalMaund').value :  
    +this.purchaseForm.get('weightPriceGroup.rate').value * (+this.purchaseForm.get('weightPriceGroup.bagQuantity').value + 
    +this.purchaseForm.get('weightPriceGroup.boriQuantity').value)
  }

}
