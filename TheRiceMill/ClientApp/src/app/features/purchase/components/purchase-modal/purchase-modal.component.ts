import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { GateinDirection, ProductType } from '../../../../shared/model/enums';
import { MatDialogRef, MatAutocompleteSelectedEvent } from '@angular/material';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { Product } from '../../../../shared/model/product.model';
import { Company } from '../../../../shared/model/company.model';
import { CompanyService } from '../../../../shared/services/company.service';
import { VehicleService } from '../../../../shared/services/vehicle.service';
import { ProductService } from '../../../../shared/services/product.service';
import { Purchase } from '../../../../shared/model/purchase.model';
import { PurchaseService } from '../../../../shared/services/purchase.service';
import { AdditionalCharges } from '../../../../shared/model/additionalcharges.model';
import { CompanyResponse } from '../../../../shared/model/company-response.model';
import { ProductResponse } from '../../../../shared/model/product-response.model';
import { VehicleResponse } from '../../../../shared/model/vehicle-response.model';
import * as moment from 'moment';
import 'moment-timezone';
import { NotificationService } from '../../../../shared/services/notification.service';
import { PurchaseResponse } from '../../../../shared/model/purchase-response.model';
import { SpinnerService } from '../../../../shared/services/spinner.service';
@Component({
  selector: 'app-purchase-modal',
  templateUrl: './purchase-modal.component.html',
  styleUrls: ['./purchase-modal.component.scss']
})
export class PurchaseModalComponent implements OnInit {
  vehicleSuggestions: Vehicle[];
  companySuggestions: Company[];
  productSuggestions: Product[];
  additionalCharges = 0;
  commission = 0;
  basePrice = 0;
  purchaseForm: FormGroup = new FormGroup({
    checkIn: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), Validators.required),
    direction: new FormControl(null, Validators.required),
    additionalCharges: new FormArray([]),
    companyGroup: new FormGroup({
      name: new FormControl('', Validators.required),
      address: new FormControl('', Validators.required),
      phoneNumber: new FormControl(null, [Validators.required, Validators.maxLength(12)])
    }),
    vehicleGroup: new FormGroup({
      name: new FormControl(null, Validators.required),
      plateNo: new FormControl(null, Validators.required)
    }),
    productGroup: new FormGroup({
      name: new FormControl(null, Validators.required),
      price: new FormControl(null, [Validators.required, Validators.min(0)]),
      type: new FormControl(+ProductType.Purchase, Validators.required)
    }),
    weightPriceGroup: new FormGroup({
      bagQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      bagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      kandaWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      expectedBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalExpectedBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      emptyBagWeight: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(2)]),
      totalEmptyBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      actualBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalActualBagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalMaund: new FormControl(0, [Validators.required, Validators.min(0)]),
      vibration: new FormControl(0, [Validators.required, Validators.min(0)]),
      ratePerKg: new FormControl(0, [Validators.required, Validators.min(0)]),
      ratePerMaund: new FormControl(0, [Validators.required, Validators.min(0)]),
      commission: new FormControl(0, [Validators.required, Validators.min(0)]),
      percentCommission: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalPrice: new FormControl(0, [Validators.required, Validators.min(0)]),
      actualBags: new FormControl(0, [Validators.required, Validators.min(0)]),
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
    private companyService: CompanyService,
    private vehicleService: VehicleService,
    private productService: ProductService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
    this.purchaseForm.get('weightPriceGroup.bagQuantity').valueChanges.subscribe(
      (value) => {
        const totalEmptyBagWeight = +value * +this.purchaseForm.get('weightPriceGroup.emptyBagWeight').value;
        const bagWeight = +this.purchaseForm.get('weightPriceGroup.bagWeight').value;
        const vibration = 0.2 * +value;
        const kandaWeight = +this.purchaseForm.get('weightPriceGroup.kandaWeight').value;
        const expectedBagWeight = kandaWeight / (+value === 0 ? 1 : +value);
        const totalActualBagWeight = kandaWeight - totalEmptyBagWeight - vibration;
        const ratePerMaund = +this.purchaseForm.get('weightPriceGroup.ratePerMaund').value;
        const totalMaund = totalActualBagWeight / 40;
        const percentCommission = +this.purchaseForm.get('weightPriceGroup.percentCommission').value;
        this.basePrice = Math.round(totalActualBagWeight * (+ratePerMaund / 40));
        this.commission = Math.round(totalMaund * percentCommission);
        this.purchaseForm.get('weightPriceGroup').patchValue({
          vibration: vibration,
          totalEmptyBagWeight: totalEmptyBagWeight,
          expectedBagWeight: +expectedBagWeight.toFixed(3),
          totalExpectedBagWeight: +(expectedBagWeight * +value).toFixed(3),
          totalActualBagWeight: +totalActualBagWeight.toFixed(3),
          actualBagWeight: +((totalActualBagWeight / (+value === 0 ? 1 : +value)).toFixed(3)),
          totalMaund: +totalMaund.toFixed(3),
          ratePerKg: ratePerMaund / 40,
          commission: this.commission,
          totalPrice: this.basePrice + +this.additionalCharges + this.commission,
          actualBags: Math.round(totalActualBagWeight / (bagWeight === 0 ? 1 : bagWeight))
        }, { emitEvent: false });
        this.purchaseForm.get('productGroup.price').setValue(ratePerMaund);
      }
    );

    this.purchaseForm.get('weightPriceGroup.kandaWeight').valueChanges.subscribe(
      (value) => {
        const bagQuantity = +this.purchaseForm.get('weightPriceGroup.bagQuantity').value;
        const totalEmptyBagWeight = +this.purchaseForm.get('weightPriceGroup.totalEmptyBagWeight').value;
        const bagWeight = +this.purchaseForm.get('weightPriceGroup.bagWeight').value;
        const vibration = +bagQuantity * 0.2;
        const expectedBagWeight = +value / (+bagQuantity === 0 ? 1 : +bagQuantity);
        const totalActualBagWeight = +value - totalEmptyBagWeight - vibration;
        const totalMaund = totalActualBagWeight / 40;
        const percentCommission = +this.purchaseForm.get('weightPriceGroup.percentCommission').value;
        const ratePerMaund = +this.purchaseForm.get('weightPriceGroup.ratePerMaund').value;
        this.basePrice = Math.round(totalActualBagWeight * (+ratePerMaund / 40));
        this.commission = Math.round(totalMaund * percentCommission);
        this.purchaseForm.get('weightPriceGroup').patchValue({
          vibration: vibration,
          expectedBagWeight: +expectedBagWeight.toFixed(3),
          totalExpectedBagWeight: +(expectedBagWeight * +bagQuantity).toFixed(3),
          totalActualBagWeight: +totalActualBagWeight.toFixed(3),
          actualBagWeight: +((totalActualBagWeight / (+bagQuantity === 0 ? 1 : +bagQuantity)).toFixed(3)),
          totalMaund: +totalMaund.toFixed(3),
          ratePerKg: ratePerMaund / 40,
          commission: this.commission,
          totalPrice: this.basePrice + +this.additionalCharges + this.commission,
          actualBags: Math.round(totalActualBagWeight / (bagWeight === 0 ? 1 : bagWeight))
        }, { emitEvent: false });
        this.purchaseForm.get('productGroup.price').setValue(ratePerMaund);
      }
    );

    this.purchaseForm.get('weightPriceGroup.emptyBagWeight').valueChanges.subscribe(
      (value) => {
        const bagQuantity = +this.purchaseForm.get('weightPriceGroup.bagQuantity').value;
        const totalEmptyBagWeight = bagQuantity * +value;
        const bagWeight = +this.purchaseForm.get('weightPriceGroup.bagWeight').value;
        const vibration = +this.purchaseForm.get('weightPriceGroup.vibration').value;
        const kandaWeight = +this.purchaseForm.get('weightPriceGroup.kandaWeight').value;
        const totalActualBagWeight = +kandaWeight - totalEmptyBagWeight - vibration;
        const totalMaund = totalActualBagWeight / 40;
        const percentCommission = +this.purchaseForm.get('weightPriceGroup.percentCommission').value;
        const ratePerMaund = +this.purchaseForm.get('weightPriceGroup.ratePerMaund').value;
        this.basePrice = Math.round(totalActualBagWeight * (+ratePerMaund / 40));
        this.commission = Math.round(totalMaund * percentCommission);
        this.purchaseForm.get('weightPriceGroup').patchValue({
          totalEmptyBagWeight: totalEmptyBagWeight,
          totalActualBagWeight: +totalActualBagWeight.toFixed(3),
          actualBagWeight: +((totalActualBagWeight / (+bagQuantity === 0 ? 1 : +bagQuantity)).toFixed(3)),
          totalMaund: +totalMaund.toFixed(3),
          ratePerKg: ratePerMaund / 40,
          commission: this.commission,
          totalPrice: this.basePrice + +this.additionalCharges + this.commission,
          actualBags: Math.round(totalActualBagWeight / (bagWeight === 0 ? 1 : bagWeight))
        }, { emitEvent: false });
        this.purchaseForm.get('productGroup.price').setValue(ratePerMaund);
      }
    );

    this.purchaseForm.get('weightPriceGroup.vibration').valueChanges.subscribe(
      (value) => {
        const bagQuantity = +this.purchaseForm.get('weightPriceGroup.bagQuantity').value;
        const totalEmptyBagWeight = +this.purchaseForm.get('weightPriceGroup.totalEmptyBagWeight').value;
        const bagWeight = +this.purchaseForm.get('weightPriceGroup.bagWeight').value;
        const kandaWeight = +this.purchaseForm.get('weightPriceGroup.kandaWeight').value;
        const totalActualBagWeight = +kandaWeight - totalEmptyBagWeight - +value;
        const totalMaund = totalActualBagWeight / 40;
        const percentCommission = +this.purchaseForm.get('weightPriceGroup.percentCommission').value;
        const ratePerMaund = +this.purchaseForm.get('weightPriceGroup.ratePerMaund').value;
        this.basePrice = Math.round(totalActualBagWeight * (+ratePerMaund / 40));
        this.commission = Math.round(totalMaund * percentCommission);
        this.purchaseForm.get('weightPriceGroup').patchValue({
          totalActualBagWeight: +totalActualBagWeight.toFixed(3),
          actualBagWeight: +((totalActualBagWeight / (+bagQuantity === 0 ? 1 : +bagQuantity)).toFixed(3)),
          totalMaund: +totalMaund.toFixed(3),
          ratePerKg: ratePerMaund / 40,
          commission: this.commission,
          totalPrice: this.basePrice + +this.additionalCharges + this.commission,
          actualBags: Math.round(totalActualBagWeight / (bagWeight === 0 ? 1 : bagWeight))
        }, { emitEvent: false });
        this.purchaseForm.get('productGroup.price').setValue(ratePerMaund);
      }
    );

    this.purchaseForm.get('weightPriceGroup.ratePerMaund').valueChanges.subscribe(
      (value) => {
        const totalActualBagWeight = +this.purchaseForm.get('weightPriceGroup.totalActualBagWeight').value;
        const totalMaund = totalActualBagWeight / 40;
        const percentCommission = +this.purchaseForm.get('weightPriceGroup.percentCommission').value;
        this.basePrice = Math.round(totalActualBagWeight * (+value / 40));
        this.commission = Math.round(totalMaund * percentCommission);
        this.purchaseForm.get('weightPriceGroup').patchValue({
          ratePerKg: +value / 40,
          commission: this.commission,
          totalPrice: this.basePrice + +this.additionalCharges + this.commission,
        }, { emitEvent: false });
        this.purchaseForm.get('productGroup.price').setValue(+value);
      }
    );

    this.purchaseForm.get('weightPriceGroup.percentCommission').valueChanges.subscribe(
      (value) => {
        const totalActualBagWeight = +this.purchaseForm.get('weightPriceGroup.totalActualBagWeight').value;
        const totalMaund = totalActualBagWeight / 40;
        const ratePerMaund = +this.purchaseForm.get('weightPriceGroup.ratePerMaund').value;
        this.basePrice = Math.round(totalActualBagWeight * (+ratePerMaund / 40));
        this.commission = Math.round(totalMaund * +value);
        this.purchaseForm.get('weightPriceGroup').patchValue({
          ratePerKg: +ratePerMaund / 40,
          commission: this.commission,
          totalPrice: this.basePrice + +this.additionalCharges + this.commission,
        }, { emitEvent: false });
        this.purchaseForm.get('productGroup.price').setValue(+ratePerMaund);
      }
    );

    this.purchaseForm.get('weightPriceGroup.bagWeight').valueChanges.subscribe(
      (value) => {
        const bagQuantity = +this.purchaseForm.get('weightPriceGroup.bagQuantity').value;
        const totalEmptyBagWeight = 0;
        const emptyBagWeight = 0;
        // const vibration = 0;
        const vibration = +this.purchaseForm.get('weightPriceGroup.vibration').value;
        const kandaWeight = bagQuantity * +value;
        const expectedBagWeight = kandaWeight / (+bagQuantity === 0 ? 1 : +bagQuantity);
        const totalActualBagWeight = kandaWeight - totalEmptyBagWeight - vibration;
        const ratePerMaund = +this.purchaseForm.get('weightPriceGroup.ratePerMaund').value;
        const totalMaund = totalActualBagWeight / 40;
        const percentCommission = +this.purchaseForm.get('weightPriceGroup.percentCommission').value;
        this.basePrice = Math.round(totalActualBagWeight * (+ratePerMaund / 40));
        this.commission = Math.round(totalMaund * percentCommission);
        this.purchaseForm.get('weightPriceGroup').patchValue({
          kandaWeight: kandaWeight,
          vibration: vibration,
          emptyBagWeight: emptyBagWeight,
          totalEmptyBagWeight: totalEmptyBagWeight,
          expectedBagWeight: +expectedBagWeight.toFixed(3),
          totalExpectedBagWeight: +(expectedBagWeight * +bagQuantity).toFixed(3),
          totalActualBagWeight: +totalActualBagWeight.toFixed(3),
          actualBagWeight: +((totalActualBagWeight / (bagQuantity === 0 ? 1 : bagQuantity)).toFixed(3)),
          totalMaund: +totalMaund.toFixed(3),
          ratePerKg: ratePerMaund / 40,
          commission: this.commission,
          totalPrice: this.basePrice + +this.additionalCharges + this.commission,
          actualBags: Math.round(totalActualBagWeight / (+value === 0 ? 1 : +value))
        }, { emitEvent: false });
        this.purchaseForm.get('productGroup.price').setValue(ratePerMaund);
      }
    );

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
        const netPrice = +this.basePrice + +this.additionalCharges + +this.commission;
        this.purchaseForm.get('weightPriceGroup.totalPrice').setValue(netPrice, { eventEmit: false, onlySelf: true });
      }
    );

    this.purchaseForm.get('companyGroup.name').valueChanges.subscribe(
      (value: string) => {
        if (this.purchase === undefined || this.purchase === null) {
          this.purchase = new Purchase();
        }
        if (this.purchase.company === undefined || this.purchase.company === null) {
          this.purchase.company = new Company();
        }
        this.purchase.companyId = 0;
        this.purchaseForm.get('companyGroup.phoneNumber').reset();
        this.purchaseForm.get('companyGroup.address').reset();
        if (value) {
          this.companyService.getCompanies(5, 0, value).subscribe(
            (response: CompanyResponse) => {
              this.companySuggestions = response.data;
            },
            (error) => console.log(error)
          );
        }
      });

    this.purchaseForm.get('productGroup.name').valueChanges.subscribe(
      (value: string) => {
        if (this.purchase === undefined || this.purchase === null) {
          this.purchase = new Purchase();
        }
        if (this.purchase.product === undefined || this.purchase.product === null) {
          this.purchase.product = new Product();
        }
        this.purchase.productId = 0;
        this.purchaseForm.get('productGroup.price').reset(0);
        this.purchaseForm.get('weightPriceGroup.ratePerMaund').reset(0);
        if (value) {
          this.productService.getProducts(5, 0, value).subscribe(
            (response: ProductResponse) => {
              this.productSuggestions = response.data;
            },
            (error) => console.log(error)
          );
        }
      });

    this.purchaseForm.get('vehicleGroup.name').valueChanges.subscribe(
      (value: string) => {
        if (this.purchase === undefined || this.purchase === null) {
          this.purchase = new Purchase();
        }
        if (this.purchase.vehicle === undefined || this.purchase.vehicle === null) {
          this.purchase.vehicle = new Vehicle();
        }
        this.purchase.vehicleId = 0;
        this.purchaseForm.get('vehicleGroup.plateNo').reset();
        if (value) {
          this.vehicleService.getVehicles(5, 0, value).subscribe(
            (response: VehicleResponse) => {
              this.vehicleSuggestions = response.data;
            },
            (error) => console.log(error)
          );
        }
      });
  }

  closeModal() {
    this.modalRef.close();
  }

  editPurchase(purchase: Purchase) {
    this.isNew = false;
    this.purchase = new Purchase();
    Object.assign(this.purchase, purchase);
    this.commission = this.purchase.commission;
    this.basePrice = this.purchase.basePrice;

    this.purchaseForm.patchValue({
      checkIn: moment.utc(purchase.checkIn).tz('Asia/Karachi').format().slice(0, 16),
      direction: purchase.direction,
      vehicleGroup: {
        name: purchase.vehicle.name,
        plateNo: purchase.vehicle.plateNo
      },
      productGroup: {
        name: purchase.product.name,
        price: purchase.product.price,
        type: purchase.product.price
      },
      companyGroup: {
        name: purchase.company.name,
        address: purchase.company.address,
        phoneNumber: purchase.company.phoneNumber
      },
      weightPriceGroup: {
        bagQuantity: purchase.bagQuantity,
        bagWeight: purchase.bagWeight,
        kandaWeight: purchase.kandaWeight,
        emptyBagWeight: purchase.expectedEmptyBagWeight,
        totalEmptyBagWeight: purchase.totalExpectedEmptyBagWeight,
        expectedBagWeight: purchase.expectedBagWeight,
        totalExpectedBagWeight: purchase.totalExpectedBagWeight,
        actualBagWeight: purchase.actualBagWeight,
        totalActualBagWeight: purchase.totalActualBagWeight,
        vibration: purchase.vibration,
        totalMaund: purchase.totalMaund,
        ratePerKg: purchase.ratePerKg,
        ratePerMaund: purchase.ratePerMaund,
        totalPrice: purchase.totalPrice,
        actualBags: purchase.actualBags,
        percentCommission: purchase.percentCommission,
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

  submit() {
    if (this.purchaseForm.valid) {
      this.spinner.isLoading = true;
      if (this.purchase === undefined || this.purchase === null) {
        this.purchase = new Purchase();
      }
      if (this.purchase.company === undefined || this.purchase.company === null) {
        this.purchase.company = new Company();
      }
      if (this.purchase.product === undefined || this.purchase.product === null) {
        this.purchase.product = new Product();
      }
      if (this.purchase.vehicle === undefined || this.purchase.vehicle === null) {
        this.purchase.vehicle = new Vehicle();
      }
      if (this.purchase.additionalCharges === undefined || this.purchase.additionalCharges === null) {
        this.purchase.additionalCharges = [];
      }

      this.purchase.checkIn = moment(this.purchaseForm.value.checkIn).utc().format();
      this.purchase.direction = +this.purchaseForm.value.direction;

      // this.purchase.productId = +this.purchase.product.id;
      this.purchase.product.id = +this.purchase.productId;
      this.purchase.product.name = this.purchaseForm.get('productGroup').value.name;
      this.purchase.product.price = +this.purchaseForm.get('productGroup').value.price;
      this.purchase.product.type = +ProductType.Purchase;
      this.purchase.product.createdDate = moment.utc().format();

      // this.purchase.vehicleId = +this.purchase.vehicle.id;
      this.purchase.vehicle.id = +this.purchase.vehicleId;
      this.purchase.vehicle.name = this.purchaseForm.get('vehicleGroup').value.name;
      this.purchase.vehicle.plateNo = this.purchaseForm.get('vehicleGroup').value.plateNo;
      this.purchase.vehicle.createdDate = moment.utc().format();

      // this.purchase.companyId = +this.purchase.companyId;
      this.purchase.company.id = +this.purchase.companyId;
      this.purchase.company.name = this.purchaseForm.get('companyGroup').value.name;
      this.purchase.company.phoneNumber = this.purchaseForm.get('companyGroup').value.phoneNumber;
      this.purchase.company.address = this.purchaseForm.get('companyGroup').value.address;
      this.purchase.company.createdDate = moment.utc().format();

      this.purchase.bagQuantity = this.purchaseForm.get('weightPriceGroup').value.bagQuantity;
      this.purchase.bagWeight = this.purchaseForm.get('weightPriceGroup').value.bagWeight;
      this.purchase.kandaWeight = this.purchaseForm.get('weightPriceGroup').value.kandaWeight;
      this.purchase.expectedEmptyBagWeight = this.purchaseForm.get('weightPriceGroup').value.emptyBagWeight;
      this.purchase.totalExpectedEmptyBagWeight = this.purchaseForm.get('weightPriceGroup').value.totalEmptyBagWeight;
      this.purchase.expectedBagWeight = this.purchaseForm.get('weightPriceGroup').value.expectedBagWeight;
      this.purchase.totalExpectedBagWeight = this.purchaseForm.get('weightPriceGroup').value.totalExpectedBagWeight;
      this.purchase.actualBagWeight = this.purchaseForm.get('weightPriceGroup').value.actualBagWeight;
      this.purchase.totalActualBagWeight = this.purchaseForm.get('weightPriceGroup').value.totalActualBagWeight;
      this.purchase.vibration = this.purchaseForm.get('weightPriceGroup').value.vibration;
      this.purchase.totalMaund = this.purchaseForm.get('weightPriceGroup').value.totalMaund;
      this.purchase.ratePerKg = this.purchaseForm.get('weightPriceGroup').value.ratePerKg;
      this.purchase.ratePerMaund = this.purchaseForm.get('weightPriceGroup').value.ratePerMaund;
      this.purchase.totalPrice = this.purchaseForm.get('weightPriceGroup').value.totalPrice;
      this.purchase.actualBags = this.purchaseForm.get('weightPriceGroup').value.actualBags;
      this.purchase.percentCommission = this.purchaseForm.get('weightPriceGroup').value.percentCommission;
      this.purchase.commission = this.purchaseForm.get('weightPriceGroup').value.commission;
      this.purchase.basePrice = this.basePrice;

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

  selectedCompany(event: MatAutocompleteSelectedEvent) {
    this.purchaseForm.get('companyGroup').setValue({
      name: event.option.value.name,
      phoneNumber: event.option.value.phoneNumber,
      address: event.option.value.address
    }, { emitEvent: false });
    if (this.purchase === undefined || this.purchase === null) {
      this.purchase = new Purchase();
    }
    if (this.purchase.company === undefined || this.purchase.company === null) {
      this.purchase.company = new Company();
    }
    this.purchase.companyId = event.option.value.id;
    this.purchase.company = event.option.value;
  }

  selectedProduct(event: MatAutocompleteSelectedEvent) {
    this.purchaseForm.get('productGroup').setValue({
      name: event.option.value.name,
      price: event.option.value.price,
      type: event.option.value.type
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
      name: event.option.value.name,
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

}
