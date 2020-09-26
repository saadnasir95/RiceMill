import { Component, OnInit, ElementRef, ViewChild, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { ProductType } from '../../../../shared/model/enums';
import { MatDialogRef, MatAutocompleteSelectedEvent, MatChipInputEvent, MAT_DIALOG_DATA } from '@angular/material';
import { Product } from '../../../../shared/model/product.model';
import * as moment from 'moment';
import 'moment-timezone';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
import { ProcessedMaterial, CreateProcessedMaterial } from '../../../../shared/model/processed-material.model';
import { Lot } from '../../../../shared/model/lot.model';
import { LotService } from '../../../../shared/services/lot.service';
import { ProductService } from '../../../../shared/services/product.service';
import { ProductResponse } from '../../../../shared/model/product-response.model';
@Component({
  selector: 'app-gatepass-modal',
  templateUrl: './lot-modal.component.html',
  styleUrls: ['./lot-modal.component.scss']
})
export class LotModalComponent implements OnInit {
  productSuggestions: Product[];
  totalKg = 0;
  commission = 0;
  basePrice = 0;
  processedMaterial: CreateProcessedMaterial;

  lotForm: FormGroup = new FormGroup({
    date: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), Validators.required),
    processedMaterial: new FormArray([]),
  });

  public modalRef: MatDialogRef<LotModalComponent>;
  public isNew = true;
  public isDelete = false;
  lotId = 0;
  lotYear = 0;
  private lot: Lot;

  constructor(
    // private purchaseService: PurchaseService,
    private notificationService: NotificationService,
    private lotService: LotService,
    public spinner: SpinnerService,
    public productService: ProductService) {
  }

  ngOnInit() {
    // this.lotForm.get('processedMaterial').valueChanges.subscribe(
    //   (value: Array<any>) => {
    //     this.totalKg = 0;
    //     if (value.length !== 0) {
    //       for (let i = 0; i < value.length; i++) {
    //         value[i].totalKg = (+value[i].bagQuantity + (+value[i].boriQuantity)) * +value[i].perKg;
    //       }
    //       this.lotForm.get('processedMaterial').setValue(value, { emitEvent: false });
    //     }
    //   }
    // );
  }

  closeModal() {
    this.modalRef.close();
  }
  populateLotData(lotId: number, lotYear: number, processedMaterials: ProcessedMaterial[]) {
    this.lotId = lotId;
    this.lotYear = lotYear;
    if (!this.lot) {
      this.lot = new Lot();
    }
    if (!this.lot.processedMaterials) {
      this.lot.processedMaterials = [];
    }
    if (processedMaterials && processedMaterials.length > 0) {
      this.isNew = false;
      this.lot.processedMaterials = processedMaterials;
      this.populateProcessedMaterialData();
    } else {
      this.productService.getProducts(20, 0, '', ProductType.ProcessedMaterial).subscribe(
        (response: ProductResponse) => {
          for (let i = 0; i < response.data.length; i++) {
            const processedMaterial = new ProcessedMaterial();
            processedMaterial.boriQuantity = 0;
            processedMaterial.bagQuantity = 0;
            processedMaterial.id = 0;
            processedMaterial.perKG = 0;
            processedMaterial.totalKG = 0;
            processedMaterial.productId = response.data[i].id;
            processedMaterial.product = new Product();
            processedMaterial.product.name = response.data[i].name;
            this.lot.processedMaterials.push(processedMaterial);
            this.populateProcessedMaterialData();
          }
        },
        (error) => console.log(error)
      );
    }
  }
  populateProcessedMaterialData() {
    for (let i = 0; i < this.lot.processedMaterials.length; i++) {
      const formGroup = new FormGroup({
        id: new FormControl(this.lot.processedMaterials[i].id, Validators.required),
        productId: new FormControl(this.lot.processedMaterials[i].productId, Validators.required),
        productName: new FormControl(this.lot.processedMaterials[i].product.name, Validators.required),
        boriQuantity: new FormControl(this.lot.processedMaterials[i].boriQuantity, [Validators.required, Validators.min(0)]),
        bagQuantity: new FormControl(this.lot.processedMaterials[i].bagQuantity, [Validators.required, Validators.min(0)]),
        perKg: new FormControl(this.lot.processedMaterials[i].perKG, [Validators.required, Validators.min(0)]),
        totalKg: new FormControl(this.lot.processedMaterials[i].totalKG, [Validators.required, Validators.min(0)]),
      });
      (this.lotForm.get('processedMaterial') as FormArray).push(formGroup);
    }
  }

  submit() {
    if (this.lotForm.valid) {
      this.spinner.isLoading = true;
      if (this.lot.processedMaterials.length >= 0 && (this.lotForm.get('processedMaterial') as FormArray).length >= 0) {
        for (let i = 0; i < (this.lotForm.get('processedMaterial') as FormArray).length; i++) {
          this.lot.processedMaterials[i].id = (this.lotForm.get('processedMaterial') as FormArray).at(i).value.id;
          this.lot.processedMaterials[i].bagQuantity = (this.lotForm.get('processedMaterial') as FormArray).at(i).value.bagQuantity;
          this.lot.processedMaterials[i].boriQuantity = (this.lotForm.get('processedMaterial') as FormArray).at(i).value.boriQuantity;
          this.lot.processedMaterials[i].productId = (this.lotForm.get('processedMaterial') as FormArray).at(i).value.productId;
          this.lot.processedMaterials[i].perKG = (this.lotForm.get('processedMaterial') as FormArray).at(i).value.perKg;
          this.lot.processedMaterials[i].totalKG = (this.lotForm.get('processedMaterial') as FormArray).at(i).value.totalKg;
        }
      }
      const createProcessedMaterial = new CreateProcessedMaterial();
      createProcessedMaterial.lotId = +this.lotId;
      createProcessedMaterial.lotYear = +this.lotYear;
      createProcessedMaterial.processedMaterials = this.lot.processedMaterials;
      if (this.isNew) {
        this.lotService.createProcessedMaterial(createProcessedMaterial).subscribe(
          (response: any) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Processed material added successfully');
            this.modalRef.close();
            this.lotService.lotEmitter.emit(response);
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          });

      } else {
        this.lotService.updateProcessedMaterial(createProcessedMaterial).subscribe(
          (response: any) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Processed material updated successfully');
            this.modalRef.close();
            this.lotService.lotEmitter.emit(response);
          },
          (error) => {
            console.log(error);
            this.spinner.isLoading = false;
            this.notificationService.errorNotifcation('Something went wrong');
          });
      }
    }
  }

  selectedProduct(event: MatAutocompleteSelectedEvent, index: number) {
    (<FormArray>this.lotForm.controls['processedMaterial']).at(index).patchValue({
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
      productName: new FormControl(null, Validators.required),
      boriQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      bagQuantity: new FormControl(0, [Validators.required, Validators.min(0)]),
      perKg: new FormControl(0, [Validators.required, Validators.min(0)]),
      totalKg: new FormControl(0, [Validators.required, Validators.min(0)]),
    });
    (this.lotForm.get('processedMaterial') as FormArray).push(formGroup);
    if (this.lot === undefined || this.lot === null) {
      this.lot = new Lot();
    }
    if (this.lot.processedMaterials === undefined || this.lot.processedMaterials === null) {
      this.lot.processedMaterials = [];
    }
    this.lot.processedMaterials.push(new ProcessedMaterial());
  }

  searchProduct(name) {
    setTimeout(() => {
      if (name) {
        this.productService.getProducts(5, 0, name, ProductType.All).subscribe(
          (response: ProductResponse) => {
            this.productSuggestions = response.data;
          },
          (error) => console.log(error)
        );
      }
    }, 500);
  }

  deleteProcessedMaterial(id: number) {
    (this.lotForm.get('processedMaterial') as FormArray).removeAt(id);
    this.lot.processedMaterials.splice(id, 1);
  }


}
