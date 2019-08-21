import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html'
})
export class FormComponent {
  @Input()
  formGroup: FormGroup;

  @Input()
  isEnabled = true;

  @Output()
  submitted = new EventEmitter();

  get errorMessage() {
    const error: any = this.formGroup['__error'];
    if (error.status === 400) {
      return error.result;
    } else {
      return error.message;
    }
  }
}
