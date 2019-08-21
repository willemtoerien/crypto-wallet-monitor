import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, CanLoad, Router } from '@angular/router';
import { AuthTokenService } from 'src/app/services';

@Injectable({
  providedIn: 'root'
})
export class NonAuthGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(private authToken: AuthTokenService, private router: Router) {}

  canActivate() {
    return this.isNotAuthenticated();
  }

  canActivateChild() {
    return this.isNotAuthenticated();
  }

  canLoad() {
    return this.isNotAuthenticated();
  }

  private isNotAuthenticated() {
    if (this.authToken.isAuthenticated.value) {
      this.router.navigateByUrl('/');
    }
    return !this.authToken.isAuthenticated.value;
  }
}
