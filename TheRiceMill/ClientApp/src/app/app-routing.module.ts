import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthGuard } from './shared/services/auth-guard.service';
import { LayoutComponent } from './features/layout/components/layout/layout.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' },
];

const config: ExtraOptions = {
  useHash: false,
  enableTracing: true
};


@NgModule({
  imports: [RouterModule.forRoot(routes, config)],
  exports: [RouterModule],
  // providers: [AuthService, AuthGuard]
})
export class AppRoutingModule {
}
