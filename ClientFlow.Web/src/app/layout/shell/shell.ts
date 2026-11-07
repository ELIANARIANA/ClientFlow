import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Auth } from '../../core/auth';

@Component({
  imports: [RouterLink, RouterOutlet, RouterLinkActive],
  selector: 'app-shell',
  styleUrl: './shell.scss',
  templateUrl: './shell.html',
})
export class Shell {
  private readonly auth = inject(Auth);
  private readonly router = inject(Router);

  readonly navItems = [
    { label: "Dashboard", path: '/dashboard', icon: '📊'},
  ];

  logout(): void {
    this.auth.logout();
    this.router.navigateByUrl('/login');
  }
}
