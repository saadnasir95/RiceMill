import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BankTransactionService } from '../../../../shared/services/bank-transaction.service';
import { BankTransaction } from '../../../../shared/model/bank-transaction.model';
import { TransactionType, Bank, PaymentType } from '../../../../shared/model/enums';
import { Party } from '../../../../shared/model/party.model';
import { PartyService } from '../../../../shared/services/party.service';
import { PartyResponse } from '../../../../shared/model/party-response.model';
import * as moment from 'moment';
import 'moment-timezone';
import { NotificationService } from '../../../../shared/services/notification.service';
import { BankAccountService } from '../../../../shared/services/bank-account.service';
import { BankAccount } from '../../../../shared/model/bank-account.model';
import { BankAccountResponse } from '../../../../shared/model/bank-account-response.model';
import { SpinnerService } from '../../../../shared/services/spinner.service';

@Component({
  selector: 'app-bank-transaction-modal',
  templateUrl: './bank-transaction-modal.component.html',
  styleUrls: ['./bank-transaction-modal.component.scss']
})
export class BankTransactionModalComponent implements OnInit {
  isCash = false;
  partyList: Party[];
  bankAccountList: BankAccount[];
  public bankTransactionForm: FormGroup = new FormGroup({
    transactionType: new FormControl(+TransactionType.Credit, [Validators.required]),
    paymentType: new FormControl(+PaymentType.Cheque, [Validators.required]),
    transactionAmount: new FormControl(null, [Validators.required, Validators.min(0)]),
    transactionDate: new FormControl(moment.tz('Asia/Karachi').format().slice(0, 16), [Validators.required]),
    bankAccountId: new FormControl(null, Validators.required),
    chequeNumber: new FormControl(null, Validators.required),
    partyId: new FormControl(null, Validators.required)
  });
  public modalRef: MatDialogRef<BankTransactionModalComponent>;
  public isNew = true;
  public isDelete = false;
  private transaction: BankTransaction;
  constructor(
    private bankService: BankTransactionService,
    private partyService: PartyService,
    private bankAccountService: BankAccountService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
    this.partyService.getParties(100, 0).subscribe(
      (response: PartyResponse) => {
        this.partyList = response.data;
      }
    );
    this.bankAccountService.getBankAccounts(100, 0).subscribe(
      (response: BankAccountResponse) => {
        this.bankAccountList = response.data;
      }
    );
    this.bankTransactionForm.get('paymentType').valueChanges.subscribe(
      (value) => {
        if (value === +PaymentType.Cash) {
          this.isCash = true;
        } else {
          this.isCash = false;
        }
        if (this.isCash) {
          this.bankTransactionForm.get('chequeNumber').setValue('cash');
        } else {
          this.bankTransactionForm.get('chequeNumber').setValue('');
        }
      });
  }

  closeModal() {
    this.modalRef.close();
  }

  editTransaction(transaction: BankTransaction) {
    this.isNew = false;
    this.transaction = new BankTransaction();
    Object.assign(this.transaction, transaction);
    this.bankTransactionForm.setValue({
      transactionType: transaction.transactionType,
      paymentType: transaction.paymentType,
      transactionAmount: transaction.transactionAmount,
      bankAccountId: transaction.bankAccountId,
      chequeNumber: transaction.chequeNumber,
      partyId: transaction.partyId,
      transactionDate: moment.utc(transaction.transactionDate).tz('Asia/Karachi').format().slice(0, 16)
    });
    if (this.transaction.paymentType === PaymentType.Cash) {
      this.isCash = true;
    }
  }
  deleteTransaction(transaction: BankTransaction) {
    this.isDelete = true;
    this.transaction = new BankTransaction();
    Object.assign(this.transaction, transaction);
  }
  delete() {
    this.spinner.isLoading = true;
    this.bankService.deleteBankTransaction(this.transaction).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('Transaction deleted successfully');
        this.bankService.transactionEmitter.emit(true);
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
    if (this.bankTransactionForm.valid) {
      this.spinner.isLoading = true;
      if (this.isNew) {
        this.transaction = new BankTransaction();
        this.transaction.transactionType = +this.bankTransactionForm.value.transactionType;
        this.transaction.paymentType = +this.bankTransactionForm.value.paymentType;
        this.transaction.partyId = +this.bankTransactionForm.value.partyId;
        this.transaction.transactionAmount = +this.bankTransactionForm.value.transactionAmount;
        this.transaction.bankAccountId = this.bankTransactionForm.value.bankAccountId;
        this.transaction.chequeNumber = this.bankTransactionForm.value.chequeNumber;
        this.transaction.transactionDate = moment(this.bankTransactionForm.value.transactionDate).utc().format();
        this.bankService.addbankTransaction(this.transaction).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Transaction added successfully');
            this.bankService.transactionEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          }
        );
      } else {
        this.transaction.transactionType = +this.bankTransactionForm.value.transactionType;
        this.transaction.paymentType = +this.bankTransactionForm.value.paymentType;
        this.transaction.partyId = +this.bankTransactionForm.value.partyId;
        this.transaction.transactionAmount = +this.bankTransactionForm.value.transactionAmount;
        this.transaction.bankAccountId = this.bankTransactionForm.value.bankAccountId;
        this.transaction.chequeNumber = this.bankTransactionForm.value.chequeNumber;
        this.transaction.transactionDate = moment(this.bankTransactionForm.value.transactionDate).utc().format();
        this.bankService.updateBankTransaction(this.transaction).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Transaction updated successfully');
            this.bankService.transactionEmitter.emit(true);
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
