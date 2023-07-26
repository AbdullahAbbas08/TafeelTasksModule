import { LocalizationService } from './../../../shared/_services/localization.service';
import { StoreService } from './../../../shared/_services/store.service';
import { Subscription } from 'rxjs';
import { AuthService } from './../../../auth/auth.service';
import { SignalRService } from 'src/app/shared/_services/signal-r.service';
import { LayoutService } from './../../../shared/_services/layout.service';
import {
  GerogorianMonthsAr,
  GerogorianMonthsEn,
} from './../../../shared/_enums/AppEnums';
import {
  AttendeeDTO,
  MeetingTopicDTO,
  MeetingUserAttendationDTO,
  SurveyAnswerUserDTO,
  SurveyDTO,
  TopicCommentDTO,
  TopicState,
} from './../../../core/_services/swagger/SwaggerClient.service';
import { SingleMeetingService } from './../single-meeting.service';
import { AgendaService } from './agenda.service';
import {
  Component,
  NgZone,
  OnDestroy,
  OnInit,
  QueryList,
  Input,
  ViewChildren,
  Output,
  EventEmitter
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TopicType } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { ActivatedRoute } from '@angular/router';
import { TopicTimingControlComponent } from './tobic-item/topic-timing-control/topic-timing-control.component';

@Component({
  selector: 'app-agenda',
  templateUrl: './agenda.component.html',
  styleUrls: ['./agenda.component.scss'],
})
export class AgendaComponent implements OnInit, OnDestroy {
  @ViewChildren('topicTimingControl')
  topicTimingControlComponents: QueryList<TopicTimingControlComponent>;
  @Input('meetingApproved') meetingApproved: boolean;
  @Output() meetingStarted: EventEmitter<boolean> = new EventEmitter();
  currentLang: string;
  topicType = TopicType;

  meetingDate: Date;
  meetingDay: number;
  meetingMonth: string;
  meetingYear: number;

  meetingId: number;
  addTopic: boolean = false;

  meetingTopics: MeetingTopicDTO[] = [];
  count: number = 0;

  loading = false;
  isCoordinator = false;
  isCreator = false;

  meetingFinished = false;
  meetingCanceled:boolean = false;
  userId: number;
  started = false;

  isMeetingDayOrAfter = false;
  subscription: Subscription;

  attendanceList: MeetingUserAttendationDTO[] = [];

  constructor(
    public translateService: TranslateService,
    private agendaService: AgendaService,
    private singleMeetingService: SingleMeetingService,
    private activatedRoute: ActivatedRoute,
    private signalRService: SignalRService,
    private authService: AuthService,
    private ngZone: NgZone,
    private storeService: StoreService,
    private localization: LocalizationService
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.checkMode();
    this.getMeetingTopics();
    this.isCoordinator = this.singleMeetingService.meeting.isCoordinator;
    this.meetingFinished = this.singleMeetingService.meetingFinished;
    this.meetingCanceled = this.singleMeetingService.meetingCanceled
    this.isCreator = this.singleMeetingService.meeting.isCreator;
    this.subscription = this.signalRService.onConnected.subscribe((a) => {
      this.registerTrackToSignalR();
    });
    this.userId = this.authService.getUser().userId;
    this.isReachedMeetingDate();
    this.getAttendanceList();
    this.localization.isArabic$.subscribe(() => {});
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setMeetingData();
      this.refresh();
    });
  }

  getMeetingTopics() {
    this.loading = true;

    this.agendaService.getMeetingTopics(this.meetingId).subscribe((res) => {
      if (res) {
        this.meetingTopics = res;
        if (this.meetingTopics.length) {
          this.started = this.meetingTopics.some(
            (topic) =>
              topic.topicState == TopicState._2 ||
              topic.topicState == TopicState._4 ||
              topic.topicState == TopicState._3
          );
          this.agendaService.meetingStarted.next(this.started);
        }
        this.loading = false;
      }
    });
  }

  setMeetingData() {
    this.meetingDate = this.singleMeetingService.meeting.meetingFromTime;
    this.meetingYear = this.meetingDate.getFullYear();
    this.meetingMonth =
      this.currentLang === 'ar'
        ? GerogorianMonthsAr[this.meetingDate.getMonth() + 1]
        : GerogorianMonthsEn[this.meetingDate.getMonth() + 1];

    this.meetingDay = this.meetingDate.getDate();
  }

  checkMode() {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.meetingId = +params.get('id');

      if (this.meetingId) {
        this.setMeetingData();
      }
    });
  }

  addTopicToList(topic: MeetingTopicDTO) {
    this.meetingTopics.push(topic);
  }

  beginMeeting() {
    this.started = true;
    this.agendaService.meetingStarted.next(true);
    this.meetingStarted.emit(true)
    let firstTopicIndex = this.meetingTopics.findIndex(
      (t) => t.topicState === TopicState._1
    );
    let firstTopic =
      this.topicTimingControlComponents.toArray()[firstTopicIndex];

    if (firstTopic) {
      firstTopic.onBeginTopic();
    }
  }

  onNextTopic(currentIndex: number, nextIndex: number) {
    let currentTopicId = this.meetingTopics[currentIndex].id;
    let nextTopicId = this.meetingTopics[nextIndex].id;

    this.agendaService
      .nextTopic(currentTopicId, nextTopicId, currentIndex)
      .subscribe((res) => {
        if (res?.nextTopicId) {
          this.meetingTopics[currentIndex].topicAcualStartDateTime =
            res.currentStartDate;
          this.meetingTopics[currentIndex].topicAcualEndDateTime =
            res.currentEndDate;
          let nextIndex = this.meetingTopics.findIndex(
            (topic) => topic.id == res.nextTopicId
          );
          this.topicTimingControlComponents.toArray()[nextIndex].beginCount();
        } else {
          this.agendaService.endTopic(currentTopicId).subscribe(() => {
            this.topicTimingControlComponents
              .toArray()
              [currentIndex].finishTopic();
              this.meetingStarted.emit(false)
            this.finishMeeting();
          });
        }
      });
  }

  cancelTopic(topicId: number) {
    this.meetingTopics.find((topic) => topic.id == topicId).topicState =
      TopicState._5;
  }

  finishMeeting() {
    this.meetingFinished = true;
    this.singleMeetingService.meetingFinished = true;
    this.singleMeetingService.meeting.isFinished = true;
    this.agendaService.meetingEnded.next(true);
    this.sendAttendanceList();
  }

  refresh() {
    this.getMeetingTopics();
  }

  registerTrackToSignalR() {
    this.signalRService.on('NextTopicListener', (data, userId) => {
      this.ngZone.run(() => {
        let currentIndex = data.currentIndex;
        let nextIndex = this.meetingTopics.findIndex(
          (topic) => topic.id == data.nextTopicId
        );
        this.topicTimingControlComponents.toArray()[currentIndex].finishTopic();
        if (data.nextTopicId) {
          this.topicTimingControlComponents.toArray()[nextIndex].beginCount();
        } else {
          this.finishMeeting();
        }
      });
    });

    this.signalRService.on('BeginTopicListener', (topicId, userId) => {
      this.ngZone.run(() => {
        if (userId != this.userId) {
          let topicIndex = this.meetingTopics.findIndex(
            (topic) => topic.id === topicId
          );
          this.started = true;
          this.agendaService.meetingStarted.next(this.started);
          this.topicTimingControlComponents.toArray()[topicIndex].beginCount();
        }
      });
    });

    this.signalRService.on('EndTopicListener', (topicId, userId) => {
      if (userId != this.userId) {
        let topicIndex = this.meetingTopics.findIndex(
          (topic) => topic.id === topicId
        );

        this.topicTimingControlComponents.toArray()[topicIndex].finishTopic();
        this.finishMeeting();
      }
    });

    this.signalRService.on('PauseTopicListener', (topicId, userId) => {
      this.ngZone.run(() => {
        if (userId != this.userId) {
          let topicIndex = this.meetingTopics.findIndex(
            (topic) => topic.id === topicId
          );

          this.topicTimingControlComponents.toArray()[topicIndex].pauseCount();
        }
      });
    });

    this.signalRService.on('ResumeTopicListener', (topicId, userId) => {
      this.ngZone.run(() => {
        if (userId != this.userId) {
          let topicIndex = this.meetingTopics.findIndex(
            (topic) => topic.id === topicId
          );

          this.topicTimingControlComponents.toArray()[topicIndex].resumeCount();
        }
      });
    });

    this.signalRService.on(
      'InsertTopicCommentListener',
      (topicComments: TopicCommentDTO[], userId) => {
        this.ngZone.run(() => {
          if (userId != this.userId) {
            this.storeService.refreshTopicCommentsRecommendations$.next(
              topicComments[0]
            );
          }
        });
      }
    );

    this.signalRService.on(
      'InsertSurveyListener',
      (voting: SurveyDTO, userId) => {
        this.ngZone.run(() => {
          if (userId != this.userId) {
            this.storeService.refreshTopicVotings$.next(voting);
          }
        });
      }
    );

    this.signalRService.on(
      'SurveyAnswerUserListener',
      (answerUsers: SurveyAnswerUserDTO[], topicId: number, userId) => {
        this.ngZone.run(() => {
          if (userId !== +this.userId) {
            this.storeService.refreshAnswerUsers$.next({
              answerUsers,
              topicId,
              firstId: answerUsers[0]?.surveyAnswerUserId,
            });
          }
        });
      }
    );
  }

  isReachedMeetingDate() {
    // Cheack if meeting date reached or passed.

    let today: Date = new Date();
    today.setHours(0, 0, 0, 0);
    let meetingDay = new Date(this.meetingDate);
    meetingDay.setHours(0, 0, 0, 0);
    this.isMeetingDayOrAfter = today.getTime() >= meetingDay.getTime();
  }

  getAttendanceList() {
    this.agendaService
      .getMeetingAttendanceList(this.meetingId)
      .subscribe((res) => {
        this.attendanceList = [...res];
        if (!this.meetingFinished) {
          this.attendanceList = this.attendanceList.map((member) => {
            return {
              ...member,
              attended: true,
            } as MeetingUserAttendationDTO;
          });
        }
      });
  }

  sendAttendanceList() {
    this.agendaService
      .saveAttendeeList(
        this.meetingId,
        this.attendanceList
          .filter((attendee) => attendee.attended)
          .map((att) => new AttendeeDTO({ id: att.userId, type: att.type }))
      )
      .subscribe();
  }
}
