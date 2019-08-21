import { Component, OnInit, Input } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html'
})
export class InputComponent {
  @Input()
  name: string;

  @Input()
  label: string;

  @Input()
  description: string;

  @Input()
  labelAsPlaceholder = false;

  @Input()
  set isEnabled(value: boolean) {
    if (value) {
      this.control.enable();
    } else {
      this.control.disable();
    }
  }

  get isEnabled() {
    return this.control.enabled;
  }

  get hasError() {
    return this.control.errors && (this.control.dirty || this.control.touched);
  }

  get errors() {
    const result: any[] = [];
    const control = this.control;
    // tslint:disable-next-line: forin
    for (const errorName in control.errors) {
      const messages = {
        required: 'This field is required.',
        email: 'This is not a valid email address.'
      };
      const message = messages[errorName] ? messages[errorName] : control.errors[errorName];
      result.push({ errorName, message });
    }
    return result;
  }

  get isRequired() {
    if (!this.control.validator) {
      return false;
    }
    const result = this.control.validator(this.control);
    return result && result.required;
  }

  get control() {
    const control = this.directive.form.get(this.name);
    if (!control) {
      throw new Error(`Control not found for ${this.name}.`);
    }
    return control;
  }

  constructor(private directive: FormGroupDirective) {}
}
