import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { GatePassType, ProductType } from '../../../../shared/model/enums';
import { MatDialogRef, MatAutocompleteSelectedEvent } from '@angular/material';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { Product } from '../../../../shared/model/product.model';
import { Party } from '../../../../shared/model/party.model';
import { PartyService } from '../../../../shared/services/party.service';
import { VehicleService } from '../../../../shared/services/vehicle.service';
import { ProductService } from '../../../../shared/services/product.service';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import { PartyResponse } from '../../../../shared/model/party-response.model';
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
  partySuggestions: Party[];
  productSuggestions: Product[];
  isGatein = true;
  gatepassForm: FormGroup = new FormGroup({
    dateTime: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), Validators.required),
    type: new FormControl(+GatePassType.InwardGatePass, Validators.required),
    broker: new FormControl(''),
    biltyNumber: new FormControl(''),
    lotId: new FormControl(''),
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
      // isProcessedMaterial: new FormControl(false, Validators.required)
    }),
    weightPriceGroup: new FormGroup({
      bagQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      boriQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      weightPerBag: new FormControl(0, [Validators.required, Validators.min(1)]),
      kandaWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      emptyWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      netWeight: new FormControl(0, [Validators.required, Validators.min(0)]),
      maund: new FormControl(0, [Validators.required, Validators.min(0)]),
    })
  });
  public modalRef: MatDialogRef<GatepassModalComponent>;
  public isNew = true;
  public isDelete = false;
  private gatepass: Gatepass;
  constructor(
    private gatepassService: GatepassService,
    private partyService: PartyService,
    private vehicleService: VehicleService,
    private productService: ProductService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
    this.gatepassForm.get('type').valueChanges.subscribe(
      (value) => {
        if (value === +GatePassType.InwardGatePass) {
          this.gatepassForm.get('lotId').setErrors(null);
        } else {
          this.gatepassForm.get('lotId').setErrors({ 'required': true });
        }
        // this.gatepassForm.get('productGroup.type').setValue(value);
      });

    // this.gatepassForm.get('weightPriceGroup.bagQuantity').valueChanges.subscribe(
    //   (value) => {
    //     const kandaWeight = +value * +this.gatepassForm.get('weightPriceGroup').value.bagWeight;
    //     this.gatepassForm.get('weightPriceGroup').patchValue({
    //       kandaWeight: kandaWeight,
    //       totalMaund: (kandaWeight / 40)
    //     }, { emitEvent: false });
    //   }
    // );

    // this.gatepassForm.get('weightPriceGroup.bagWeight').valueChanges.subscribe(
    //   (value) => {
    //     const kandaWeight = +value * +this.gatepassForm.get('weightPriceGroup').value.bagQuantity;
    //     this.gatepassForm.get('weightPriceGroup').patchValue({
    //       kandaWeight: kandaWeight,
    //       totalMaund: (kandaWeight / 40)
    //     }, { emitEvent: false });
    //   }
    // );

    this.gatepassForm.get('weightPriceGroup').valueChanges.subscribe(
      (value) => {
        const kandaWeight = +value.kandaWeight;
        const emptyWeight = +value.emptyWeight;
        const netWeight = kandaWeight - emptyWeight;
        const maund = netWeight / 40;
        this.gatepassForm.get('weightPriceGroup').patchValue({
          kandaWeight: kandaWeight,
          emptyWeight: emptyWeight,
          netWeight: netWeight.toFixed(3),
          maund: maund.toFixed(3)
        }, { emitEvent: false });
      }
    );
    // this.gatepassForm.get('weightPriceGroup.netWeight').valueChanges.subscribe(
    //   (value) => {
    //     this.gatepassForm.get('weightPriceGroup').patchValue({
    //       maund: (+value / 40)
    //     }, { emitEvent: false });
    //   }
    // );
    // this.gatepassForm.get('weightPriceGroup.emptyWeight').valueChanges.subscribe(
    //   (value) => {

    //     this.gatepassForm.get('weightPriceGroup').patchValue({
    //       maund: (+value / 40)
    //     }, { emitEvent: false });
    //   }
    // );
    // this.gatepassForm.get('weightPriceGroup.kandaWeight').valueChanges.subscribe(
    //   (value) => {
    //     this.gatepassForm.get('weightPriceGroup').patchValue({
    //       maund: (+value / 40)
    //     }, { emitEvent: false });
    //   }
    // );

    this.gatepassForm.get('partyGroup.name').valueChanges.subscribe(
      (value: string) => {
        if (this.gatepass === undefined || this.gatepass === null) {
          this.gatepass = new Gatepass();
        }
        if (this.gatepass.party === undefined || this.gatepass.party === null) {
          this.gatepass.party = new Party();
        }
        this.gatepass.partyId = 0;
        this.gatepassForm.get('partyGroup.phoneNumber').reset();
        this.gatepassForm.get('partyGroup.address').reset();
        if (value) {
          this.partyService.getParties(5, 0, value).subscribe(
            (response: PartyResponse) => {
              this.partySuggestions = response.data;
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
        if (value) {
          this.productService.getProducts(5, 0, value, ProductType.All).subscribe(
            (response: ProductResponse) => {
              this.productSuggestions = response.data;
            },
            (error) => console.log(error)
          );
        }
      });
    this.gatepassForm.get('vehicleGroup.plateNo').valueChanges.subscribe(
      (value: string) => {
        if (this.gatepass === undefined || this.gatepass === null) {
          this.gatepass = new Gatepass();
        }
        if (this.gatepass.vehicle === undefined || this.gatepass.vehicle === null) {
          this.gatepass.vehicle = new Vehicle();
        }
        this.gatepass.vehicleId = 0;
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
      dateTime: moment(gatepass.dateTime).format().slice(0, 16),
      type: gatepass.type,
      broker: gatepass.broker,
      biltyNumber: gatepass.biltyNumber,
      lotId: gatepass.lotId,
      vehicleGroup: {
        plateNo: gatepass.vehicle.plateNo
      },
      productGroup: {
        name: gatepass.product.name,
        // isProcessedMaterial: gatepass.product.isProcessedMaterial
      },
      partyGroup: {
        name: gatepass.party.name,
        address: gatepass.party.address,
        phoneNumber: gatepass.party.phoneNumber
      },
      weightPriceGroup: {
        bagQuantity: gatepass.bagQuantity,
        boriQuantity: gatepass.boriQuantity,
        weightPerBag: gatepass.weightPerBag,
        kandaWeight: gatepass.kandaWeight,
        emptyWeight: gatepass.emptyWeight,
        netWeight: gatepass.netWeight,
        maund: gatepass.maund
      }
    }, { emitEvent: false });
    this.gatepassForm.get('lotId').disable();
    this.gatepassForm.get('type').disable();
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
      if (this.gatepass.party === undefined || this.gatepass.party === null) {
        this.gatepass.party = new Party();
        this.gatepass.partyId = 0;
      }
      if (this.gatepass.product === undefined || this.gatepass.product === null) {
        this.gatepass.product = new Product();
        this.gatepass.productId = 0;
      }
      if (this.gatepass.vehicle === undefined || this.gatepass.vehicle === null) {
        this.gatepass.vehicle = new Vehicle();
        this.gatepass.vehicleId = 0;
      }
      this.gatepass.dateTime = moment(this.gatepassForm.value.dateTime).format();
      if (this.gatepassForm.value.broker) {
        this.gatepass.broker = this.gatepassForm.value.broker;
      } else {
        this.gatepass.broker = this.gatepassForm.get('partyGroup').value.name;
      }
      this.gatepass.biltyNumber = this.gatepassForm.value.biltyNumber;
      if (this.isNew) {
        this.gatepass.lotId = this.gatepassForm.value.lotId ? +this.gatepassForm.value.lotId : 0;
        this.gatepass.type = +this.gatepassForm.value.type;
      }
      this.gatepass.lotYear = moment(this.gatepassForm.value.dateTime).year();
      this.gatepass.product.id = +this.gatepass.productId;
      this.gatepass.product.name = this.gatepassForm.get('productGroup').value.name;
      this.gatepass.product.createdDate = moment().format();

      this.gatepass.vehicle.id = +this.gatepass.vehicleId;
      this.gatepass.vehicle.plateNo = this.gatepassForm.get('vehicleGroup').value.plateNo;
      this.gatepass.vehicle.createdDate = moment().format();

      this.gatepass.party.id = +this.gatepass.partyId;
      this.gatepass.party.name = this.gatepassForm.get('partyGroup').value.name;
      this.gatepass.party.phoneNumber = this.gatepassForm.get('partyGroup').value.phoneNumber;
      this.gatepass.party.address = this.gatepassForm.get('partyGroup').value.address;
      this.gatepass.party.createdDate = moment().format();

      this.gatepass.bagQuantity = this.gatepassForm.get('weightPriceGroup').value.bagQuantity;
      this.gatepass.boriQuantity = this.gatepassForm.get('weightPriceGroup').value.boriQuantity;
      this.gatepass.weightPerBag = this.gatepassForm.get('weightPriceGroup').value.weightPerBag;
      this.gatepass.kandaWeight = this.gatepassForm.get('weightPriceGroup').value.kandaWeight;
      this.gatepass.emptyWeight = this.gatepassForm.get('weightPriceGroup').value.emptyWeight;
      this.gatepass.netWeight = this.gatepassForm.get('weightPriceGroup').value.netWeight;
      this.gatepass.maund = this.gatepassForm.get('weightPriceGroup').value.maund;

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

  selectedParty(event: MatAutocompleteSelectedEvent) {
    this.gatepassForm.get('partyGroup').setValue({
      name: event.option.value.name,
      phoneNumber: event.option.value.phoneNumber,
      address: event.option.value.address
    }, { emitEvent: false });
    if (this.gatepass === undefined || this.gatepass === null) {
      this.gatepass = new Gatepass();
    }
    if (this.gatepass.party === undefined || this.gatepass.party === null) {
      this.gatepass.party = new Party();
    }
    this.gatepass.partyId = event.option.value.id;
    this.gatepass.party = event.option.value;
  }

  selectedProduct(event: MatAutocompleteSelectedEvent) {
    this.gatepassForm.get('productGroup').setValue({
      name: event.option.value.name,
      // isProcessedMaterial: event.option.value.isProcessedMaterial
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
