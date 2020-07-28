import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource, MatDialogRef, MatPaginator, MatSort, MatDialog } from '@angular/material';
import { Party } from '../../../../shared/model/party.model';
import { PartyModalComponent } from '../party-modal/party-modal.component';
import { Subscription } from 'rxjs';
import { PartyService } from '../../../../shared/services/party.service';
import { PartyResponse } from '../../../../shared/model/party-response.model';

@Component({
  selector: 'app-party',
  templateUrl: './party.component.html',
  styleUrls: ['./party.component.scss']
})
export class PartyComponent implements OnInit, OnDestroy {

  displayedColumns: string[] = ['Name', 'PhoneNumber', 'Address', 'CreatedDate', 'Action'];
  dataSource: MatTableDataSource<Party>;
  parties: Party[];
  isLoadingData: Boolean = false;
  dialogRef: MatDialogRef<PartyModalComponent>;
  PartySubscription: Subscription;
  partySearch = '';
  sortDirection = 'false';
  sortOrderBy = '';
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private partyService: PartyService, private matDialog: MatDialog) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.paginator.pageSize = 10;
    this.getParties();
    this.PartySubscription = this.partyService.partyEmitter.subscribe(
      () => {
        this.paginator.pageIndex = 0;
        this.getParties();
      }
    );
  }

  ngOnDestroy() {
    this.PartySubscription.unsubscribe();
  }

  applyFilter(filterValue: string) {
    this.paginator.pageIndex = 0;
    this.partySearch = filterValue.trim().toLowerCase();
    this.getParties();
  }
  sortData() {
    this.paginator.pageIndex = 0;
    this.sortDirection = this.sort.direction === 'desc' ? 'true' : 'false';
    this.sortOrderBy = this.sort.active;
    this.getParties();
  }
  changePage() {
    this.getParties();
  }
  openModal() {
    this.dialogRef = this.matDialog.open(PartyModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
  }
  editParty(party: Party) {
    this.dialogRef = this.matDialog.open(PartyModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editParty(party);
  }
  deleteParty(party: Party) {
    this.dialogRef = this.matDialog.open(PartyModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteParty(party);
  }
  getParties() {
    this.partyService
      .getParties(this.paginator.pageSize, this.paginator.pageIndex, this.partySearch, this.sortDirection, this.sortOrderBy)
      .subscribe(
        (response: PartyResponse) => {
          this.parties = response.data;
          this.dataSource.data = this.parties;
          this.paginator.length = response.count;
        },
        (error) => console.log(error)
      );
  }

}
