import { FormGroup } from '@angular/forms';
import { catchError } from 'rxjs/operators';
import { throwError, EMPTY, ObservableInput } from 'rxjs';
import { ModelStateDictionary } from 'src/app/services';

export function validateForm<T, O extends ObservableInput<T>>(formGroup: FormGroup) {
  return catchError<T, O>(error => {
    if (error.status !== 400 && error.status !== 404) {
      return throwError(error) as any;
    }

    if (error.result) {
      let modelState: ModelStateDictionary;

      if (error.result.errors) {
        modelState = error.result.errors;
      } else if (typeof error.result !== 'string') {
        modelState = error.result;
      }

      if (modelState) {
        // tslint:disable-next-line: forin
        for (const entryKey in modelState) {
          if (entryKey === '') {
            return throwError(new Error(modelState[entryKey]));
          }
          const entry = modelState[entryKey];
          const key = entryKey[0].toLowerCase() + entryKey.substring(1);
          const control = formGroup.get(key);
          if (!control) {
            console.error(`Validation failed:` + JSON.stringify(entry[0]));
            continue;
          }
          control.setErrors({
            server: entry[0]
          });
          control.markAsDirty();
        }
        return EMPTY;
      }
    }
    return throwError(error);
  });
}
