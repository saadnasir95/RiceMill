import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HeadComponent } from './components/head/head.component';

const routes: Routes = [
  { path: '', component: HeadComponent }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HeadRoutingModule { }
