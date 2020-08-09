import { Component, OnInit } from '@angular/core';
import { SidebarItems } from '../../../../shared/model/sidebar-item.model';
import { AuthService } from '../../../../shared/services/auth.service';
import { TokenService } from '../../../../shared/services/token.service';
declare const $: any;
@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  menuItems: SidebarItems[];
  constructor(private authService: AuthService, private tokenService: TokenService) { }

  ngOnInit() {
    const userRoles: string[] = this.tokenService.userInfo.roles.split(',');
    const administrator = [
      { path: '/admin/gatepass', title: 'Gate Pass', icon: 'fas fa-passport', class: '' },
      { path: '/admin/purchase', title: 'Purchase', icon: 'fa fa-shopping-cart', class: '' },
      // { path: '/admin/sale', title: 'Sale', icon: 'fa fa-shopping-cart', class: '' },
      { path: '/admin/party', title: 'Party', icon: 'fa fa-building', class: '' },
      { path: '/admin/vehicle', title: 'Vehicle', icon: 'fa fa-truck', class: '' },
      { path: '/admin/product', title: 'Product', icon: 'fa fa-shopping-bag', class: '' },
      { path: '/admin/ledger/party', title: 'Party Ledger', icon: 'fa fa-book', class: '' },
      { path: '/admin/ledger/company', title: 'Company Ledger', icon: 'fa fa-book', class: '' },
      // { path: '/admin/bank-transaction', title: 'Bank Transaction', icon: 'fa fa-exchange-alt', class: '' },
      // { path: '/admin/bank-account', title: 'Bank Account', icon: 'fa fa-university', class: '' }
    ];
    const userItems = [
      { path: '/admin/settings', title: 'Settings', icon: 'fa fa-cog', class: '' },
    ];
    const gateKeeper = [
      { path: '/admin/gatepass', title: 'Gate Pass', icon: 'fas fa-passport', class: '' },
    ];
    this.menuItems = [];
    if (userRoles.includes('Administrator')) {
      this.menuItems.push(...administrator, ...userItems);
    } else if (userRoles.includes('GateKeeper')) {
      this.menuItems.push(...gateKeeper, ...userItems);
    }
  }

  isMobileMenu() {
    if (window.window.innerWidth > 991) {
      return false;
    }
    return true;
  }
  logout() {
    this.authService.logout();
  }

}
