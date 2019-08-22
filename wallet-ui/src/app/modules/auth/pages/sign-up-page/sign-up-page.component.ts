import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AsyncValidatorFn, FormControl } from '@angular/forms';
import { UsersService, AuthTokenService } from 'src/app/services';
import { Router, ActivatedRoute } from '@angular/router';
import { invokeForm } from 'src/app/modules/shared/operators';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-sign-up-page',
  templateUrl: './sign-up-page.component.html'
})
export class SignUpPageComponent implements OnInit {
  formGroup: FormGroup;

  get assessmentId() {
    return this.activatedRoute.snapshot.queryParams.assessmentId;
  }

  get assessmentInvitationId() {
    return this.activatedRoute.snapshot.queryParams.assessmentInvitationId;
  }

  get returnPath() {
    return this.activatedRoute.snapshot.queryParams.returnPath;
  }

  get email() {
    return this.activatedRoute.snapshot.queryParams.email;
  }

  constructor(
    private users: UsersService,
    private builder: FormBuilder,
    private router: Router,
    private auth: AuthTokenService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.formGroup = this.builder.group({
      email: new FormControl(this.email, {
        updateOn: 'blur',
        validators: [Validators.required, Validators.email],
        asyncValidators: this.isEmailUnique()
      }),
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    const request = this.formGroup.value;
    request.assessmentId = this.assessmentId;
    if (this.email) {
      request.email = this.email;
    }
    this.users
      .signUp(request)
      .pipe(invokeForm(this.formGroup))
      .subscribe(user => {
        this.auth.user = user;
        this.router.navigateByUrl('/');
      });
  }

  private isEmailUnique(): AsyncValidatorFn {
    return control => {
      if (!control.value) {
        return of(null);
      }
      return this.users.isEmailUnique(control.value).pipe(
        map(isUnique => {
          const error = { isUnique: 'The email you have provided is already in use.' };
          return isUnique ? null : error;
        })
      );
    };
  }
}
