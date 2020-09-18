import { Component, TemplateRef } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';

@Component({
  selector: 'app-template-renderer',
  template: `
    <div *ngIf="condition">
      <ng-container
        *ngTemplateOutlet="template; context: templateContext"
      ></ng-container>
    </div>
  `
})
export class TemplateRendererComponent implements ICellRendererAngularComp {
  condition = false;
  template: TemplateRef<any>;
  templateContext: { $implicit: any, params: any };

refresh(params: any): boolean {
  if(params.value == "footer"){
    this.condition = false;
    return
  }
  this.templateContext = {
    $implicit: params.data,
    params: params
  };
  this.condition = true;
  return true;
}

  agInit(params: ICellRendererParams): void {
    this.template = params['ngTemplate'];
    this.refresh(params);
  }
}
