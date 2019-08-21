import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { User } from './models';

@Injectable({
  providedIn: 'root'
})
export class AuthTokenService {
  user: User;
  isAuthenticated = new BehaviorSubject<boolean>(false);

  get token() {
    const token = localStorage.getItem('auth-token');
    this.update(token);
    return token;
  }

  set token(val: string) {
    if (!val) {
      localStorage.removeItem('auth-token');
      return;
    }
    localStorage.setItem('auth-token', val);
    this.update(val);
  }

  constructor() {
    this.update(this.token);
  }

  private update(token: string) {
    if (token && !this.isAuthenticated.value) {
      this.isAuthenticated.next(true);
    }
    if (!token && this.isAuthenticated.value) {
      this.isAuthenticated.next(false);
    }
  }
}
