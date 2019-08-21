import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignUpRequest, SignInRequest, User } from './models';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private http: HttpClient) {}

  signUp(request: SignUpRequest) {
    return this.http.post<User>(`/users/sign-up`, request);
  }

  signIn(request: SignInRequest) {
    return this.http.post<User>(`/users/sign-in`, request);
  }

  get() {
    return this.http.get<User>(`/users`);
  }

  isEmailUnique(email: string) {
    return this.http.get<boolean>(`/users/email/unique?email=${encodeURIComponent(email)}`);
  }
}
