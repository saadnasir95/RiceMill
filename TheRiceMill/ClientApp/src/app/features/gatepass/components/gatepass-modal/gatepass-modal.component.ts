import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { GateinDirection, ProductType, GatepassType } from '../../../../shared/model/enums';
import { MatDialogRef, MatAutocompleteSelectedEvent } from '@angular/material';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { Product } from '../../../../shared/model/product.model';
import { Company } from '../../../../shared/model/company.model';
import { CompanyService } from '../../../../shared/services/company.service';
import { VehicleService } from '../../../../shared/services/vehicle.service';
import { ProductService } from '../../../../shared/services/product.service';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import { CompanyResponse } from '../../../../shared/model/company-response.model';
import { VehicleResponse } from '../../../../shared/model/vehicle-response.model';
import { ProductResponse } from '../../../../shared/model/product-response.model';
import * as moment from 'moment';
import 'moment-timezone';
import { GatepassResponse } from '../../../../shared/model/gatepass-response.model';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
@Component({
  selector: 'app-gatepass-modal',
  templateUrl: './gatepass-modal.component.html',
  styleUrls: ['./gatepass-modal.component.scss']
})
export class GatepassModalComponent implements OnInit {
  vehicleSuggestions: Vehicle[];
  companySuggestions: Company[];
  productSuggestions: Product[];
  isGatein = true;
  gatepassForm: FormGroup = new FormGroup({
    checkDateTime: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), Validators.required),
    gatepassType: new FormControl(+GatepassType.Gatein, Validators.required),
    direction: new FormControl(+GateinDirection.Milling, Validators.required),
    biltyNumber: new FormControl('123', Validators.required),
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
      price: new FormControl(0, [Validators.required, Validators.min(0)]),
      type: new FormControl(+ProductType.Purchase, Validators.required)
    }),
    weightPriceGroup: new FormGroup({
      bagQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      bagWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      kandaWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalMaund: new FormControl(0, [Validators.required, Validators.min(0)]),
    })
  });
  public gatepassDirectionTypes = [
    { text: 'Milling', value: +GateinDirection.Milling },
    { text: 'Outside', value: +GateinDirection.Outside },
    { text: 'Stockpile', value: +GateinDirection.Stockpile },
  ];
  public modalRef: MatDialogRef<GatepassModalComponent>;
  public isNew = true;
  public isDelete = false;
  private gatepass: Gatepass;
  constructor(
    private gatepassService: GatepassService,
    private companyService: CompanyService,
    private vehicleService: VehicleService,
    private productService: ProductService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
    this.gatepassForm.get('gatepassType').valueChanges.subscribe(
      (value) => {
        if (value === +GatepassType.Gatein) {
          this.isGatein = true;
        } else {
          this.isGatein = false;
        }
        // this.gatepassForm.get('productGroup.type').setValue(value);
        if (this.isGatein) {
          this.gatepassForm.get('biltyNumber').setValue('123');
          this.gatepassForm.get('productGroup.type').setValue(+ProductType.Purchase);
        } else {
          this.gatepassForm.get('direction').setValue(+GateinDirection.Milling);
          this.gatepassForm.get('productGroup.type').setValue(+ProductType.Sale);
        }
      });

    this.gatepassForm.get('weightPriceGroup.bagQuantity').valueChanges.subscribe(
      (value) => {
        const kandaWeight = +value * +this.gatepassForm.get('weightPriceGroup').value.bagWeight;
        this.gatepassForm.get('weightPriceGroup').patchValue({
          kandaWeight: kandaWeight,
          totalMaund: (kandaWeight / 40)
        }, { emitEvent: false });
      }
    );

    this.gatepassForm.get('weightPriceGroup.bagWeight').valueChanges.subscribe(
      (value) => {
        const kandaWeight = +value * +this.gatepassForm.get('weightPriceGroup').value.bagQuantity;
        this.gatepassForm.get('weightPriceGroup').patchValue({
          kandaWeight: kandaWeight,
          totalMaund: (kandaWeight / 40)
        }, { emitEvent: false });
      }
    );

    this.gatepassForm.get('weightPriceGroup.kandaWeight').valueChanges.subscribe(
      (value) => {
        const bagQuantity = +this.gatepassForm.get('weightPriceGroup').value.bagQuantity;
        this.gatepassForm.get('weightPriceGroup').patchValue({
          bagWeight: +value / (bagQuantity === 0 ? 1 : bagQuantity),
          totalMaund: (+value / 40)
        }, { emitEvent: false });
      }
    );

    this.gatepassForm.get('companyGroup.name').valueChanges.subscribe(
      (value: string) => {
        if (this.gatepass === undefined || this.gatepass === null) {
          this.gatepass = new Gatepass();
        }
        if (this.gatepass.company === undefined || this.gatepass.company === null) {
          this.gatepass.company = new Company();
        }
        this.gatepass.companyId = 0;
        this.gatepassForm.get('companyGroup.phoneNumber').reset();
        this.gatepassForm.get('companyGroup.address').reset();
        if (value) {
          this.companyService.getCompanies(5, 0, value).subscribe(
            (response: CompanyResponse) => {
              this.companySuggestions = response.data;
            },
            (error) => console.log(error)
          );
        }
      });
    this.gatepassForm.get('productGroup.name').valueChanges.subscribe(
      (value: string) => {
        if (this.gatepass === undefined || this.gatepass === null) {
          this.gatepass = new Gatepass();
        }
        if (this.gatepass.product === undefined || this.gatepass.product === null) {
          this.gatepass.product = new Product();
        }
        this.gatepass.productId = 0;
        this.gatepassForm.get('productGroup.price').reset(0);
        if (value) {
          this.productService.getProducts(5, 0, value).subscribe(
            (response: ProductResponse) => {
              this.productSuggestions = response.data;
            },
            (error) => console.log(error)
          );
        }
      });
    this.gatepassForm.get('vehicleGroup.name').valueChanges.subscribe(
      (value: string) => {
        if (this.gatepass === undefined || this.gatepass === null) {
          this.gatepass = new Gatepass();
        }
        if (this.gatepass.vehicle === undefined || this.gatepass.vehicle === null) {
          this.gatepass.vehicle = new Vehicle();
        }
        this.gatepass.vehicleId = 0;
        this.gatepassForm.get('vehicleGroup.plateNo').reset();
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

  editGatepass(gatepass: Gatepass) {
    this.isNew = false;
    this.gatepass = new Gatepass();
    Object.assign(this.gatepass, gatepass);
    this.gatepassForm.setValue({
      checkDateTime: moment.utc(gatepass.checkDateTime).tz('Asia/Karachi').format().slice(0, 16),
      gatepassType: gatepass.type,
      direction: gatepass.direction,
      biltyNumber: gatepass.biltyNumber,
      vehicleGroup: {
        name: gatepass.vehicle.name,
        plateNo: gatepass.vehicle.plateNo
      },
      productGroup: {
        name: gatepass.product.name,
        price: gatepass.product.price,
        type: gatepass.product.price
      },
      companyGroup: {
        name: gatepass.company.name,
        address: gatepass.company.address,
        phoneNumber: gatepass.company.phoneNumber
      },
      weightPriceGroup: {
        bagQuantity: gatepass.bagQuantity,
        bagWeight: gatepass.bagWeight,
        kandaWeight: gatepass.kandaWeight,
        totalMaund: gatepass.totalMaund
      }
    }, { emitEvent: false });
  }

  deleteGatepass(gatepass: Gatepass) {
    this.isDelete = true;
    this.gatepass = new Gatepass();
    Object.assign(this.gatepass, gatepass);
  }

  delete() {
    this.spinner.isLoading = true;
    this.gatepassService.deleteGatepass(this.gatepass).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('Gatepass deleted successfully');
        this.gatepassService.gatepassEmitter.emit(true);
        this.modalRef.close();
      },
      (error) => {
        this.spinner.isLoading = false;
        console.log(error);
        this.notificationService.errorNotifcation('Something went wrong');
      });
  }

  submit() {
    if (this.gatepassForm.valid) {
      this.spinner.isLoading = true;
      if (this.gatepass === undefined || this.gatepass === null) {
        this.gatepass = new Gatepass();
      }
      if (this.gatepass.company === undefined || this.gatepass.company === null) {
        this.gatepass.company = new Company();
        this.gatepass.companyId = 0;
      }
      if (this.gatepass.product === undefined || this.gatepass.product === null) {
        this.gatepass.product = new Product();
        this.gatepass.productId = 0;
      }
      if (this.gatepass.vehicle === undefined || this.gatepass.vehicle === null) {
        this.gatepass.vehicle = new Vehicle();
        this.gatepass.vehicleId = 0;
      }
      this.gatepass.checkDateTime = moment(this.gatepassForm.value.checkDateTime).utc().format();
      this.gatepass.direction = +this.gatepassForm.value.direction;
      this.gatepass.type = +this.gatepassForm.value.gatepassType;
      this.gatepass.biltyNumber = +this.gatepassForm.value.biltyNumber;
      // this.gatepass.productId = +this.gatepass.productId;
      this.gatepass.product.id = +this.gatepass.productId;
      this.gatepass.product.name = this.gatepassForm.get('productGroup').value.name;
      this.gatepass.product.price = +this.gatepassForm.get('productGroup').value.price;
      this.gatepass.product.type = +this.gatepassForm.get('productGroup').value.type;
      this.gatepass.product.createdDate = moment.utc().format();

      // this.gatepass.vehicleId = +this.gatepass.vehicleId;
      this.gatepass.vehicle.id = +this.gatepass.vehicleId;
      this.gatepass.vehicle.name = this.gatepassForm.get('vehicleGroup').value.name;
      this.gatepass.vehicle.plateNo = this.gatepassForm.get('vehicleGroup').value.plateNo;
      this.gatepass.vehicle.createdDate = moment.utc().format();

      // this.gatepass.companyId = +this.gatepass.companyId;
      this.gatepass.company.id = +this.gatepass.companyId;
      this.gatepass.company.name = this.gatepassForm.get('companyGroup').value.name;
      this.gatepass.company.phoneNumber = this.gatepassForm.get('companyGroup').value.phoneNumber;
      this.gatepass.company.address = this.gatepassForm.get('companyGroup').value.address;
      this.gatepass.company.createdDate = moment.utc().format();

      this.gatepass.bagQuantity = this.gatepassForm.get('weightPriceGroup').value.bagQuantity;
      this.gatepass.bagWeight = this.gatepassForm.get('weightPriceGroup').value.bagWeight;
      this.gatepass.kandaWeight = this.gatepassForm.get('weightPriceGroup').value.kandaWeight;
      this.gatepass.totalMaund = this.gatepassForm.get('weightPriceGroup').value.totalMaund;

      if (this.isNew) {
        this.gatepassService.addGatepass(this.gatepass).subscribe(
          (response: GatepassResponse) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Gatepass added successfully');
            this.modalRef.close();
            this.gatepassService.gatepassEmitter.emit(response.data);
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          });
      } else {
        this.gatepassService.updateGatepass(this.gatepass).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Gatepass update successfully');
            this.gatepassService.gatepassEmitter.emit(true);
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
    this.gatepassForm.get('companyGroup').setValue({
      name: event.option.value.name,
      phoneNumber: event.option.value.phoneNumber,
      address: event.option.value.address
    }, { emitEvent: false });
    if (this.gatepass === undefined || this.gatepass === null) {
      this.gatepass = new Gatepass();
    }
    if (this.gatepass.company === undefined || this.gatepass.company === null) {
      this.gatepass.company = new Company();
    }
    this.gatepass.companyId = event.option.value.id;
    this.gatepass.company = event.option.value;
  }

  selectedProduct(event: MatAutocompleteSelectedEvent) {
    this.gatepassForm.get('productGroup').setValue({
      name: event.option.value.name,
      price: event.option.value.price,
      type: event.option.value.type
    }, { emitEvent: false });
    if (this.gatepass === undefined || this.gatepass === null) {
      this.gatepass = new Gatepass();
    }
    if (this.gatepass.product === undefined || this.gatepass.product === null) {
      this.gatepass.product = new Product();
    }
    this.gatepass.productId = event.option.value.id;
    this.gatepass.product = event.option.value;
  }

  selectedVehicle(event: MatAutocompleteSelectedEvent) {
    this.gatepassForm.get('vehicleGroup').setValue({
      name: event.option.value.name,
      plateNo: event.option.value.plateNo
    }, { emitEvent: false });
    if (this.gatepass === undefined || this.gatepass === null) {
      this.gatepass = new Gatepass();
    }
    if (this.gatepass.vehicle === undefined || this.gatepass.vehicle === null) {
      this.gatepass.vehicle = new Vehicle();
    }
    this.gatepass.vehicleId = event.option.value.id;
    this.gatepass.vehicle = event.option.value;
  }
}
