import { Component, OnInit,Input,OnDestroy } from '@angular/core';
import { CommiteeDTO, CountResultDTO, UserTaskCountDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { StoreService } from 'src/app/shared/_services/store.service';
import { ExtentedCommiteeMemberDTO, UserService } from 'src/app/committees/committee-details/users/user.service';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-committes-minutes',
  templateUrl: './committes-minutes.component.html',
  styleUrls: ['./committes-minutes.component.scss']
})
export class CommittesMinutesComponent implements OnInit, OnDestroy {
  @Input() committeDetails:CommiteeDTO;
  @Input() AllUsers:ExtentedCommiteeMemberDTO[];
  @Input() commiteeMinutsCount:CountResultDTO[];
  subscription: Subscription;
  constructor(public translateService: TranslateService,private storeService:StoreService,private UserService:UserService) { }

  ngOnInit(): void {}
  ngOnDestroy() {
  }
}
