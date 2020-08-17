import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { ProductType, RateBasedOn, GatePassType } from '../../../../shared/model/enums';
import { MatDialogRef, MatAutocompleteSelectedEvent, MatChipInputEvent } from '@angular/material';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { Product } from '../../../../shared/model/product.model';
import { Party } from '../../../../shared/model/party.model';
import { PartyService } from '../../../../shared/services/party.service';
import { VehicleService } from '../../../../shared/services/vehicle.service';
import { ProductService } from '../../../../shared/services/product.service';
import { Sale } from '../../../../shared/model/sale.model';
import { SaleService } from '../../../../shared/services/sale.service';
import { AdditionalCharges } from '../../../../shared/model/additionalcharges.model';
import { PartyResponse } from '../../../../shared/model/party-response.model';
import { ProductResponse } from '../../../../shared/model/product-response.model';
import { VehicleResponse } from '../../../../shared/model/vehicle-response.model';
import * as moment from 'moment';
import 'moment-timezone';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SaleResponse } from '../../../../shared/model/sale-response.model';
import { SpinnerService } from '../../../../shared/services/spinner.service';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import { GatepassResponse } from '../../../../shared/model/gatepass-response.model';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Purchase } from '../../../../shared/model/purchase.model';
@Component({
  selector: 'app-sale-modal',
  templateUrl: './sale-modal.component.html',
  styleUrls: ['./sale-modal.component.scss']
})
export class SaleModalComponent implements OnInit {
  @ViewChild('gatepassInput') gatepassInput: ElementRef<HTMLInputElement>;
  selectedPartyId: number = 0;
  vehicleSuggestions: Vehicle[];
  partySuggestions: Party[];
  productSuggestions: Product[];
  additionalCharges = 0;
  commission = 0;
  basePrice = 0;
  selectedRateOnText: string;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  filteredGatepasses: Gatepass[];
  gatepasses: Gatepass[] = [];
  saleForm: FormGroup = new FormGroup({
    date: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), Validators.required),
    additionalCharges: new FormArray([]),
    gatepass: new FormControl(),
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

  public modalRef: MatDialogRef<SaleModalComponent>;
  public isNew = true;
  public isDelete = false;
  private sale: Sale;
  constructor(
    private saleService: SaleService,
    private partyService: PartyService,
    private vehicleService: VehicleService,
    private productService: ProductService,
    private notificationService: NotificationService,
    private gatepassService: GatepassService,
    public spinner: SpinnerService) { }

  ngOnInit() {
    this.saleForm.controls['gatepass'].valueChanges.subscribe(
      (response: string) => {
        this.gatepassService
        .getGatepassList(10, 0, response, 'false', '',true,GatePassType.OutwardGatePass)
        .subscribe(
          (response: GatepassResponse) => {
            this.filteredGatepasses = response.data;
          }
    )
  })

  this.saleForm.get('weightPriceGroup.isMaundBasedRate').valueChanges.subscribe(
    (response: string) => {
      if(+response == RateBasedOn.Maund){
        this.selectedRateOnText = "Maund";
      } 
      else if(+response == RateBasedOn.Bag) {
        this.selectedRateOnText = "Bag"; 
      }
  })

    this.saleForm.get('additionalCharges').valueChanges.subscribe(
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

          this.saleForm.get('additionalCharges').setValue(value, { emitEvent: false });
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

    this.saleForm.controls['gatepass'].setValue(null);
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
      this.saleForm.get('weightPriceGroup.totalMaund').setValue(
        (this.saleForm.get('weightPriceGroup.totalMaund').value - gatepass.maund).toFixed(2)
      );

      this.saleForm.get('weightPriceGroup.boriQuantity').setValue(
        (this.saleForm.get('weightPriceGroup.boriQuantity').value - gatepass.boriQuantity).toFixed(2)
      );

      this.saleForm.get('weightPriceGroup.bagQuantity').setValue(
        (this.saleForm.get('weightPriceGroup.bagQuantity').value - gatepass.bagQuantity).toFixed(2)
      );
    }

    if(this.gatepasses.length === 0){
      this.selectedPartyId = 0
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    if(!this.isGatepassExists(event.option.value)){
      this.gatepasses.push(event.option.value)
      this.saleForm.get('weightPriceGroup.totalMaund').setValue(
        +this.saleForm.get('weightPriceGroup.totalMaund').value + event.option.value.maund
      );

      this.saleForm.get('weightPriceGroup.boriQuantity').setValue(
        +this.saleForm.get('weightPriceGroup.boriQuantity').value + event.option.value.boriQuantity
      );

      this.saleForm.get('weightPriceGroup.bagQuantity').setValue(
        +this.saleForm.get('weightPriceGroup.bagQuantity').value + event.option.value.bagQuantity
      );
      this.selectedPartyId = event.option.value.party.id
    };
    this.gatepassInput.nativeElement.value = '';
    this.saleForm.controls['gatepass'].setValue(null);
  }

  isGatepassExists(gatepass: Gatepass): boolean{
    const findGatePass = this.gatepasses.find(_gatepass => _gatepass.id == gatepass.id)
    return findGatePass ? true : false 
  }

  closeModal() {
    this.modalRef.close();
  }

  editSale(sale: Sale) {
    this.isNew = false;
    this.sale = new Sale();
    Object.assign(this.sale, sale);
    this.commission = this.sale.commission;
    this.basePrice = this.sale.basePrice;

    this.saleForm.patchValue({
      date: moment.utc(sale.date).tz('Asia/Karachi').format().slice(0, 16),
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
        isMaundBasedRate: sale.rateBasedOn.toString(),
        totalMaund: sale.totalMaund,
        bagQuantity: sale.bagQuantity,
        boriQuantity: sale.boriQuantity,
        rate: sale.rate,
        totalPrice: sale.totalPrice,
        commission: sale.commission
      }
    }, { emitEvent: false });


    if (this.sale.additionalCharges.length >= 0) {
      for (let i = 0; i < this.sale.additionalCharges.length; i++) {
        const formGroup = new FormGroup({
          id: new FormControl(this.sale.additionalCharges[i].id, Validators.required),
          addPrice: new FormControl(this.sale.additionalCharges[i].addPrice, Validators.required),
          task: new FormControl(this.sale.additionalCharges[i].task, Validators.required),
          bagQuantity: new FormControl(this.sale.additionalCharges[i].bagQuantity, [Validators.required, Validators.min(0)]),
          rate: new FormControl(this.sale.additionalCharges[i].rate, [Validators.required, Validators.min(0)]),
          total: new FormControl(this.sale.additionalCharges[i].total, [Validators.required, Validators.min(0)]),
        });
        (this.saleForm.get('additionalCharges') as FormArray).push(formGroup);
        if (this.sale.additionalCharges[i].addPrice) {
          this.additionalCharges += this.sale.additionalCharges[i].total;
        } else {
          this.additionalCharges -= this.sale.additionalCharges[i].total;
        }
      }
    }
  }

  deleteSale(sale: Sale) {
    this.isDelete = true;
    this.sale = new Sale();
    Object.assign(this.sale, sale);

  }

  delete() {
    this.spinner.isLoading = true;
    this.saleService.deleteSale(this.sale).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('Sale deleted successfully');
        this.saleService.saleEmitter.emit(true);
        this.modalRef.close();
      },
      (error) => {
        this.spinner.isLoading = false;
        console.log(error);
        this.notificationService.errorNotifcation('Something went wrong');
      });
  }

  submit() {
    if (this.saleForm.valid) {
      if(this.gatepasses.length == 0){
        return
      }

      this.spinner.isLoading = true;
      if (this.sale === undefined || this.sale === null) {
        this.sale = new Purchase();
      }
     
      if (this.sale.additionalCharges === undefined || this.sale.additionalCharges === null) {
        this.sale.additionalCharges = [];
      }

      this.sale.date = moment(this.saleForm.value.date).utc().format(); 
      this.sale.gatepassIds = this.gatepasses.map(gatepass => gatepass.id);
      this.sale.rateBasedOn = this.saleForm.get('weightPriceGroup').value.isMaundBasedRate;
      this.sale.totalMaund = this.saleForm.get('weightPriceGroup').value.totalMaund;
      this.sale.bagQuantity = this.saleForm.get('weightPriceGroup').value.bagQuantity;
      this.sale.boriQuantity = this.saleForm.get('weightPriceGroup').value.boriQuantity;
      this.sale.rate = this.saleForm.get('weightPriceGroup').value.rate;
      this.sale.totalPrice = this.getRateBasedOnTotal() + this.saleForm.get('weightPriceGroup.commission').value + this.additionalCharges;
      this.sale.commission = this.saleForm.get('weightPriceGroup').value.commission;

      if (this.sale.additionalCharges.length >= 0 && (this.saleForm.get('additionalCharges') as FormArray).length >= 0) {
        for (let i = 0; i < (this.saleForm.get('additionalCharges') as FormArray).length; i++) {
          this.sale.additionalCharges[i].id = (this.saleForm.get('additionalCharges') as FormArray).at(i).value.id;
          this.sale.additionalCharges[i].addPrice = (this.saleForm.get('additionalCharges') as FormArray).at(i).value.addPrice;
          this.sale.additionalCharges[i].task = (this.saleForm.get('additionalCharges') as FormArray).at(i).value.task;
          this.sale.additionalCharges[i].bagQuantity = (this.saleForm.get('additionalCharges') as FormArray).at(i).value.bagQuantity;
          this.sale.additionalCharges[i].rate = (this.saleForm.get('additionalCharges') as FormArray).at(i).value.rate;
          this.sale.additionalCharges[i].total = (this.saleForm.get('additionalCharges') as FormArray).at(i).value.total;
        }
      }

      if (this.isNew) {
        this.saleService.addSale(this.sale).subscribe(
          (response: SaleResponse) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Sale Invoice added successfully');
            this.modalRef.close();
            this.saleService.saleEmitter.emit(response.data);
          },
          (error) => {
            console.log(error);
            this.spinner.isLoading = false;
            this.notificationService.errorNotifcation('Something went wrong');
          });

      } else {
        this.saleService.updateSale(this.sale).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Sale Invoice updated successfully');
            this.saleService.saleEmitter.emit(true);
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


  addCharges() {
    const formGroup = new FormGroup({
      id: new FormControl(0, Validators.required),
      addPrice: new FormControl(true, Validators.required),
      task: new FormControl(null, Validators.required),
      bagQuantity: new FormControl(null, [Validators.required, Validators.min(0)]),
      rate: new FormControl(null, [Validators.required, Validators.min(0)]),
      total: new FormControl(null, [Validators.required, Validators.min(0)]),
    });
    (this.saleForm.get('additionalCharges') as FormArray).push(formGroup);
    if (this.sale === undefined || this.sale === null) {
      this.sale = new Sale();
    }
    if (this.sale.additionalCharges === undefined || this.sale.additionalCharges === null) {
      this.sale.additionalCharges = [];
    }
    this.sale.additionalCharges.push(new AdditionalCharges());
  }
  deleteCharges(id: number) {
    (this.saleForm.get('additionalCharges') as FormArray).removeAt(id);
    this.sale.additionalCharges.splice(id, 1);
  }

  addCommission() {
    const formGroup = new FormGroup({
      rate: new FormControl(null, [Validators.required, Validators.min(0)]),
      total: new FormControl(null, [Validators.required, Validators.min(0)]),
    });
    (this.saleForm.get('commissions') as FormArray).push(formGroup);
  }
  deleteCommission(id: number) {
    (this.saleForm.get('commissions') as FormArray).removeAt(id);
  }

  getRateBasedOnTotal():number{
    return this.saleForm.get('weightPriceGroup.isMaundBasedRate').value == '1' ? 
    +this.saleForm.get('weightPriceGroup.rate').value * +this.saleForm.get('weightPriceGroup.totalMaund').value :  
    +this.saleForm.get('weightPriceGroup.rate').value * (+this.saleForm.get('weightPriceGroup.bagQuantity').value + 
    +this.saleForm.get('weightPriceGroup.boriQuantity').value)
  }

}
