import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { VehicleService } from '../../../../shared/services/vehicle.service';
import * as moment from 'moment';
import { NotificationService } from '../../../../shared/services/notification.service';
import { SpinnerService } from '../../../../shared/services/spinner.service';
@Component({
  selector: 'app-vehicle-modal',
  templateUrl: './vehicle-modal.component.html',
  styleUrls: ['./vehicle-modal.component.scss']
})
export class VehicleModalComponent implements OnInit {
  public vehicleForm: FormGroup = new FormGroup({
    name: new FormControl(null, Validators.required),
    plateNo: new FormControl(null, Validators.required)
  });
  public modalRef: MatDialogRef<VehicleModalComponent>;
  public isNew = true;
  public isDelete = false;
  private vehicle: Vehicle;
  constructor(
    private vehicleService: VehicleService,
    private notificationService: NotificationService,
    public spinner: SpinnerService) { }

  ngOnInit() {
  }

  closeModal() {
    this.modalRef.close();
  }

  editVehicle(vehicle: Vehicle) {
    this.isNew = false;
    this.vehicle = new Vehicle();
    Object.assign(this.vehicle, vehicle);
    this.vehicleForm.setValue({
      name: vehicle.name,
      plateNo: vehicle.plateNo
    });
  }
  deleteVehicle(vehicle: Vehicle) {
    this.isDelete = true;
    this.vehicle = new Vehicle();
    Object.assign(this.vehicle, vehicle);
  }
  delete() {
    this.spinner.isLoading = true;
    this.vehicleService.deleteVehicle(this.vehicle).subscribe(
      (data) => {
        this.spinner.isLoading = false;
        this.notificationService.successNotifcation('Vehicle deleted successfully');
        this.vehicleService.vehicleEmitter.emit(true);
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
    if (this.vehicleForm.valid) {
      this.spinner.isLoading = true;
      if (this.isNew) {
        this.vehicle = new Vehicle();
        this.vehicle.name = this.vehicleForm.value.name;
        this.vehicle.plateNo = this.vehicleForm.value.plateNo;
        this.vehicle.createdDate = moment.utc().format();
        this.vehicleService.addVehicle(this.vehicle).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Vehicle added successfully');
            this.vehicleService.vehicleEmitter.emit(true);
            this.modalRef.close();
          },
          (error) => {
            this.spinner.isLoading = false;
            console.log(error);
            this.notificationService.errorNotifcation('Something went wrong');
          }
        );
      } else {
        this.vehicle.name = this.vehicleForm.value.name;
        this.vehicle.plateNo = this.vehicleForm.value.plateNo;
        this.vehicleService.updateVehicle(this.vehicle).subscribe(
          (data) => {
            this.spinner.isLoading = false;
            this.notificationService.successNotifcation('Vehicle updated successfully');
            this.vehicleService.vehicleEmitter.emit(true);
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
