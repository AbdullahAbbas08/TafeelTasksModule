import { Injectable, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class DestroyService implements OnDestroy {
  subDestroyed: Subject<boolean>;
  constructor() {
    this.subDestroyed = new Subject<boolean>();
  }

  ngOnDestroy() {
    this.subDestroyed.next(null);
    this.subDestroyed.complete();
  }
}
