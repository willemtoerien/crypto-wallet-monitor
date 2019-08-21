import { Component, OnInit, Input } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-submit-button',
  templateUrl: './submit-button.component.html'
})
export class SubmitButtonComponent {
  @Input()
  context = 'primary';

  @Input()
  isEnabled = true;

  @Input()
  isBlock = false;

  @Input()
  offset = true;

  get formGroup() {
    return this.directive.form;
  }

  constructor(public directive: FormGroupDirective) {}
}
