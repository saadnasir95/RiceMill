import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { Company } from '../../../../shared/model/company.model';
import { CompanyService } from '../../../../shared/services/company.service';
import * as moment from 'moment';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
@Component({
  selector: 'app-company-modal',
  templateUrl: './company-modal.component.html',
  styleUrls: ['./company-modal.component.scss']
})
export class CompanyModalComponent implements OnInit {

  public companyForm: FormGroup = new FormGroup({
    name: new FormControl(null, Validators.required),
    phoneNumber: new FormControl(null, [Validators.required, Validators.maxLength(12)]),
    address: new FormControl(null, Validators.required)
  });
  public modalRef: MatDialogRef<CompanyModalComponent>;
  public isNew = true;
  public isDelete = false;
  private company: Company;
  constructor(
    private companyService: CompanyService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
  }

  closeModal() {
    this.modalRef.close();
  }

  editCompany(company: Company) {
    this.isNew = false;
    this.company = new Company();
    Object.assign(this.company, company);
    this.companyForm.setValue({
      name: company.name,
      phoneNumber: company.phoneNumber,
      address: company.address
    });
  }
  deleteCompany(company: Company) {
    this.isDelete = true;
    this.company = new Company();
    Object.assign(this.company, company);
  }
  delete() {
    this.spinner.isLoading = true;
    this.companyService.deleteCompany(this.company).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('Company deleted successfully');
        this.companyService.companyEmitter.emit(true);
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
    if (this.companyForm.valid) {
      this.spinner.isLoading = true;
      if (this.isNew) {
        this.company = new Company();
        this.company.name = this.companyForm.value.name;
        this.company.phoneNumber = this.companyForm.value.phoneNumber;
        this.company.address = this.companyForm.value.address;
        this.company.createdDate = moment.utc().format();
        this.companyService.addCompany(this.company).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Company added successfully');
            this.companyService.companyEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          }
        );

      } else {
        this.company.name = this.companyForm.value.name;
        this.company.phoneNumber = this.companyForm.value.phoneNumber;
        this.company.address = this.companyForm.value.address;
        this.companyService.updateCompany(this.company).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Company updated successfully');
            this.companyService.companyEmitter.emit(true);
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
