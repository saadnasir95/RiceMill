import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource, MatDialogRef, MatSort, MatDialog, MatPaginator } from '@angular/material';
import { Product } from '../../../../shared/model/product.model';
import { ProductModalComponent } from '../product-modal/product-modal.component';
import { Subscription } from 'rxjs';
import { ProductService } from '../../../../shared/services/product.service';
import { ProductResponse } from '../../../../shared/model/product-response.model';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['Id', 'Name', 'CreatedDate', 'Action'];
  dataSource: MatTableDataSource<Product>;
  products: Product[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<ProductModalComponent>;
  productSubscription: Subscription;
  productSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private productService: ProductService, private matDialog: MatDialog) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getProducts();
    this.productSubscription = this.productService.productEmitter.subscribe(
      () => {
        this.paginator.pageIndex = 0;
        this.getProducts();
      }
    );
  }

  ngOnDestroy() {
    this.productSubscription.unsubscribe();
  }

  applyFilter(filterValue: string) {
    this.paginator.pageIndex = 0;
    this.productSearch = filterValue.trim().toLowerCase();
    this.getProducts();
  }

  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getProducts();
  }

  changePage() {
    this.getProducts();
  }

  openModal() {
    this.dialogRef = this.matDialog.open(ProductModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editProduct(product: Product) {
    this.dialogRef = this.matDialog.open(ProductModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editProduct(product);
  }
  deleteProduct(product: Product) {
    this.dialogRef = this.matDialog.open(ProductModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteProduct(product);
  }
  getProducts() {
    this.productService
      .getProducts(this.paginator.pageSize, this.paginator.pageIndex, this.productSearch, this.sortDirection, this.sortOrderBy)
      .subscribe(
        (response: ProductResponse) => {
          this.products = response.data;
          this.dataSource.data = this.products;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }
}
