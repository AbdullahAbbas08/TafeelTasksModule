import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-descisions',
  templateUrl: './descisions.component.html',
  styleUrls: ['./descisions.component.scss']
})
export class DescisionsComponent implements OnInit {
  transactionGenericType: string = 'Decisions';
  constructor() { }

  ngOnInit(): void {
  }
  onGenericTransactionTypeChange(event) {
    if (event) {
      this.transactionGenericType = 'Generalization';
    } else {
      this.transactionGenericType = 'Decisions';
    }
  }
}
