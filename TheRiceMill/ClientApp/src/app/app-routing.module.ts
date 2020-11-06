import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' },
];

const config: ExtraOptions = {
  useHash: false
};


@NgModule({
  imports: [RouterModule.forRoot(routes, config)],
  exports: [RouterModule],
  // providers: [AuthService, AuthGuard]
})
export class AppRoutingModule {
}
