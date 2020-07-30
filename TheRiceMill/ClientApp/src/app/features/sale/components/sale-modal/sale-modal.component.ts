import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { ProductType } from '../../../../shared/model/enums';
import { MatDialogRef, MatAutocompleteSelectedEvent } from '@angular/material';
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
@Component({
  selector: 'app-sale-modal',
  templateUrl: './sale-modal.component.html',
  styleUrls: ['./sale-modal.component.scss']
})
export class SaleModalComponent implements OnInit {
  vehicleSuggestions: Vehicle[];
  partySuggestions: Party[];
  productSuggestions: Product[];
  additionalCharges = 0;
  commission = 0;
  basePrice = 0;
  saleForm: FormGroup = new FormGroup({
    checkOut: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), Validators.required),
    biltyNumber: new FormControl(null, Validators.required),
    additionalCharges: new FormArray([]),
    // commissions: new FormArray([]),
    partyGroup: new FormGroup({
      name: new FormControl('', Validators.required),
      address: new FormControl('', Validators.required),
      phoneNumber: new FormControl(null, [Validators.required, Validators.maxLength(12)])
    }),
    vehicleGroup: new FormGroup({
      plateNo: new FormControl(null, Validators.required)
    }),
    productGroup: new FormGroup({
      name: new FormControl(null, Validators.required),
    }),
    weightPriceGroup: new FormGroup({
      bagQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      bagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      kandaWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      expectedBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalExpectedBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      emptyBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalEmptyBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      actualBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalActualBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalMaund: new FormControl(0, [Validators.required, Validators.min(0)]),
      ratePerKg: new FormControl(0, [Validators.required, Validators.min(0)]),
      ratePerMaund: new FormControl(0, [Validators.required, Validators.min(0)]),
      commission: new FormControl(0, [Validators.required, Validators.min(0)]),
      percentCommission: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalPrice: new FormControl(0, [Validators.required, Validators.min(0)]),
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
    public spinner: SpinnerService) { }

  ngOnInit() {
    this.saleForm.get('weightPriceGroup').valueChanges.subscribe(
      (value) => {
        const totalEmptyBagWeight = +value.bagQuantity * (+value.bagWeight / 1000);
        const emptyBagWeight = +totalEmptyBagWeight / (+value.bagQuantity === 0 ? 1 : +value.bagQuantity);
        const expectedBagWeight = +value.kandaWeight / (+value.bagQuantity === 0 ? 1 : +value.bagQuantity);
        const totalActualBagWeight = +value.kandaWeight - totalEmptyBagWeight;
        const totalMaund = totalActualBagWeight / 40;
        this.basePrice = Math.round(totalActualBagWeight * (+value.ratePerMaund / 40));
        this.commission = Math.round(+this.basePrice * (+value.percentCommission / 100));
        this.saleForm.get('weightPriceGroup').patchValue({
          totalEmptyBagWeight: totalEmptyBagWeight.toFixed(2),
          emptyBagWeight: emptyBagWeight.toFixed(2),
          expectedBagWeight: +expectedBagWeight.toFixed(3),
          totalExpectedBagWeight: +(expectedBagWeight * +value.bagQuantity).toFixed(3),
          totalActualBagWeight: +totalActualBagWeight.toFixed(3),
          actualBagWeight: +((totalActualBagWeight / (+value.bagQuantity === 0 ? 1 : +value.bagQuantity)).toFixed(3)),
          totalMaund: +totalMaund.toFixed(3),
          ratePerKg: +value.ratePerMaund / 40,
          commission: this.commission,
          totalPrice: this.basePrice + this.additionalCharges - this.commission,
        }, { emitEvent: false });

      }
    );
    this.saleForm.get('partyGroup.name').valueChanges.subscribe(
      (value: string) => {
        if (this.sale === undefined || this.sale === null) {
          this.sale = new Sale();
        }
        if (this.sale.party === undefined || this.sale.party === null) {
          this.sale.party = new Party();
        }
        this.sale.partyId = 0;
        this.saleForm.get('partyGroup.phoneNumber').reset();
        this.saleForm.get('partyGroup.address').reset();
        if (value) {
          this.partyService.getParties(5, 0, value).subscribe(
            (response: PartyResponse) => {
              this.partySuggestions = response.data;
            },
            (error) => console.log(error)
          );
        }
      });
    this.saleForm.get('productGroup.name').valueChanges.subscribe(
      (value: string) => {
        if (this.sale === undefined || this.sale === null) {
          this.sale = new Sale();
        }
        if (this.sale.product === undefined || this.sale.product === null) {
          this.sale.product = new Product();
        }
        this.sale.productId = 0;
        if (value) {
          this.productService.getProducts(5, 0, value).subscribe(
            (response: ProductResponse) => {
              this.productSuggestions = response.data;
            },
            (error) => console.log(error)
          );
        }
      });
    this.saleForm.get('vehicleGroup.plateNo').valueChanges.subscribe(
      (value: string) => {
        if (this.sale === undefined || this.sale === null) {
          this.sale = new Sale();
        }
        if (this.sale.vehicle === undefined || this.sale.vehicle === null) {
          this.sale.vehicle = new Vehicle();
        }
        this.sale.vehicleId = 0;
        if (value) {
          this.vehicleService.getVehicles(5, 0, value).subscribe(
            (response: VehicleResponse) => {
              this.vehicleSuggestions = response.data;
            },
            (error) => console.log(error)
          );
        }
      });
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
        const netPrice = +this.basePrice + +this.additionalCharges - +this.commission;
        this.saleForm.get('weightPriceGroup.totalPrice').setValue(netPrice, { eventEmit: false, onlySelf: true });
      }
    );
    // this.saleForm.get('commissions').valueChanges.subscribe(
    //   (value: Array<any>) => {
    //     this.commission = 0;
    //     if (value.length !== 0) {
    //       for (let i = 0; i < value.length; i++) {
    //         value[i].total = +this.basePrice * +value[i].rate;
    //         this.commission += +value[i].total;
    //       }
    //       this.saleForm.get('commissions').setValue(value, { emitEvent: false });
    //     }
    //     const netPrice = +this.basePrice + +this.additionalCharges - +this.commission;
    //     this.saleForm.get('weightPriceGroup.totalPrice').setValue(netPrice, { eventEmit: false, onlySelf: true });
    //   }
    // );
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
      checkOut: moment.utc(sale.checkOut).tz('Asia/Karachi').format().slice(0, 16),
      biltyNumber: sale.biltyNumber,
      vehicleGroup: {
        plateNo: sale.vehicle.plateNo
      },
      productGroup: {
        name: sale.product.name,
      },
      partyGroup: {
        name: sale.party.name,
        address: sale.party.address,
        phoneNumber: sale.party.phoneNumber
      },
      weightPriceGroup: {
        bagQuantity: sale.bagQuantity,
        bagWeight: sale.bagWeight,
        kandaWeight: sale.kandaWeight,
        emptyBagWeight: sale.expectedEmptyBagWeight,
        totalEmptyBagWeight: sale.totalExpectedEmptyBagWeight,
        expectedBagWeight: sale.expectedBagWeight,
        totalExpectedBagWeight: sale.totalExpectedBagWeight,
        actualBagWeight: sale.actualBagWeight,
        totalActualBagWeight: sale.totalActualBagWeight,
        totalMaund: sale.totalMaund,
        ratePerKg: sale.ratePerKg,
        ratePerMaund: sale.ratePerMaund,
        totalPrice: sale.totalPrice,
        biltyNumber: sale.biltyNumber,
        percentCommission: sale.percentCommission,
        commission: sale.commission,
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
      this.spinner.isLoading = true;
      if (this.sale === undefined || this.sale === null) {
        this.sale = new Sale();
      }
      if (this.sale.party === undefined || this.sale.party === null) {
        this.sale.party = new Party();
      }
      if (this.sale.product === undefined || this.sale.product === null) {
        this.sale.product = new Product();
      }
      if (this.sale.vehicle === undefined || this.sale.vehicle === null) {
        this.sale.vehicle = new Vehicle();
      }
      if (this.sale.additionalCharges === undefined || this.sale.additionalCharges === null) {
        this.sale.additionalCharges = [];
      }
      if (this.sale.additionalCharges === undefined || this.sale.additionalCharges === null) {
        this.sale.additionalCharges = [];
      }
      this.sale.checkOut = moment(this.saleForm.value.checkOut).utc().format();
      this.sale.biltyNumber = this.saleForm.value.biltyNumber;

      // this.sale.productId = +this.sale.productId;
      this.sale.product.id = +this.sale.productId;
      this.sale.product.name = this.saleForm.get('productGroup').value.name;
      this.sale.product.createdDate = moment.utc().format();

      // this.sale.vehicleId = +this.sale.vehicleId;
      this.sale.vehicle.id = +this.sale.vehicleId;
      this.sale.vehicle.plateNo = this.saleForm.get('vehicleGroup').value.plateNo;
      this.sale.vehicle.createdDate = moment.utc().format();

      // this.sale.partyId = +this.sale.partyId;
      this.sale.party.id = +this.sale.partyId;
      this.sale.party.name = this.saleForm.get('partyGroup').value.name;
      this.sale.party.phoneNumber = this.saleForm.get('partyGroup').value.phoneNumber;
      this.sale.party.address = this.saleForm.get('partyGroup').value.address;
      this.sale.party.createdDate = moment.utc().format();

      this.sale.bagQuantity = this.saleForm.get('weightPriceGroup').value.bagQuantity;
      this.sale.bagWeight = this.saleForm.get('weightPriceGroup').value.bagWeight;
      this.sale.kandaWeight = this.saleForm.get('weightPriceGroup').value.kandaWeight;
      this.sale.expectedEmptyBagWeight = this.saleForm.get('weightPriceGroup').value.emptyBagWeight;
      this.sale.totalExpectedEmptyBagWeight = this.saleForm.get('weightPriceGroup').value.totalEmptyBagWeight;
      this.sale.expectedBagWeight = this.saleForm.get('weightPriceGroup').value.expectedBagWeight;
      this.sale.totalExpectedBagWeight = this.saleForm.get('weightPriceGroup').value.totalExpectedBagWeight;
      this.sale.actualBagWeight = this.saleForm.get('weightPriceGroup').value.actualBagWeight;
      this.sale.totalActualBagWeight = this.saleForm.get('weightPriceGroup').value.totalActualBagWeight;
      this.sale.totalMaund = this.saleForm.get('weightPriceGroup').value.totalMaund;
      this.sale.ratePerKg = this.saleForm.get('weightPriceGroup').value.ratePerKg;
      this.sale.ratePerMaund = this.saleForm.get('weightPriceGroup').value.ratePerMaund;
      this.sale.totalPrice = this.saleForm.get('weightPriceGroup').value.totalPrice;
      this.sale.basePrice = this.basePrice;
      this.sale.commission = this.saleForm.get('weightPriceGroup').value.commission;
      this.sale.percentCommission = this.saleForm.get('weightPriceGroup').value.percentCommission;

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
            this.notificationService.successNotifcation('Sale added successfully');
            this.modalRef.close();
            this.saleService.saleEmitter.emit(response.data);
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          });

      } else {
        this.saleService.updateSale(this.sale).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Sale updated successfully');
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

  selectedParty(event: MatAutocompleteSelectedEvent) {
    this.saleForm.get('partyGroup').setValue({
      name: event.option.value.name,
      phoneNumber: event.option.value.phoneNumber,
      address: event.option.value.address
    }, { emitEvent: false });
    if (this.sale === undefined || this.sale === null) {
      this.sale = new Sale();
    }
    if (this.sale.party === undefined || this.sale.party === null) {
      this.sale.party = new Party();
    }
    this.sale.partyId = event.option.value.id;
    this.sale.party = event.option.value;
  }

  selectedProduct(event: MatAutocompleteSelectedEvent) {
    this.saleForm.get('productGroup').setValue({
      name: event.option.value.name,
    }, { emitEvent: false });
    this.saleForm.get('weightPriceGroup.ratePerMaund').setValue(event.option.value.price);
    if (this.sale === undefined || this.sale === null) {
      this.sale = new Sale();
    }
    if (this.sale.product === undefined || this.sale.product === null) {
      this.sale.product = new Product();
    }
    this.sale.productId = event.option.value.id;
    this.sale.product = event.option.value;
  }
  selectedVehicle(event: MatAutocompleteSelectedEvent) {
    this.saleForm.get('vehicleGroup').setValue({
      plateNo: event.option.value.plateNo
    }, { emitEvent: false });
    if (this.sale === undefined || this.sale === null) {
      this.sale = new Sale();
    }
    if (this.sale.vehicle === undefined || this.sale.vehicle === null) {
      this.sale.vehicle = new Vehicle();
    }
    this.sale.vehicleId = event.option.value.id;
    this.sale.vehicle = event.option.value;
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

}
