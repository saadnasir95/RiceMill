import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { NotificationService } from '../../../../shared/services/notification.service';
import { Bank } from '../../../../shared/model/bank.model';
import { BankService } from '../../../../shared/services/bank.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';

@Component({
  selector: 'app-bank-modal',
  templateUrl: './bank-modal.component.html',
  styleUrls: ['./bank-modal.component.scss']
})

export class BankModalComponent implements OnInit {
  bankEnumList = Bank;
  public bankForm: FormGroup = new FormGroup({
    name: new FormControl(null, Validators.required),
  });
  public modalRef: MatDialogRef<BankModalComponent>;
  public isNew = true;
  public isDelete = false;
  private bank: Bank;
  constructor(
    private bankService: BankService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
  }

  closeModal() {
    this.modalRef.close();
  }

  editBank(bank: Bank) {
    this.isNew = false;
    this.bank = new Bank();
    Object.assign(this.bank, bank);
    this.bankForm.setValue({
      name: bank.name,
    });
  }
  deleteBank(bank: Bank) {
    this.isDelete = true;
    this.bank = new Bank();
    Object.assign(this.bank, bank);
  }
  delete() {
    this.spinner.isLoading = true;
    this.bankService.deleteBank(this.bank).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('Bank deleted successfully');
        this.bankService.bankEmitter.emit(true);
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
    if (this.bankForm.valid) {
      this.spinner.isLoading = true;
      if (this.isNew) {
        this.bank = new Bank();
        this.bank.id = 0;
        this.bank.name = this.bankForm.value.name;
        this.bankService.addBank(this.bank).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Bank added successfully');
            this.bankService.bankEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          }
        );
      } else {
        this.bank.name = this.bankForm.value.name;
        this.bankService.updateBank(this.bank).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Bank updated successfully');
            this.bankService.bankEmitter.emit(true);
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
