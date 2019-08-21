import { Component, OnInit, Input } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';

export type CheckboxType = 'checkbox' | 'radio' | 'switch';

@Component({
  selector: 'app-check-box',
  templateUrl: './check-box.component.html'
})
export class CheckBoxComponent {
  @Input()
  name: string;

  @Input()
  label: string;

  @Input()
  description: string;

  @Input()
  isEnabled = true;

  @Input()
  type: CheckboxType = 'checkbox';

  get inputType() {
    return this.type === 'radio' ? 'radio' : 'checkbox';
  }

  get control() {
    return this.directive.form.get(this.name);
  }

  constructor(private directive: FormGroupDirective) {}
}
