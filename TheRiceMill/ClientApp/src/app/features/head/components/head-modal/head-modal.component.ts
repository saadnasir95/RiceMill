import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { Head } from '../../../../shared/model/head.model';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
import { VehicleModalComponent } from '../../../vehicle/components/vehicle-modal/vehicle-modal.component';

@Component({
  selector: 'app-head-modal',
  templateUrl: './head-modal.component.html',
  styleUrls: ['./head-modal.component.scss']
})
export class HeadModalComponent implements OnInit {

  public headForm: FormGroup = new FormGroup({
    name: new FormControl(null, Validators.required)
  });
  public modalRef: MatDialogRef<HeadModalComponent>;
  public isNew = true;
  public isDelete = false;
  constructor(
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
  }

  closeModal() {
    this.modalRef.close();
  }

  editHead(head: Head) {
    this.isNew = false;
    // this.vehicle = new Vehicle();
    // Object.assign(this.vehicle, vehicle);
    // this.vehicleForm.setValue({
    //   plateNo: vehicle.plateNo
    // });
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
    // if (this.headForm.valid) {
    //   this.spinner.isLoading = true;
    //   if (this.isNew) {
    //     this.vehicle = new Vehicle();
    //     this.vehicle.plateNo = this.vehicleForm.value.plateNo;
    //     this.vehicle.createdDate = moment.utc().format();
    //     this.vehicleService.addVehicle(this.vehicle).subscribe(
    //       (data) => {
    //         this.spinner.isLoading = false;
    //         this.notificationService.successNotifcation('Vehicle added successfully');
    //         this.vehicleService.vehicleEmitter.emit(true);
    //         this.modalRef.close();
    //       },
    //       (error) => {
    //         this.spinner.isLoading = false;
    //         console.log(error);
    //         this.notificationService.errorNotifcation('Something went wrong');
    //       }
    //     );
    //   } else {
    //     this.vehicle.plateNo = this.vehicleForm.value.plateNo;
    //     this.vehicleService.updateVehicle(this.vehicle).subscribe(
    //       (data) => {
    //         this.spinner.isLoading = false;
    //         this.notificationService.successNotifcation('Vehicle updated successfully');
    //         this.vehicleService.vehicleEmitter.emit(true);
    //         this.modalRef.close();
    //       },
    //       (error) => {
    //         this.spinner.isLoading = false;
    //         console.log(error);
    //         this.notificationService.errorNotifcation('Something went wrong');
    //       }
    //     );
    //   }
    // }
  }
}
