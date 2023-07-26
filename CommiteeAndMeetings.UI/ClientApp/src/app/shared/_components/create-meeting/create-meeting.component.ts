import { TranslateService } from '@ngx-translate/core';
import { Component, Input, OnInit } from '@angular/core';
import { SharedModalService } from 'src/app/core/_services/modal.service';
@Component({
  selector: 'app-create-meeting',
  templateUrl: './create-meeting.component.html',
  styleUrls: ['./create-meeting.component.scss'],
})
export class CreateMeetingComponent implements OnInit {
  @Input() committeeId;

  constructor(
    private modalService: SharedModalService,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {}
  close() {
    this.modalService.destroyModal();
  }
}
