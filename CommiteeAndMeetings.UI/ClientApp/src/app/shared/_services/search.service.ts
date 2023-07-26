import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  searchcriteria = new Subject<string>();

  constructor() {}

  search(searchword: string) {
    this.searchcriteria.next(searchword);
  }
}
