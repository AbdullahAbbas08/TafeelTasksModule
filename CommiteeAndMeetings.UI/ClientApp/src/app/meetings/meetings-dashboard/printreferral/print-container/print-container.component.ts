import { Component, OnInit,Inject, Input,OnDestroy} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { API_BASE_URL, CommentDTO, MeetingSummaryDTO, MeetingTopicDTO, MOMCommentDTO, MOMSummaryDTO} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { takeUntil } from 'rxjs/operators';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-print-container',
  templateUrl: './print-container.component.html',
  styleUrls: ['./print-container.component.scss']
})
export class PrintContainerComponent implements OnInit, OnDestroy {
  @Input('summary') summary:MeetingSummaryDTO;
  @Input('allmeetingTopics')allmeetingTopics;
  private ngUnsubscribe = new Subject();
  Subscription: Subscription;
  meetingTopics:any[] = []
  constructor(@Inject(API_BASE_URL) public baseUrl: string,public translateService: TranslateService,private storeService: StoreService) { }

  ngOnInit(): void {
  this.Subscription =  this.storeService.refreshRecommendation$.pipe(takeUntil(this.ngUnsubscribe)).subscribe((val) => {
     if(val) {
        this.summary.momComment?.push(
          new MOMCommentDTO({
            comment: new CommentDTO({
              text:val.comment.text
            })
          })
        )
 
     }
    })
    this.storeService.refreshMinutesOfMeeting$.subscribe((val) => {
      if(val) {
        this.summary?.momSummaries?.push( 
          new MOMSummaryDTO({
            title:val.title,
            description:val.description
          })
        )
      }
    })
  }
  ngOnDestroy() {
    this.storeService.refreshRecommendation$.next(undefined)
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
    this.Subscription.unsubscribe()
    }
}
