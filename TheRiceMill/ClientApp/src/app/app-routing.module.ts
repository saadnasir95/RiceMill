import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthGuard } from './shared/services/auth-guard.service';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', loadChildren: './features/login/login.module#LoginModule' },
  { path: 'admin', loadChildren: './features/layout/layout.module#LayoutModule' },
  { path: '**', redirectTo: '/login' },
];

const config: ExtraOptions = {
  useHash: false,
};


@NgModule({
  imports: [RouterModule.forRoot(routes, config)],
  exports: [RouterModule],
  // providers: [AuthService, AuthGuard]
})
export class AppRoutingModule {
}
