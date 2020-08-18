import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Vehicle } from '../../../../shared/model/vehicle.model';
import { VehicleService } from '../../../../shared/services/vehicle.service';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogRef } from '@angular/material';
import { VehicleModalComponent } from '../vehicle-modal/vehicle-modal.component';
import { Subscription } from 'rxjs';
import { VehicleResponse } from '../../../../shared/model/vehicle-response.model';
import { CompanyService } from '../../../../shared/services/company.service';

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.scss']
})
export class VehicleComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['Id', 'PlateNo', 'CreatedDate', 'Action'];
  dataSource: MatTableDataSource<Vehicle>;
  vehicles: Vehicle[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<VehicleModalComponent>;
  vehicleSubscription: Subscription;
  companySubscription: Subscription;
  vehicleSearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  companyId = 0;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    private vehicleService: VehicleService,
    private matDialog: MatDialog,
    private companyService: CompanyService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getVehicles();
    this.vehicleSubscription = this.vehicleService.vehicleEmitter.subscribe(
      () => {
        this.paginator.pageIndex = 0;
        this.getVehicles();
      }
    );
    this.companySubscription = this.companyService.companySubject.subscribe(
      (companyId: number) => {
        if (this.companyId !== companyId) {
          this.companyId = companyId;
          this.paginator.pageIndex = 0;
          this.getVehicles();
        }
      }
    );
  }

  ngOnDestroy() {
    if (this.vehicleSubscription) {
      this.vehicleSubscription.unsubscribe();
    }
    if (this.companySubscription) {
      this.companySubscription.unsubscribe();
    }
  }

  applyFilter(filterValue: string) {
    this.vehicleSearch = filterValue.trim().toLowerCase();
    this.paginator.pageIndex = 0;
    this.getVehicles();
  }
  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getVehicles();
  }
  changePage() {
    this.getVehicles();
  }
  addVehicle() {
    this.dialogRef = this.matDialog.open(VehicleModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editVehicle(vehicle: Vehicle) {
    this.dialogRef = this.matDialog.open(VehicleModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editVehicle(vehicle);
  }
  deleteVehicle(vehicle: Vehicle) {
    this.dialogRef = this.matDialog.open(VehicleModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteVehicle(vehicle);
  }
  getVehicles() {
    this.vehicleService
      .getVehicles(this.paginator.pageSize, this.paginator.pageIndex, this.vehicleSearch, this.sortDirection, this.sortOrderBy)
      .subscribe(
        (response: VehicleResponse) => {
          this.vehicles = response.data;
          this.dataSource.data = this.vehicles;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }
}
