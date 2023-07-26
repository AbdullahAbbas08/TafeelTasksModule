import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ValidateFilesService {

  constructor() {
  }

  upload(files: any) {
    const result = [];
    const handleFileTYpe = (view, resolve, reject, arrayLength) => {
      // If File Size Equal Zero
      if (!view.byteLength) {
        return result.push(false);
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
        case 'FFD8FFE1': // image/jpeg
        case 'FFD8FFE2': // image/jpeg
        case 'FFD8FFE3': // image/jpeg
        case '89504E47': // image/png
        case '47494638': // image/gif
        case '49492A00': // image/tiff
        case '25504446': // application/pdf
        case '504B0304': // new office xlsx/docx/pptx
        case 'D0CF11E0': // old office xls/doc/ppt
        case 'FFD8FFDB': 
        case '51607': 
          result.push(true);
          break;
        default:
          result.push(false);
          break;
      }
      if (arrayLength === result.length) {
        if (result.some(x => x !== true)) {
          reject(result);
        }
        resolve(result);
      }
    };

    const handleFileType = (file, resolve, reject, arrayLength) => {
      const fileReader = new FileReader();
      fileReader.onload = (e) => {
        const af = fileReader.result
          , view = new DataView(<ArrayBufferLike>af);

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
  }
}
