import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatDialogRef } from '@angular/material';
import { BankAccountResponse } from '../../../../shared/model/bank-account-response.model';
import { BankAccount } from '../../../../shared/model/bank-account.model';
import { VoucherDetailType, VoucherType } from '../../../../shared/model/enums';
import { PartyResponse } from '../../../../shared/model/party-response.model';
import { Party } from '../../../../shared/model/party.model';
import { PurchaseResponse } from '../../../../shared/model/purchase-response.model';
import { Purchase } from '../../../../shared/model/purchase.model';
import { SaleResponse } from '../../../../shared/model/sale-response.model';
import { Sale } from '../../../../shared/model/sale.model';
import { Voucher } from '../../../../shared/model/voucher.model';
import { BankAccountService } from '../../../../shared/services/bank-account.service';
import { NotificationService } from '../../../../shared/services/notification.service';
import { PartyService } from '../../../../shared/services/party.service';
import { PurchaseService } from '../../../../shared/services/purchase.service';
import { SaleService } from '../../../../shared/services/sale.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
import { VoucherService } from '../../../../shared/services/voucher.service';

@Component({
  selector: 'app-voucher-modal',
  templateUrl: './voucher-modal.component.html',
  styleUrls: ['./voucher-modal.component.scss']
})
export class VoucherModalComponent implements OnInit {
  partySuggestions: Party[];
  saleSuggestions: Sale[];
  purchaseSuggestions: Purchase[];
  bankAccountSuggestions: BankAccount[];
  isGatein = true;
  voucherType = VoucherType;
  voucherDetailType = VoucherDetailType;
  isSaleVoucher = true;
  isBankVoucher = false;
  voucherForm: FormGroup = new FormGroup({
    type: new FormControl(+this.voucherType.Sale, Validators.required),
    detailType: new FormControl(+this.voucherDetailType.CashReceivable, Validators.required),
    voucherDetails: new FormArray([
      new FormGroup({
        partyId: new FormControl(''),
        party: new FormControl(''),
        saleId: new FormControl(''),
        sale: new FormControl(''),
        purchaseId: new FormControl(''),
        purchase: new FormControl(''),
        bankAccountId: new FormControl(''),
        bankAccount: new FormControl(''),
        credit: new FormControl(''),
        debit: new FormControl(''),
        remarks: new FormControl('')
      })
    ])
  });
  public modalRef: MatDialogRef<VoucherModalComponent>;
  public isNew = true;
  public isDelete = false;
  private voucher: Voucher;
  constructor(
    private voucherService: VoucherService,
    private partyService: PartyService,
    private saleService: SaleService,
    private purchaseService: PurchaseService,
    private bankAccountService: BankAccountService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
    this.voucherForm.get('type').valueChanges.subscribe(
      (value) => {
        if (value === +this.voucherType.Sale) {
          this.isSaleVoucher = true;
          this.setSaleInvoiceRequired();
        } else {
          this.isSaleVoucher = false;
          this.setPurchaseInvoiceRequired();
        }
      });
    this.voucherForm.get('detailType').valueChanges.subscribe(
      (value) => {
        if (value === +this.voucherDetailType.BankPayable || value === +this.voucherDetailType.BankReceivable) {
          this.isBankVoucher = true;
          this.setBankAccountRequired();
        } else {
          this.isBankVoucher = false;
          this.setBankAccountNotRequired();
        }
      });
  }
  setPurchaseInvoiceRequired() {
    for (const voucherDetail of this.voucherForm.get('voucherDetails')['controls']) {
      voucherDetail.get('purhcaseId').setErrors({ 'required': true });
      voucherDetail.get('saleId').setErrors(null);
    }
  }
  setSaleInvoiceRequired() {
    for (const voucherDetail of this.voucherForm.get('voucherDetails')['controls']) {
      voucherDetail.get('saleId').setErrors({ 'required': true });
      voucherDetail.get('purhcaseId').setErrors(null);
    }
  }
  setBankAccountRequired() {
    for (const voucherDetail of this.voucherForm.get('voucherDetails')['controls']) {
      voucherDetail.get('bankAccountId').setErrors({ 'required': true });
    }
  }
  setBankAccountNotRequired() {
    for (const voucherDetail of this.voucherForm.get('voucherDetails')['controls']) {
      voucherDetail.get('bankAccountId').setErrors(null);
    }
  }
  onPartyInput(value: string) {
    if (value) {
      this.partyService.getParties(5, 0, value).subscribe(
        (response: PartyResponse) => {
          this.partySuggestions = response.data;
        },
        (error) => console.log(error)
      );
    }
  }
  onSaleInput(value: string) {
    if (value) {
      this.saleService.getSaleList(5, 0, value).subscribe(
        (response: SaleResponse) => {
          this.saleSuggestions = response.data;
        },
        (error) => console.log(error)
      );
    }
  }
  onPurchaseInput(value: string) {
    if (value) {
      this.purchaseService.getPurchaseList(5, 0, value).subscribe(
        (response: PurchaseResponse) => {
          this.purchaseSuggestions = response.data;
        },
        (error) => console.log(error)
      );
    }
  }
  onBankAccountInput(value: string) {
    if (value) {
      this.bankAccountService.getBankAccounts(5, 0, value).subscribe(
        (response: BankAccountResponse) => {
          this.bankAccountSuggestions = response.data;
        },
        (error) => console.log(error)
      );
    }
  }
  closeModal() {
    this.modalRef.close();
  }

  editVoucher(voucher: Voucher) {
    this.isNew = false;
    this.voucher = new Voucher();
    Object.assign(this.voucher, voucher);
    // this.gatepassForm.setValue({
    //   dateTime: moment(gatepass.dateTime).format().slice(0, 16),
    //   type: gatepass.type,
    //   broker: gatepass.broker,
    //   biltyNumber: gatepass.biltyNumber,
    //   lotId: gatepass.lotId,
    //   vehicleGroup: {
    //     plateNo: gatepass.vehicle.plateNo
    //   },
    //   productGroup: {
    //     name: gatepass.product.name,
    //     // isProcessedMaterial: gatepass.product.isProcessedMaterial
    //   },
    //   partyGroup: {
    //     name: gatepass.party.name,
    //     address: gatepass.party.address,
    //     phoneNumber: gatepass.party.phoneNumber
    //   },
    //   weightPriceGroup: {
    //     bagQuantity: gatepass.bagQuantity,
    //     boriQuantity: gatepass.boriQuantity,
    //     weightPerBag: gatepass.weightPerBag,
    //     kandaWeight: gatepass.kandaWeight,
    //     emptyWeight: gatepass.emptyWeight,
    //     netWeight: gatepass.netWeight,
    //     maund: gatepass.maund
    //   }
    // }, { emitEvent: false });
    // this.gatepassForm.get('lotId').disable();
    // this.gatepassForm.get('type').disable();
  }

  deleteVoucher(voucher: Voucher) {
    this.isDelete = true;
    this.voucher = new Voucher();
    Object.assign(this.voucher, voucher);
  }

  delete() {
    this.spinner.isLoading = true;
    // this.gatepassService.deleteGatepass(this.gatepass).subscribe(
    //   (data) => {
    //     this.spinner.isLoading = false;
    //     this.notificationService.successNotifcation('Gatepass deleted successfully');
    //     this.gatepassService.gatepassEmitter.emit(true);
    //     this.modalRef.close();
    //   },
    //   (error) => {
    //     this.spinner.isLoading = false;
    //     console.log(error);
    //     this.notificationService.errorNotifcation('Something went wrong');
    //   });
  }

  submit() {
    // if (this.gatepassForm.valid) {
    //   this.spinner.isLoading = true;
    //   if (this.gatepass === undefined || this.gatepass === null) {
    //     this.gatepass = new Gatepass();
    //   }
    //   if (this.gatepass.party === undefined || this.gatepass.party === null) {
    //     this.gatepass.party = new Party();
    //     this.gatepass.partyId = 0;
    //   }
    //   if (this.gatepass.product === undefined || this.gatepass.product === null) {
    //     this.gatepass.product = new Product();
    //     this.gatepass.productId = 0;
    //   }
    //   if (this.gatepass.vehicle === undefined || this.gatepass.vehicle === null) {
    //     this.gatepass.vehicle = new Vehicle();
    //     this.gatepass.vehicleId = 0;
    //   }
    //   this.gatepass.dateTime = moment(this.gatepassForm.value.dateTime).format();
    //   if (this.gatepassForm.value.broker) {
    //     this.gatepass.broker = this.gatepassForm.value.broker;
    //   } else {
    //     this.gatepass.broker = this.gatepassForm.get('partyGroup').value.name;
    //   }
    //   this.gatepass.biltyNumber = this.gatepassForm.value.biltyNumber;
    //   if (this.isNew) {
    //     this.gatepass.lotId = this.gatepassForm.value.lotId ? +this.gatepassForm.value.lotId : 0;
    //     this.gatepass.type = +this.gatepassForm.value.type;
    //   }
    //   this.gatepass.lotYear = moment(this.gatepassForm.value.dateTime).year();
    //   this.gatepass.product.id = +this.gatepass.productId;
    //   this.gatepass.product.name = this.gatepassForm.get('productGroup').value.name;
    //   this.gatepass.product.createdDate = moment().format();

    //   this.gatepass.vehicle.id = +this.gatepass.vehicleId;
    //   this.gatepass.vehicle.plateNo = this.gatepassForm.get('vehicleGroup').value.plateNo;
    //   this.gatepass.vehicle.createdDate = moment().format();

    //   this.gatepass.party.id = +this.gatepass.partyId;
    //   this.gatepass.party.name = this.gatepassForm.get('partyGroup').value.name;
    //   this.gatepass.party.phoneNumber = this.gatepassForm.get('partyGroup').value.phoneNumber;
    //   this.gatepass.party.address = this.gatepassForm.get('partyGroup').value.address;
    //   this.gatepass.party.createdDate = moment().format();

    //   this.gatepass.bagQuantity = this.gatepassForm.get('weightPriceGroup').value.bagQuantity;
    //   this.gatepass.boriQuantity = this.gatepassForm.get('weightPriceGroup').value.boriQuantity;
    //   this.gatepass.weightPerBag = this.gatepassForm.get('weightPriceGroup').value.weightPerBag;
    //   this.gatepass.kandaWeight = this.gatepassForm.get('weightPriceGroup').value.kandaWeight;
    //   this.gatepass.emptyWeight = this.gatepassForm.get('weightPriceGroup').value.emptyWeight;
    //   this.gatepass.netWeight = this.gatepassForm.get('weightPriceGroup').value.netWeight;
    //   this.gatepass.maund = this.gatepassForm.get('weightPriceGroup').value.maund;

    //   if (this.isNew) {
    //     this.gatepassService.addGatepass(this.gatepass).subscribe(
    //       (response: GatepassResponse) => {
    //         this.spinner.isLoading = false;
    //         this.notificationService.successNotifcation('Gatepass added successfully');
    //         this.modalRef.close();
    //         this.gatepassService.gatepassEmitter.emit(response.data);
    //       },
    //       (error) => {
    //         this.spinner.isLoading = false;
    //         console.log(error);
    //         this.notificationService.errorNotifcation('Something went wrong');
    //       });
    //   } else {
    //     this.gatepassService.updateGatepass(this.gatepass).subscribe(
    //       (data) => {
    //         this.spinner.isLoading = false;
    //         this.notificationService.successNotifcation('Gatepass update successfully');
    //         this.gatepassService.gatepassEmitter.emit(true);
    //         this.modalRef.close();
    //       },
    //       (error) => {
    //         this.spinner.isLoading = false;
    //         console.log(error);
    //         this.notificationService.errorNotifcation('Something went wrong');
    //       });
    //   }
    // }
  }

  selectedParty(i: number, event: MatAutocompleteSelectedEvent) {
    (this.voucherForm.get('voucherDetails') as FormArray).at(i).patchValue({
      party: 'Party: ' + event.option.value.name + ' | ' + event.option.value.address,
      partyId: event.option.value.id
    });
  }
  selectedSale(i: number, event: MatAutocompleteSelectedEvent) {
    (this.voucherForm.get('voucherDetails') as FormArray).at(i).patchValue({
      // sale: 'Party: ' + event.option.value.name + ' | ' + event.option.value.address,
      saleId: event.option.value.id
    });
  }
  selectedPurchase(i: number, event: MatAutocompleteSelectedEvent) {
    (this.voucherForm.get('voucherDetails') as FormArray).at(i).patchValue({
      // sale: 'Party: ' + event.option.value.name + ' | ' + event.option.value.address,
      purchaseId: event.option.value.id
    });
  }
  selectedBankAccount(i: number, event: MatAutocompleteSelectedEvent) {
    (this.voucherForm.get('voucherDetails') as FormArray).at(i).patchValue({
      // sale: 'Party: ' + event.option.value.name + ' | ' + event.option.value.address,
      bankAccountId: event.option.value.id
    });
  }
  addVoucherDetail() {
    const formGroup = new FormGroup({
      partyId: new FormControl(''),
      party: new FormControl(''),
      saleId: new FormControl(''),
      sale: new FormControl(''),
      purchaseId: new FormControl(''),
      purchase: new FormControl(''),
      bankAccountId: new FormControl(''),
      bankAccount: new FormControl(''),
      credit: new FormControl(''),
      debit: new FormControl(''),
      remarks: new FormControl('')
    });
    (this.voucherForm.get('voucherDetails') as FormArray).push(formGroup);
  }
  deleteVoucherDetail(id: number) {
    (this.voucherForm.get('voucherDetails') as FormArray).removeAt(id);
    // this.sale.additionalCharges.splice(id, 1);
  }
}
