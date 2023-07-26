import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { SurveyAnswerUserDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';

@Component({
  selector: 'app-vote-users-list',
  templateUrl: './vote-users-list.component.html',
  styleUrls: ['./vote-users-list.component.scss'],
})
export class VoteUsersListComponent implements OnInit {
  answerUsers: SurveyAnswerUserDTO[];
  currentLanguage: string;
  constructor(private translateService : TranslateService) {}

  ngOnInit(): void {
    this.currentLanguage = this.translateService.currentLang;
  }
}
