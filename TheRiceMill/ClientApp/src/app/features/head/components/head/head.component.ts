import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Subscription } from 'rxjs';
import { HeadLevel, HeadType } from '../../../../shared/model/enums';
import { HeadResponseModel } from '../../../../shared/model/head-response.model';
import { Head, Head1, Head3 } from '../../../../shared/model/head.model';
import { HeadService } from '../../../../shared/services/head.service';
import { HeadModalComponent } from '../head-modal/head-modal.component';

@Component({
  selector: 'app-head',
  templateUrl: './head.component.html',
  styleUrls: ['./head.component.scss'],
})
export class HeadComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['code', 'name', 'action'];
  panelOpenState = false;
  headType = HeadType;
  headLevel = HeadLevel;
  head1: Head1[] = [];
  headSubscription: Subscription;
  dialogRef: MatDialogRef<HeadModalComponent>;
  expandedLevel = HeadLevel.None;
  expandedId = 0;
  expandedDivFound = false;
  constructor(private matDialog: MatDialog, private headService: HeadService) { }

  ngOnInit() {
    this.getAllHeads();
    this.headSubscription = this.headService.headEmitter.subscribe(data => {
      this.getAllHeads();
    });
  }
  ngOnDestroy() {
    if (this.headSubscription) {
      this.headSubscription.unsubscribe();
    }
  }
  addHead(parent: any, level: HeadLevel) {
    this.expandedLevel = level;
    this.expandedId = parent ? +parent : 0;
    this.expandedDivFound = false;
    let code = '';
    switch (level) {
      case HeadLevel.Level1: {
        const totalHeads = this.head1.length;
        if (totalHeads > 0) {
          const lastHead = this.head1[totalHeads - 1];
          const codearray = lastHead.code.split('-');
          codearray[0] = String(+codearray[0] + 1).padStart(2, '0');
          code = codearray.join('-');
        } else {
          code = '01-00-00-00-0000';
        }
        break;
      }
      case HeadLevel.Level2: {
        const totalChilds = parent.head2.length;
        if (totalChilds > 0) {
          const lastChild = parent.head2[totalChilds - 1];
          const codearray = lastChild.code.split('-');
          codearray[1] = String(+codearray[1] + 1).padStart(2, '0');
          code = codearray.join('-');
        } else {
          const codearray = parent.code.split('-');
          codearray[1] = '01';
          code = codearray.join('-');
        }
        break;
      }
      case HeadLevel.Level3: {
        const totalChilds = parent.head3.length;
        if (totalChilds > 0) {
          const lastChild = parent.head3[totalChilds - 1];
          const codearray = lastChild.code.split('-');
          codearray[2] = String(+codearray[2] + 1).padStart(2, '0');
          code = codearray.join('-');
        } else {
          const codearray = parent.code.split('-');
          codearray[2] = '01';
          code = codearray.join('-');
        }
        break;
      }
      case HeadLevel.Level4: {
        const totalChilds = parent.head4.length;
        if (totalChilds > 0) {
          const lastChild = parent.head4[totalChilds - 1];
          const codearray = lastChild.code.split('-');
          codearray[3] = String(+codearray[3] + 1).padStart(2, '0');
          code = codearray.join('-');
        } else {
          const codearray = parent.code.split('-');
          codearray[3] = '01';
          code = codearray.join('-');
        }
        break;
      }
      case HeadLevel.Level5: {
        const totalChilds = parent.head5.length;
        if (totalChilds > 0) {
          const lastChild = parent.head5[totalChilds - 1];
          const codearray = lastChild.code.split('-');
          codearray[4] = String(+codearray[4] + 1).padStart(4, '0');
          code = codearray.join('-');
        } else {
          const codearray = parent.code.split('-');
          codearray[4] = '0001';
          code = codearray.join('-');
        }
        break;
      }
    }
    const parentHeadId = parent ? parent.id : 0;
    const head: Head = {
      type: HeadType.BalanceSheet,
      name: '',
      code: code,
      id: 0
    };
    this.dialogRef = this.matDialog.open(HeadModalComponent, {
      disableClose: true,
      width: '400px'
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.addHead(head, level, parentHeadId);
  }

  editHead(head: any, level: HeadLevel) {
    this.expandedLevel = level;
    this.expandedId = head.id;
    this.expandedDivFound = false;
    const edithead: Head = {
      id: head.id,
      type: head.type,
      code: head.code,
      name: head.name
    };
    let parentId = 0;
    switch (level) {
      case HeadLevel.Level2: {
        parentId = head.head1Id;
        break;
      }
      case HeadLevel.Level3: {
        parentId = head.head2Id;
        break;
      }
      case HeadLevel.Level4: {
        parentId = head.head3Id;
        break;
      }
      case HeadLevel.Level5: {
        parentId = head.head4Id;
        break;
      }
    }
    this.dialogRef = this.matDialog.open(HeadModalComponent, {
      disableClose: true,
      width: '400px',
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.editHead(head, level, parentId);
  }
  deleteHead(head: Head, level: HeadLevel) {
    this.dialogRef = this.matDialog.open(HeadModalComponent, {
      disableClose: true,
      width: '400px',
    });
    this.dialogRef.componentInstance.modalRef = this.dialogRef;
    this.dialogRef.componentInstance.deleteHead(head);
  }
  getAllHeads() {
    this.headService.getAllHeads(20, 0).subscribe(
      (response: HeadResponseModel) => {
        this.head1 = response.data;
      },
      error => console.log(error)
    );
  }
  IsExpandedDiv(head1: Head1): boolean {
    // if (!this.expandedDivFound && this.expandedId > 0 && this.expandedLevel !== HeadLevel.None && head1) {
    //   if (this.expandedLevel === HeadLevel.Level1) {
    //     this.expandedDivFound = head1.id === this.expandedId;
    //     return this.expandedDivFound;
    //   } else if (this.expandedLevel === HeadLevel.Level2) {
    //     this.expandedDivFound = head1.head2 && head1.head2.length > 0 ? (head1.head2.find(c => c.id === this.expandedId) ? true : false) : false;
    //     return this.expandedDivFound;
    //   } else if (this.expandedLevel === HeadLevel.Level3) {
    //     const head3List = [];
    //     head1.head2.forEach(head2 => {
    //       if (head2.head3 && head2.head3.length > 0) {
    //         head3List.push(...head2.head3);
    //       }
    //     });
    //     this.expandedDivFound = head3List.find(c => c.id === this.expandedId) ? true : false;
    //     return this.expandedDivFound;
    //   }
    // }
    return false;
  }
}
