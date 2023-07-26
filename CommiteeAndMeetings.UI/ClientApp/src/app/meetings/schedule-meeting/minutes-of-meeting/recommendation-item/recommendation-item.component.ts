import { SingleMeetingService } from './../../single-meeting.service';
import { MOMService } from './../mom.service';
import { MeetingCommentDTO, MeetingCoordinatorDTO, SurveyAnswerUserDTO } from './../../../../core/_services/swagger/SwaggerClient.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { VotingService } from 'src/app/committees/committee-details/votes/voting.service';
import { AuthService } from 'src/app/auth/auth.service';
import { ThemeService } from 'src/app/shared/_services/theme.service';

@Component({
  selector: 'app-recommendation-item',
  templateUrl: './recommendation-item.component.html',
  styleUrls: ['./recommendation-item.component.scss'],
})
export class RecommendationItemComponent implements OnInit {
  @Input() recommendation: MeetingCommentDTO;
  @Input() meetingClosed;
  @Output() editRecommendation: EventEmitter<MeetingCommentDTO> =
    new EventEmitter();
  voteAnswer: string[] = [];
  @Output() deleteRecommendation: EventEmitter<MeetingCommentDTO> =
    new EventEmitter();
  @Input() isCoordinator;
  @Input() isCreator;
  // @Input() isMeetingClosed;
  @Input() byPassStartForActivities;
  userId: number;
  createdUserName: string;
  currentLang: string;
  createdUserImage: string;
  userParticipated = false;
  meetingCoordinateAttende:MeetingCoordinatorDTO[]=[];
  notAttendees:boolean = false
  constructor(private authService: AuthService,private votingService: VotingService,public singleMeeting: SingleMeetingService) {}

  ngOnInit(): void {
    this.userId = this.authService.getUser().userId;
    this.checkIfUserVoted();
    this.setCreatedByUserDetails();
    this.checkIsCoordinatorAttendee()
  }

  onEditRecommendation() {
    this.editRecommendation.emit(this.recommendation);
  }

  onDeleteRecommendation() {
    this.deleteRecommendation.emit(this.recommendation);
  }
  sendVote() {
    if (this.voteAnswer.length < 1) return;
    let surveyAnswer: SurveyAnswerUserDTO[] = this.voteAnswer.map(
      (answerId) => {
        return new SurveyAnswerUserDTO({
          userId: this.userId,
          surveyAnswerId: +answerId,
          meetingId:this.recommendation.meetingId
        });
      }
    );

    this.votingService.sendVoteAnswer(surveyAnswer).subscribe((res) => {
      if (res) {
        res.forEach((item) => {
          this.recommendation.surveyAnswers
            .filter((ans) => ans.surveyAnswerId === item.surveyAnswerId)
            .forEach((answer) => answer.surveyAnswerUsers.unshift(item));
        });
        this.userParticipated = true;
      }
    });
  }
  setCreatedByUserDetails() {
    if (this.recommendation?.createdByUser) {
      this.createdUserName =
        this.currentLang === 'ar'
          ? this.recommendation?.createdByUser.fullNameAr
          : this.recommendation?.createdByUser.fullNameEn;
      this.createdUserImage = this.recommendation?.createdByUser.profileImage;
    }
  }
  checkIfUserVoted() {
    this.recommendation?.surveyAnswers.forEach((answer) => {
      answer.surveyAnswerUsers.forEach((user) => {
        if (user.user.userId == this.userId) {
          this.userParticipated = true;
        }
      });
    });
  }
  checkIsCoordinatorAttendee(){
    this.meetingCoordinateAttende = this.singleMeeting.meeting.meetingCoordinators.filter((coor) => {
      return coor.coordinator.userId == +this.userId
    });
    if(this.meetingCoordinateAttende){
      // && !this.meetingCoordinateAttende[0]?.isCreator
      if(this.meetingCoordinateAttende[0]?.confirmeAttendance=== false){
        this.isCoordinator = true;
        this.notAttendees = true;
       // this.meetingClosed = true
      }
    }
  }
}
