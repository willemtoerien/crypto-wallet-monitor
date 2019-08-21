import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, CanLoad, Router } from '@angular/router';
import { AuthTokenService } from 'src/app/services';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(private authToken: AuthTokenService, private router: Router) {}

  canActivate() {
    return this.isAuthenticated();
  }

  canActivateChild() {
    return this.isAuthenticated();
  }

  canLoad() {
    return this.isAuthenticated();
  }

  private isAuthenticated() {
    if (!this.authToken.isAuthenticated.value) {
      this.router.navigateByUrl('/auth/sign-in');
    }

    return this.authToken.isAuthenticated.value;
  }
}
