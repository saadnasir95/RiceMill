import { Component, OnInit, Input, TemplateRef } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {
  @Input() Header: TemplateRef<any>;
  @Input() Body: TemplateRef<any>;
  constructor() { }

  ngOnInit() {
  }

}
