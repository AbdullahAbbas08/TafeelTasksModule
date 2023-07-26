import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';
@Injectable({
  providedIn: 'root',
})
export class BrowserStorageService {
  private encryptKey = CryptoJS.enc.Utf8.parse('4512631236589784');

  getSession(key: string): any {
    const data = window.sessionStorage.getItem(key);
    if (data) {
      return JSON.parse(data);
    } else {
      return null;
    }
  }

  setSession(key: string, value: any): void {
    const data = value === undefined ? '' : JSON.stringify(value);
    window.sessionStorage.setItem(key, data);
  }

  removeSession(key: string): void {
    window.sessionStorage.removeItem(key);
  }

  removeAllSessions(): void {
    for (const key in window.sessionStorage) {
      if (window.sessionStorage.hasOwnProperty(key)) {
        this.removeSession(key);
      }
    }
  }

  getLocal(key: string): any {
    const data = window.localStorage.getItem(key);
    if (data) {
      return JSON.parse(data);
    } else {
      return null;
    }
  }

  setLocal(key: string, value: any): void {
    const data = value === undefined ? '' : JSON.stringify(value);
    window.localStorage.setItem(key, data);
  }

  removeLocal(key: string): void {
    window.localStorage.removeItem(key);
  }

  removeAllLocals(): void {
    for (const key in window.localStorage) {
      if (window.localStorage.hasOwnProperty(key)) {
        this.removeLocal(key);
      }
    }
  }

  decryptUser(value = null): any {
    if (!value) {
      value = localStorage.getItem('user')
        ? JSON.parse(localStorage.getItem('user'))
        : null;
    }

    if (!value) {
      return;
    }

    var user = {};

    for (let key in value) {
      var prop = value[key];

      if (prop instanceof Array) {
        // array
        var newArray = [];

        prop.forEach((res, index) => {
          if (res instanceof Object) {
            var obj = {};
            for (let i in res) {
              let item = res[i];
              if (item === '' || !item) {
              } else {
                obj[i] = this.decrypteString(item);
              }
            }
            newArray.push(obj);
            // user[key] = newArray;
          } else if (prop === '' || !prop) {
          } else {
            newArray.push(this.decrypteString(res));
          }
        });
        if (newArray.length) {
          user[key] = newArray;
        }
      } else if (prop === '' || !prop) {
      } else {
        user[key] = this.decrypteString(prop);
      }
    }

    return user;
  }

  encryptUser(value = null): any {
    if (!value) return;
    var user = {};

    for (let key in value) {
      var prop = value[key];

      if (prop instanceof Array) {
        // array
        var newArray = [];

        prop.forEach((res, index) => {
          if (res instanceof Object) {
            var obj = {};
            for (let i in res) {
              let item = res[i];
              if (item === '' || !item) {
              } else {
                obj[i] = this.encrypteString(item);
              }
            }
            newArray.push(obj);
            // user[key] = newArray;
          } else if (prop === '' || !prop) {
          } else {
            newArray.push(this.encrypteString(res));
          }
        });
        if (newArray.length) {
          user[key] = newArray;
        }
      } else if (prop === '' || !prop) {
      } else {
        user[key] = this.encrypteString(prop);
      }
    }

    return user;
  }

  decrypteString(string) {
    if (!string) return;
    var decrypted = CryptoJS.AES.decrypt(string, this.encryptKey, {
      keySize: 128 / 8,
      iv: this.encryptKey,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7,
    });

    let decrptyString = decrypted.toString(CryptoJS.enc.Utf8);

    if (decrptyString.toLocaleLowerCase() === 'true') {
      decrptyString = true;
    } else if (decrptyString.toLocaleLowerCase() === 'false') {
      decrptyString = false;
    }

    return decrptyString;
  }

  encrypteString(string = null) {
    if (!string) return;
    var encrypted = CryptoJS.AES.encrypt(
      CryptoJS.enc.Utf8.parse(string),
      this.encryptKey,
      {
        keySize: 128 / 8,
        iv: this.encryptKey,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7,
      }
    );

    return encrypted.toString();
  }
  encryptCommitteId(id:string){
    const committeId = this.decrypteString(id),
     userRoleId = this.getUserRoleId(),
     encrypytCommitteId = this.encrypteString(`${committeId}_${userRoleId}`);
    return encrypytCommitteId
  }
  getUserRoleId(){
    return this.decrypteString(
      JSON.parse(localStorage.getItem("user"))["userRoleId"]
    );
  }
}
