import { Component, OnInit, AfterViewInit, Input, ViewChild, ElementRef } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-text-box',
  templateUrl: './text-box.component.html'
})
export class TextBoxComponent implements AfterViewInit {
  @Input()
  type = 'text';

  @Input()
  name: string;

  @Input()
  label: string;

  @Input()
  description: string;

  @Input()
  autoComplete = 'off';

  @Input()
  isEnabled = true;

  @Input()
  labelAsPlaceholder: boolean;

  @ViewChild('input', { static: true })
  inputElement: ElementRef<HTMLInputElement>;

  get control() {
    return this.directive.form.get(this.name);
  }

  constructor(private directive: FormGroupDirective) {}

  ngAfterViewInit() {
    if (this.autoComplete) {
      this.inputElement.nativeElement.autocomplete = this.autoComplete;
    }
    if (this.labelAsPlaceholder) {
      this.inputElement.nativeElement.placeholder = this.label;
    }
  }
}
