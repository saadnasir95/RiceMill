import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { Product } from '../../../../shared/model/product.model';
import { ProductService } from '../../../../shared/services/product.service';
import { ProductType } from '../../../../shared/model/enums';
import * as moment from 'moment';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
@Component({
  selector: 'app-product-modal',
  templateUrl: './product-modal.component.html',
  styleUrls: ['./product-modal.component.scss']
})
export class ProductModalComponent implements OnInit {

  public productForm: FormGroup = new FormGroup({
    name: new FormControl(null, Validators.required),
    isProcessedMaterial: new FormControl(false, Validators.required),
  });
  public productTypes = [
    { text: 'Sale', value: +ProductType.Sale },
    { text: 'Purchase', value: +ProductType.Purchase }
  ];
  public modalRef: MatDialogRef<ProductModalComponent>;
  public isNew = true;
  public isDelete = false;
  private product: Product;
  constructor(
    private productService: ProductService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
  }

  closeModal() {
    this.modalRef.close();
  }

  editProduct(product: Product) {
    this.isNew = false;
    this.product = new Product();
    Object.assign(this.product, product);
    this.productForm.setValue({
      name: product.name,
      isProcessedMaterial: product.isProcessedMaterial
    });
  }

  deleteProduct(product: Product) {
    this.isDelete = true;
    this.product = new Product();
    Object.assign(this.product, product);
  }
  
  delete() {
    this.spinner.isLoading = true;
    this.productService.deleteProduct(this.product).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('Product deleted successfully');
        this.productService.productEmitter.emit(true);
        this.modalRef.close();
      },
      (error) => {
        this.spinner.isLoading = false;
        console.log(error);
        this.notificationService.errorNotifcation('Something went wrong');
      }
    );
  }

  submit() {
    if (this.productForm.valid) {
      this.spinner.isLoading = true;
      if (this.isNew) {
        this.product = new Product();
        this.product.name = this.productForm.value.name;
        this.product.isProcessedMaterial = this.productForm.value.isProcessedMaterial;
        this.product.createdDate = moment.utc().format();
        this.productService.addProduct(this.product).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Product added successfully');
            this.productService.productEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          }
        );
      } else {
        this.product.name = this.productForm.value.name;
        this.product.isProcessedMaterial = this.productForm.value.isProcessedMaterial;
        this.productService.updateProduct(this.product).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Product updated successfully');
            this.productService.productEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          }
        );
      }
    }
  }

}
