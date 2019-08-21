import { pipe, throwError, OperatorFunction, MonoTypeOperatorFunction, UnaryFunction } from 'rxjs';
import { tap, finalize, catchError } from 'rxjs/operators';
import { pipeFromArray } from 'rxjs/internal/util/pipe';
import { FormGroup } from '@angular/forms';
import { validateForm } from './validate-form';

export function invokeForm<T>(formGroup: FormGroup): OperatorFunction<T, T> {
  formGroup['__isBusy'] = true;
  formGroup['__error'] = undefined;
  return pipeFromArray([
    finalize(() => (formGroup['__isBusy'] = false)),
    validateForm(formGroup),
    catchError(error => {
      formGroup['__error'] = error;
      return throwError(error);
    })
  ]);
}
