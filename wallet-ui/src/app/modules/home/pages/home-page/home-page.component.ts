import { Component, OnInit } from '@angular/core';
import { TransactionsService, AuthTokenService } from 'src/app/services';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { invokeForm } from 'src/app/modules/shared/operators';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {
  formGroup: FormGroup;

  constructor(private builder: FormBuilder, private transactions: TransactionsService, private auth: AuthTokenService) {}

  ngOnInit() {
    this.formGroup = this.builder.group({
      email: new FormControl('', [Validators.required, Validators.email]),
      purpose: [''],
      amount: [0]
    });
  }

  onSubmit() {
    this.transactions
      .pay(this.formGroup.value)
      .pipe(invokeForm(this.formGroup))
      .subscribe(() => (this.auth.user.balance -= parseFloat(this.formGroup.value.amount)));
  }
}
