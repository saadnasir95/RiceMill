import { Component, OnInit, ElementRef, ViewChild, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { GatePassType, ProductType } from '../../../../shared/model/enums';
import { MatDialogRef, MatAutocompleteSelectedEvent, MatChipInputEvent, MAT_DIALOG_DATA } from '@angular/material';
import { Product } from '../../../../shared/model/product.model';
import { Gatepass } from '../../../../shared/model/gatepass.model';
import { GatepassService } from '../../../../shared/services/gatepass.service';
import * as moment from 'moment';
import 'moment-timezone';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
import { Purchase } from '../../../../shared/model/purchase.model';
import { ProcessedMaterial, CreateProcessedMaterial } from '../../../../shared/model/processed-material.model';
import { Lot } from '../../../../shared/model/lot.model';
import { LotService } from '../../../../shared/services/lot.service';
import { ProductService } from '../../../../shared/services/product.service';
import { ProductResponse } from '../../../../shared/model/product-response.model';
import { CreateRateCost, RateCost } from '../../../../shared/model/create-rate-cost.model';
@Component({
  selector: 'app-rate-cost-modal',
  templateUrl: './rate-cost-modal.component.html',
  styleUrls: ['./rate-cost-modal.component.scss']
})
export class RateCostModalComponent implements OnInit {
  @ViewChild('gatepassInput') gatepassInput: ElementRef<HTMLInputElement>;

  filteredGatepasses: Gatepass[];
  productSuggestions: Product[];
  totalKg = 0;
  commission = 0;
  basePrice = 0;
  processedMaterial: CreateProcessedMaterial;
  rateCost: RateCost;

  rateCostForm: FormGroup = new FormGroup({
    // date: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), Validators.required),
    labourUnloadingAndLoading: new FormControl(null, Validators.required),
    freight: new FormControl(null, Validators.required),
    purchaseBrokery: new FormControl(null, Validators.required),
    total: new FormControl(null, Validators.required),
    ratePer40WithoutProcessing : new FormControl(null, Validators.required),
    processingExpense : new FormControl(null, Validators.required),
    bardanaMisc : new FormControl(null, Validators.required),
    grandTotal : new FormControl(null, Validators.required),
    ratePer40LessByProduct: new FormControl(null, Validators.required),
    saleBrokery: new FormControl(null, Validators.required),
  });

  public modalRef: MatDialogRef<RateCostModalComponent>;
  public isNew = true;
  public isDelete = false;
  private lot: Lot;

  constructor(
    private notificationService: NotificationService,
    private lotService: LotService,
    public spinner: SpinnerService,
    public productService: ProductService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
  }

  ngOnInit() {
  }


  closeModal() {
    this.modalRef.close();
  }

  editRateCost(rateCost: CreateRateCost){
    this.isNew = false;
    debugger
    this.rateCost = new RateCost();
    Object.assign(this.rateCost, rateCost);
    this.rateCostForm.setValue({
      labourUnloadingAndLoading: rateCost.labourUnloadingAndLoading,
      freight: rateCost.freight,
      purchaseBrokery: rateCost.purchaseBrokery,
      total: rateCost.total,
      ratePer40WithoutProcessing : rateCost.ratePer40WithoutProcessing,
      processingExpense : rateCost.processingExpense,
      bardanaMisc : rateCost.bardanaAndMisc,
      grandTotal : rateCost.grandTotal,
      ratePer40LessByProduct: rateCost.ratePer40LessByProduct,
      saleBrokery: rateCost.saleBrockery
    }, { emitEvent: false });
  }


  submit() {
      const rateCost = new RateCost() 
      rateCost.lotId = +this.data.lotId;
      rateCost.lotYear = +this.data.lotYear;
      rateCost.bardanaAndMisc = this.rateCostForm.value.bardanaAndMisc;
      rateCost.freight = this.rateCostForm.value.freight;
      rateCost.grandTotal = this.rateCostForm.value.grandTotal;
      rateCost.labourUnloadingAndLoading = this.rateCostForm.value.labourUnloadingAndLoading; 
      rateCost.ratePer40LessByProduct = this.rateCostForm.value.ratePer40LessByProduct;
      rateCost.ratePer40WithoutProcessing = this.rateCostForm.value.ratePer40WithoutProcessing;
      rateCost.saleBrockery = this.rateCostForm.value.saleBrockery;
      rateCost.total = this.rateCostForm.value.total;
      rateCost.processingExpense = this.rateCostForm.value.processingExpense;
      rateCost.purchaseBrokery = this.rateCostForm.value.purchaseBrokery;
      if (this.isNew) {
        this.lotService.createRateCost(rateCost).subscribe(
          (response: any) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Rate Cost added successfully');
            this.modalRef.close();
            // this.lotService.purchaseEmitter.emit(response.data);
          },
          (error) => {
            console.log(error);
            this.spinner.isLoading = false;
            this.notificationService.errorNotifcation('Something went wrong');
          });

      } else {
        rateCost.id = this.rateCost.id;
        this.lotService.updateRateCost(rateCost).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Rate Cost updated successfully');
            // this.lotService.purchaseEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          });
      }
  //   }
  }

  selectedProduct(event: MatAutocompleteSelectedEvent, index: number) {
    (<FormArray>this.rateCostForm.controls['processedMaterial']).at(index).patchValue({
      productId: event.option.value.id,
    }, { emitEvent: false });
    if (this.processedMaterial === undefined || this.processedMaterial === null) {
      this.processedMaterial = new CreateProcessedMaterial();
    }
    if (this.processedMaterial.lotId === undefined || this.processedMaterial.lotId === null) {
      // this.processedMaterial.product = new Product();
    }
    // this.processedMaterial.item = event.option.value.id;
  }


  addProcessedMaterial() {
    const formGroup = new FormGroup({
      id: new FormControl(0, Validators.required),
      productId: new FormControl(null, Validators.required),
      boriQuantity: new FormControl(null, [Validators.required, Validators.min(0)]),
      bagQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      perKg: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalKg: new FormControl(null, [Validators.required, Validators.min(0)]),
      lotId: new FormControl(this.data.lotId, [Validators.required, Validators.min(0)]),
    });
    (this.rateCostForm.get('processedMaterial') as FormArray).push(formGroup);
    if (this.lot === undefined || this.lot === null) {
      this.lot = new Lot();
    }
    if (this.lot.processedMaterials === undefined || this.lot.processedMaterials === null) {
      this.lot.processedMaterials = [];
    }
    this.lot.processedMaterials.push(new ProcessedMaterial());
  }

  searchProduct(name){
    setTimeout(() => {
      if (name) {
        this.productService.getProducts(5, 0, name, ProductType.All).subscribe(
          (response: ProductResponse) => {
            this.productSuggestions = response.data;
          },
          (error) => console.log(error)
        );
      }
    },500)
  }

  deleteCharges(id: number) {
    (this.rateCostForm.get('processedMaterial') as FormArray).removeAt(id);
    this.lot.processedMaterials.splice(id, 1);
  }


}
