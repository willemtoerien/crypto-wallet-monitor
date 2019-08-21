import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckBoxComponent } from './components/check-box/check-box.component';
import { FormComponent } from './components/form/form.component';
import { InputComponent } from './components/input/input.component';
import { SubmitButtonComponent } from './components/submit-button/submit-button.component';
import { TextBoxComponent } from './components/text-box/text-box.component';

@NgModule({
  declarations: [CheckBoxComponent, FormComponent, InputComponent, SubmitButtonComponent, TextBoxComponent],
  imports: [CommonModule],
  exports: [CheckBoxComponent, FormComponent, InputComponent, SubmitButtonComponent, TextBoxComponent]
})
export class SharedModule {}
