import { TranslateService } from '@ngx-translate/core';
import { MOMService } from './../mom.service';
import {
  Component,
  Input,
  OnInit,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import {
  CommentDTO,
  CommentType,
  MeetingCommentDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { BackgroundTrigger } from '../add-edit-recommendation.animation';

@Component({
  selector: 'app-add-recommendation',
  templateUrl: './add-recommendation.component.html',
  styleUrls: ['./add-recommendation.component.scss'],
  animations: [BackgroundTrigger],
})
export class AddRecommendationComponent implements OnInit, OnChanges {
  @Input() meetingId: number;
  @Input() meetingClosed;
  @Input() recommendation: MeetingCommentDTO = new MeetingCommentDTO();
  @Output() addRecommendation: EventEmitter<MeetingCommentDTO> =
    new EventEmitter();
  recommendationText: string;
  editMode = 'add';
  constructor(
    private momService: MOMService,
    private storeService: StoreService,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {}

  ngOnChanges(changes: SimpleChanges): void {
    // If in Edit Mode
    if (
      !changes['recommendation']?.firstChange &&
      changes['recommendation']?.currentValue?.id != null
    ) {
      this.recommendation = Object.assign(
        this.recommendation,
        changes['recommendation']?.currentValue
      );
      this.recommendationText = this.recommendation?.comment?.text;
      this.editMode = 'edit';
    }
  }

  onSubmitRecommendation() {
    if (this.recommendation?.id) {
      this.recommendation.comment.text = this.recommendationText;
      this.momService
        .updateMeetingRecommendation(this.recommendation)
        .subscribe((res) => {
          if(res){
            this.storeService.refreshRecommendation$.next(res);
          }
        }); 
    } else {
      let comment = new CommentDTO({ text: this.recommendationText });
      this.momService
        .addMeetingRecommendation(
          new MeetingCommentDTO({
            meetingId: this.meetingId,
            comment,
            commentType: CommentType._2,
          })
        )
        .subscribe((res) => {
          if (res) {
            this.addRecommendation.emit(res);
            this.storeService.refreshRecommendation$.next(res);
          }
        });
    }
    this.recommendationText = '';
    this.recommendation = new MeetingCommentDTO();
    this.editMode = 'add';
  }

  clearBox() {
    this.recommendationText = '';
    this.recommendation = new MeetingCommentDTO();
    this.editMode = 'add';
  }
}
