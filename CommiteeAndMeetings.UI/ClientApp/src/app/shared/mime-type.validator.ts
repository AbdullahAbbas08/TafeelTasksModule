import { AbstractControl } from '@angular/forms';
import { Observable, Observer, of } from 'rxjs';

export const mimeType = (
  control: AbstractControl
): Promise<{ [key: string]: any }> | Observable<{ [key: string]: any }> => {
  if (typeof control.value === 'string') {
    return of(null);
  }
  const files = control.value as File[];
  const fileReader = new FileReader();

  const frObs = new Observable((observer: Observer<{ [key: string]: any }>) => {
    fileReader.addEventListener('loadend', () => {
      const arr = new Uint8Array(fileReader.result as ArrayBuffer).subarray(
        0,
        4
      );
      let header = '';
      let isValid = false;

      files.forEach((file) => {
        for (let i = 0; i < arr.length; i++) {
          header += arr[i].toString(16).toUpperCase();
        }

        switch (header) {
          case 'FFD8FFE8': // image/jpeg
          case 'FFD8FFE0': // image/jpeg
          case 'FFD8FFDB': // image/jpg
          case 'FFD8FFEE': // image/jpg
          case 'FFD8FFE1': // image/jpeg
          case 'FFD8FFE2': // image/jpeg
          case 'FFD8FFE3': // image/jpeg
          case '89504E47': // image/png
          case '47494638': // image/gif
          case '49492A00': // image/tiff
          case '25504446': // application/pdf
          case '504B0304': // new office xlsx/docx/pptx
          case '504B0506': // new office xlsx/docx/pptx
          case '504B0708': // new office xlsx/docx/pptx
          case 'DBA52D00': // new office xlsx/docx/pptx
          case '14000600': // new office xlsx/docx/pptx
          case 'ECA5C100': // old office xlsx/docx/pptx
          case '0D444F43': // old office xlsx/docx/pptx
          case 'D0CF11E0': // old office xls/doc/ppt
          case 'CF11E0A1': // old office xls/doc/ppt
          case 'A1B11AE1': // old office xls/doc/ppt
          case '0E11FC0D': // old office xls/doc/ppt
          case 'D0CF110E': // old office xls/doc/ppt
          case 'A202020': // old office xls/doc/ppt (2003)
            isValid = true;
            break;
          default:
            isValid = false; // Or you can use the blob.type as fallback
            break;
        }

        fileReader.readAsArrayBuffer(file);
      });

      if (isValid) {
        observer.next(null);
      } else {
        observer.next({ invalidMimeType: true });
      }

      observer.complete();
    });
  });
  return frObs;
};
