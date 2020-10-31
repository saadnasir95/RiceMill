import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { NotificationService } from '../../../../shared/services/notification.service';
import { BankAccount } from '../../../../shared/model/bank-account.model';
import { BankAccountService } from '../../../../shared/services/bank-account.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
import { BankService } from '../../../../shared/services/bank.service';
import { BankResponse } from '../../../../shared/model/bank-response.model';
import { Bank } from '../../../../shared/model/bank.model';

@Component({
  selector: 'app-bank-account-modal',
  templateUrl: './bank-account-modal.component.html',
  styleUrls: ['./bank-account-modal.component.scss']
})

export class BankAccountModalComponent implements OnInit {
  bankList: Bank[];
  public bankAccountForm: FormGroup = new FormGroup({
    bankId: new FormControl('', Validators.required),
    accountNumber: new FormControl(null, Validators.required),
    currentBalance: new FormControl(null, [Validators.required, Validators.min(0)]),
  });
  public modalRef: MatDialogRef<BankAccountModalComponent>;
  public isNew = true;
  public isDelete = false;
  private bankAccount: BankAccount;
  constructor(
    private bankAccountService: BankAccountService,
    private bankService: BankService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
    this.bankService
      .getBanks(10, 0)
      .subscribe(
        (response: BankResponse) => {
          this.bankList = response.data;
        },
        (error) => console.log(error)
      );
  }

  closeModal() {
    this.modalRef.close();
  }

  editBankAccount(bankAccount: BankAccount) {
    this.isNew = false;
    this.bankAccount = new BankAccount();
    Object.assign(this.bankAccount, bankAccount);
    this.bankAccountForm.setValue({
      bankId: bankAccount.bankId,
      accountNumber: bankAccount.accountNumber,
      currentBalance: bankAccount.currentBalance
    });
  }
  deleteBankAccount(bankAccount: BankAccount) {
    this.isDelete = true;
    this.bankAccount = new BankAccount();
    Object.assign(this.bankAccount, bankAccount);
  }
  delete() {
    this.spinner.isLoading = true;
    this.bankAccountService.deleteBankAccount(this.bankAccount).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('BankAccount deleted successfully');
        this.bankAccountService.bankAccountEmitter.emit(true);
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
    if (this.bankAccountForm.valid) {
      this.spinner.isLoading = true;
      if (this.isNew) {
        this.bankAccount = new BankAccount();
        this.bankAccount.bankId = this.bankAccountForm.value.bankId;
        this.bankAccount.accountNumber = this.bankAccountForm.value.accountNumber;
        this.bankAccount.currentBalance = this.bankAccountForm.value.currentBalance;
        this.bankAccountService.addBankAccount(this.bankAccount).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Bank account added successfully');
            this.bankAccountService.bankAccountEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          }
        );
      } else {
        this.bankAccount.bankId = this.bankAccountForm.value.bankId;
        this.bankAccount.accountNumber = this.bankAccountForm.value.accountNumber;
        this.bankAccount.currentBalance = this.bankAccountForm.value.currentBalance;
        this.bankAccountService.updateBankAccount(this.bankAccount).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Bank account updated successfully');
            this.bankAccountService.bankAccountEmitter.emit(true);
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
