import { Component, OnInit } from '@angular/core';
import { filter } from 'rxjs/operators';
import { CommitteeService } from '../committee.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  constructor(public committeeService: CommitteeService) {}

  ngOnInit(): void {}

  item: string;
  value: boolean;
  currentStats:Number;
  searchThis(data) {
    this.item = data;
  }
  resetValue(flag) {
    this.value = flag;
  }
  committecurrentStats(filterId){
   this.currentStats = filterId 
  }
}
