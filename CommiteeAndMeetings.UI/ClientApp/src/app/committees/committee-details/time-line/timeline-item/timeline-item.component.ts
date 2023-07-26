import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { CommitteeDataTypes } from 'src/app/shared/_enums/AppEnums';

@Component({
  selector: 'app-timeline-item',
  templateUrl: './timeline-item.component.html',
  styleUrls: ['./timeline-item.component.scss'],
})
export class TimelineItemComponent implements OnInit {
  @Input('item') item;
  types = CommitteeDataTypes;
  itemComment: FormControl = new FormControl();

  constructor() {}

  ngOnInit(): void {}

  sendData() {
    if (this.item.type === this.types.Vote) {
    }
  }
}
