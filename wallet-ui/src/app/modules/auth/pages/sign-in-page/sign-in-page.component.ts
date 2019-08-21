import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { validateForm, invokeForm } from 'src/app/modules/shared/operators';
import { UsersService, AuthTokenService } from 'src/app/services';

@Component({
  selector: 'app-sign-in-page',
  templateUrl: './sign-in-page.component.html'
})
export class SignInPageComponent implements OnInit {
  formGroup: FormGroup;

  constructor(private users: UsersService, private builder: FormBuilder, private router: Router, private auth: AuthTokenService) {}

  ngOnInit() {
    this.formGroup = this.builder.group({
      email: new FormControl('', {
        validators: [Validators.required, Validators.email]
      }),
      password: ['', Validators.required]
    });
  }

  async onSubmit() {
    this.users
      .signIn(this.formGroup.value)
      .pipe(invokeForm(this.formGroup))
      .subscribe(user => {
        this.auth.user = user;
        this.router.navigateByUrl('/');
      });
  }
}
