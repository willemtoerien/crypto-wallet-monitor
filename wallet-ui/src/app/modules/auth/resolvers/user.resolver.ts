import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { tap, catchError } from 'rxjs/operators';
import { EMPTY, throwError } from 'rxjs';
import { User, UsersService, AuthTokenService } from 'src/app/services';

@Injectable({
  providedIn: 'root'
})
export class UserResolver implements Resolve<User> {
  constructor(private users: UsersService, private authToken: AuthTokenService) {}

  resolve() {
    if (!this.authToken.isAuthenticated.value) {
      return undefined;
    }
    if (this.authToken.user) {
      return this.authToken.user;
    }
    return this.users.get().pipe(
      catchError(error => {
        if (error.status === 401 || error.status === 400) {
          this.authToken.token = undefined;
          return EMPTY;
        }
        return throwError(error);
      }),
      tap(user => (this.authToken.user = user))
    );
  }
}
