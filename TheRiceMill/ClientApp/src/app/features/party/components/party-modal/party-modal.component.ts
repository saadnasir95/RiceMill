import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { Party } from '../../../../shared/model/party.model';
import { PartyService } from '../../../../shared/services/party.service';
import * as moment from 'moment';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
@Component({
  selector: 'app-party-modal',
  templateUrl: './party-modal.component.html',
  styleUrls: ['./party-modal.component.scss']
})
export class PartyModalComponent implements OnInit {

  public partyForm: FormGroup = new FormGroup({
    name: new FormControl(null, Validators.required),
    phoneNumber: new FormControl(null, [Validators.required, Validators.maxLength(12)]),
    address: new FormControl(null, Validators.required)
  });
  public modalRef: MatDialogRef<PartyModalComponent>;
  public isNew = true;
  public isDelete = false;
  private party: Party;
  constructor(
    private partyService: PartyService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
  }

  closeModal() {
    this.modalRef.close();
  }

  editParty(party: Party) {
    this.isNew = false;
    this.party = new Party();
    Object.assign(this.party, party);
    this.partyForm.setValue({
      name: party.name,
      phoneNumber: party.phoneNumber,
      address: party.address
    });
  }
  deleteParty(party: Party) {
    this.isDelete = true;
    this.party = new Party();
    Object.assign(this.party, party);
  }
  delete() {
    this.spinner.isLoading = true;
    this.partyService.deleteParty(this.party).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('Party deleted successfully');
        this.partyService.partyEmitter.emit(true);
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
    if (this.partyForm.valid) {
      this.spinner.isLoading = true;
      if (this.isNew) {
        this.party = new Party();
        this.party.name = this.partyForm.value.name;
        this.party.phoneNumber = this.partyForm.value.phoneNumber;
        this.party.address = this.partyForm.value.address;
        this.party.createdDate = moment.utc().format();
        this.partyService.addParty(this.party).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Party added successfully');
            this.partyService.partyEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          }
        );

      } else {
        this.party.name = this.partyForm.value.name;
        this.party.phoneNumber = this.partyForm.value.phoneNumber;
        this.party.address = this.partyForm.value.address;
        this.partyService.updateParty(this.party).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Party updated successfully');
            this.partyService.partyEmitter.emit(true);
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
