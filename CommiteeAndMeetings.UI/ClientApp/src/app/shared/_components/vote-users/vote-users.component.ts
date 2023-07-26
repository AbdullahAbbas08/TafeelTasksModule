import { TranslateService } from '@ngx-translate/core';
import { Component, Input, OnInit } from '@angular/core';
import { SurveyAnswerUserDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';

@Component({
  selector: 'app-vote-users',
  templateUrl: './vote-users.component.html',
  styleUrls: ['./vote-users.component.scss'],
})
export class VoteUsersComponent implements OnInit {
  @Input() answerId;
  @Input() surveyAnswerUsers: SurveyAnswerUserDTO[];
  @Input() userCount;
  take = 10;
  skip = 0;
  count;

  constructor(
    private modalService: SharedModalService,
    public translateService: TranslateService
  ) {}

  ngOnInit(): void {

  }

  showAnswerUsers() {
    this.translateService.get('Voters').subscribe((text) => {
      this.modalService.initAnswerUsermodal(this.surveyAnswerUsers, text);
    });
  }
}
