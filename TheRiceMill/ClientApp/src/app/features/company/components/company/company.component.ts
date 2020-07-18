import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource, MatDialogRef, MatPaginator, MatSort, MatDialog } from '@angular/material';
import { Company } from '../../../../shared/model/company.model';
import { CompanyModalComponent } from '../company-modal/company-modal.component';
import { Subscription } from 'rxjs';
import { CompanyService } from '../../../../shared/services/company.service';
import { CompanyResponse } from '../../../../shared/model/company-response.model';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.scss']
})
export class CompanyComponent implements OnInit, OnDestroy {

  displayedColumns: string[] = ['Name', 'PhoneNumber', 'Address', 'CreatedDate', 'Action'];
  dataSource: MatTableDataSource<Company>;
  companies: Company[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<CompanyModalComponent>;
  CompanySubscription: Subscription;
  companySearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private companyService: CompanyService, private matDialog: MatDialog) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getCompanies();
    this.CompanySubscription = this.companyService.companyEmitter.subscribe(
      () => {
        this.paginator.pageIndex = 0;
        this.getCompanies();
      }
    );
  }

  ngOnDestroy() {
    this.CompanySubscription.unsubscribe();
  }

  applyFilter(filterValue: string) {
    this.paginator.pageIndex = 0;
    this.companySearch = filterValue.trim().toLowerCase();
    this.getCompanies();
  }
  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getCompanies();
  }
  changePage() {
    this.getCompanies();
  }
  openModal() {
    this.dialogRef = this.matDialog.open(CompanyModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editCompany(company: Company) {
    this.dialogRef = this.matDialog.open(CompanyModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editCompany(company);
  }
  deleteCompany(company: Company) {
    this.dialogRef = this.matDialog.open(CompanyModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteCompany(company);
  }
  getCompanies() {
    this.companyService
      .getCompanies(this.paginator.pageSize, this.paginator.pageIndex, this.companySearch, this.sortDirection, this.sortOrderBy)
      .subscribe(
        (response: CompanyResponse) => {
          this.companies = response.data;
          this.dataSource.data = this.companies;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }

}
