import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { HeadLevel, HeadType } from '../../../../shared/model/enums';
import { Head, Head1, Head2, Head3, Head4, Head5 } from '../../../../shared/model/head.model';
import { HeadService } from '../../../../shared/services/head.service';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
import { VehicleModalComponent } from '../../../vehicle/components/vehicle-modal/vehicle-modal.component';

@Component({
  selector: 'app-head-modal',
  templateUrl: './head-modal.component.html',
  styleUrls: ['./head-modal.component.scss']
})
export class HeadModalComponent implements OnInit {
  parentHeadId = 0;
  head: Head;
  headLevel: HeadLevel = HeadLevel.Level1;
  headType = HeadType;
  public headForm: FormGroup = new FormGroup({
    name: new FormControl(null, Validators.required),
    type: new FormControl(+this.headType.BalanceSheet, Validators.required)
  });
  public modalRef: MatDialogRef<HeadModalComponent>;
  public isNew = true;
  public isDelete = false;
  constructor(
    private notificationService: NotificationService,
    public spinner: SpinnerService,
    private headSerivce: HeadService) { }

  ngOnInit() {
  }

  closeModal() {
    this.modalRef.close();
  }
  addHead(head: Head, headLevel: HeadLevel, parentId: number) {
    this.head = { ...head };
    this.headLevel = headLevel;
    this.parentHeadId = parentId;
  }

  editHead(head: Head, headLevel: HeadLevel, parentId: number) {
    this.isNew = false;
    this.head = { ...head };
    this.headLevel = headLevel;
    this.parentHeadId = parentId;
    this.headForm.setValue({
      name: head.name,
      type: head.type
    });
  }
  deleteHead(head: Head) {
    this.isDelete = true;
    // this.vehicle = new Vehicle();
    // Object.assign(this.vehicle, vehicle);
  }
  delete() {
    this.spinner.isLoading = true;
    // this.vehicleService.deleteVehicle(this.vehicle).subscribe(
    //   (data) => {
    //     this.spinner.isLoading = false;
    //     this.notificationService.successNotifcation('Vehicle deleted successfully');
    //     this.vehicleService.vehicleEmitter.emit(true);
    //     this.modalRef.close();
    //   },
    //   (error) => {
    //     this.spinner.isLoading = false;
    //     console.log(error);
    //     this.notificationService.errorNotifcation('Something went wrong');
    //   }
    // );
  }
  submit() {
    if (this.headForm.valid) {
      this.spinner.isLoading = true;
      const head = this.getHeadObjectByHeadLevel();
      if (this.isNew) {
        switch (this.headLevel) {
          case HeadLevel.Level1:
            this.headSerivce.addHead1(<Head1>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head1 added successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
          case HeadLevel.Level2:
            this.headSerivce.addHead2(<Head2>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head2 added successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
          case HeadLevel.Level3:
            this.headSerivce.addHead3(<Head3>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head3 added successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
          case HeadLevel.Level4:
            this.headSerivce.addHead4(<Head4>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head4 added successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
          case HeadLevel.Level5:
            this.headSerivce.addHead5(<Head5>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head5 added successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
        }
      } else {
        switch (this.headLevel) {
          case HeadLevel.Level1:
            this.headSerivce.updateHead1(<Head1>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head1 updated successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
          case HeadLevel.Level2:
            this.headSerivce.updateHead2(<Head2>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head2 updated successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
          case HeadLevel.Level3:
            this.headSerivce.updateHead3(<Head3>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head3 updated successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
          case HeadLevel.Level4:
            this.headSerivce.updateHead4(<Head4>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head4 updated successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
          case HeadLevel.Level5:
            this.headSerivce.updateHead5(<Head5>head).subscribe(
              (data) => {
                this.spinner.isLoading = false;
                this.notificationService.successNotifcation('Head5 updated successfully');
                this.headSerivce.headEmitter.emit(true);
                this.modalRef.close();
              },
              (error) => {
                this.spinner.isLoading = false;
                console.log(error);
                this.notificationService.errorNotifcation('Something went wrong');
              }
            );
            break;
        }
      }
    }
  }
  getHeadObjectByHeadLevel() {
    const head: any = { ...this.head };
    head.name = this.headForm.get('name').value;
    head.type = this.headForm.get('type').value;
    switch (this.headLevel) {
      case HeadLevel.Level2:
        head.head1Id = this.parentHeadId;
        break;
      case HeadLevel.Level3:
        head.head2Id = this.parentHeadId;
        break;
      case HeadLevel.Level4:
        head.head3Id = this.parentHeadId;
        break;
      case HeadLevel.Level5:
        head.head4Id = this.parentHeadId;
        break;
    }
    return head;
  }
}
