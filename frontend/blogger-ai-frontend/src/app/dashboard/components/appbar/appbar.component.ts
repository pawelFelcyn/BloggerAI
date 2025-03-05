import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'dashboard-appbar',
  templateUrl: './appbar.component.html',
  styleUrl: './appbar.component.scss',
})
export class AppbarComponent {
  constructor(private router: Router) {}

  logout() {
    sessionStorage.removeItem('jwt');
    this.router.navigate(['/login']);
  }
}
