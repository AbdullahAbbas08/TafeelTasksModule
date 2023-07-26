import { AbstractControl } from '@angular/forms';
import { Observable, of } from 'rxjs';

export const mimeTypeValidator = (
  control: AbstractControl
): Promise<{ [key: string]: any }> | Observable<{ [key: string]: any }> => {

  const files = control.value as File[];
  if (control.value === null) {
    return of(null);
  }
  const result = [];

  const handleFileTYpe = (view, resolve, reject, arrayLength) => {
    // If File Size Equal Zero
    if (!view.byteLength) {
      reject(new Error('Sorry, This file is empty.'));
      return;
    }
    const first4Bytes = view.getUint32(0, false);
    const first4BytesHex = Number(first4Bytes).toString(16).toUpperCase();

    let count = 0;
    while (count < 4) {
      const int8 = view.getUint8(count, false);
      count++;
    }

    switch (first4BytesHex) {
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
        result.push(true);
        break;
      default:
        result.push(false);
        break;
    }
    if (arrayLength === result.length) {
      if (result.some((x) => x !== true)) {
        resolve({ inValidMimeType: true });
      } else {
        resolve(null);
      }
    }
  };

  const handleFileType = (file, resolve, reject, arrayLength) => {
    const fileReader = new FileReader();
    fileReader.onload = (e) => {
      const af = fileReader.result,
        view = new DataView(<ArrayBufferLike>af);

      handleFileTYpe(view, resolve, reject, arrayLength);
    };
    fileReader.readAsArrayBuffer(file);
  };

  const processArray = (array, resolve, reject) => {
    for (const item of array) {
      handleFileType(item, resolve, reject, array.length);
    }
  };
  return new Promise((resolve, reject) => {
    processArray(files, resolve, reject);
  });
};
