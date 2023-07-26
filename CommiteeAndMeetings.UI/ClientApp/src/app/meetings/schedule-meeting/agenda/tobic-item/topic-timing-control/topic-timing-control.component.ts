import { AgendaService } from './../../agenda.service';
import { TopicState } from './../../../../../core/_services/swagger/SwaggerClient.service';
import { MeetingTopicDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import {
  Component,
  EventEmitter,
  Input,
  NgZone,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { CountdownComponent, CountdownEvent } from 'ngx-countdown';
import { TopicTimingState } from 'src/app/shared/_enums/AppEnums';
import {
  countDownTimerConfigModel,
  CountupTimerService,
  timerTexts,
} from 'ngx-timer';

@Component({
  selector: 'app-topic-timing-control',
  templateUrl: './topic-timing-control.component.html',
  styleUrls: ['./topic-timing-control.component.scss'],
})
export class TopicTimingControlComponent implements OnInit {
  @Input() topic: MeetingTopicDTO;
  @Input() lastTopic: boolean;
  @Input() isCoordinator;
  @Input() isCreator;
  @Output() nextTopic: EventEmitter<boolean> = new EventEmitter();
  @Output() meetingFinished: EventEmitter<boolean> = new EventEmitter();

  @ViewChild('cd', { static: false }) private countdown: CountdownComponent;

  topicDuration: number = 0;
  remainingTime: number = 0;
  countUpTime: number = 0;

  demand = true;
  autoStart = true;

  config: countDownTimerConfigModel;

  timingState: TopicTimingState = TopicTimingState.NotStarted;
  topicTimingState = TopicTimingState;

  paused: boolean = true;
  pauseTime: number = 0;

  topicState: TopicState;
  originalTimeFinished: boolean = false;

  constructor(
    private agendaService: AgendaService,
    private countupTimerService: CountupTimerService
  ) {}

  ngOnInit(): void {
    this.agendaService.cancelledTopic.subscribe((id) => {
      if (id == this.topic.id) {
        this.topic.topicState = TopicState._5;
      }
    });
    this.countUpConfig();
    this.calculateTopicDuration();
    this.checkTopicState();
  }

  onBeginTopic() {
    this.agendaService.beginTopic(this.topic.id).subscribe();
    this.beginCount();
  }

  beginCount() {
    this.countdown.begin();
    this.topic.topicState = TopicState._2;
    this.paused = false;
    if (this.topicDuration > 180) {
      this.timingState = TopicTimingState.Running;
    } else {
      this.timingState = TopicTimingState.Warning;
    }
    this.topic.topicAcualStartDateTime = new Date();
  }

  onPause() {
    // Pause Topic API CALL
    this.agendaService.pauseTopic(this.topic.id).subscribe();

    this.pauseCount();
  }

  pauseCount() {
    this.originalTimeFinished
      ? this.countupTimerService.pauseTimer()
      : this.countdown.pause();

    this.paused = true;
  }

  onResume() {
    this.agendaService.resumeTopic(this.topic.id).subscribe();
    this.resumeCount();
  }

  resumeCount() {
    this.originalTimeFinished
      ? this.countupTimerService.startTimer()
      : this.countdown.resume();

    this.paused = false;
  }

  onNextTopic() {
    if (this.lastTopic) {
      this.agendaService.endTopic(this.topic.id).subscribe();
      this.meetingFinished.emit(true);
    } else {
      this.nextTopic.emit(true);
    }

    this.finishTopic();
  }

  finishTopic() {
    this.timingState = TopicTimingState.Completed;
    this.topic.topicState = TopicState._3;

    this.originalTimeFinished
      ? this.countupTimerService.pauseTimer()
      : this.countdown.stop();

    this.remainingTime = undefined;
    this.originalTimeFinished = false;
    this.topicDuration =
      (new Date().getTime() - this.topic.topicAcualStartDateTime.getTime()) /
      1000;
  }

  handleEvent(e: CountdownEvent) {
    if (
      e.action === 'notify' &&
      e.left === 180000 &&
      this.timingState !== TopicTimingState.Completed
    ) {
      this.timingState = TopicTimingState.Warning;
    } else if (
      e.action === 'done' &&
      this.timingState !== TopicTimingState.Completed
    ) {
      this.countupTimerService.startTimer(new Date());
      this.timingState = TopicTimingState.Alarm;

      this.originalTimeFinished = true;
    }
  }

  checkTopicState() {
    this.topicState = this.topic.topicState;

    switch (this.topicState) {
      case TopicState._1:
        this.timingState = TopicTimingState.NotStarted;
        this.remainingTime = this.topicDuration;
        break;
      case TopicState._2:
        this.calculateRemainingTime(true);
        this.paused = false;
        break;
      case TopicState._4:
        this.calculateRemainingTime(false);
        this.paused = true;
        break;
      case TopicState._3:
        this.timingState = TopicTimingState.Completed;
        if (
          this.topic.topicAcualEndDateTime &&
          this.topic.topicAcualStartDateTime
        ) {
          this.topicDuration =
            (this.topic.topicAcualEndDateTime?.getTime() -
              this.topic.topicAcualStartDateTime?.getTime()) /
            1000;
        }
        this.lastTopic && this.meetingFinished.emit(true);
        break;
    }
  }

  calculateRemainingTime(autoStart: boolean) {
    this.remainingTime = this.topic.reminingDuration?.remining;

    let positiveRemaining = this.topic.reminingDuration?.up;

    if (positiveRemaining) {
      this.originalTimeFinished = false;

      this.remainingTime > 180
        ? (this.timingState = TopicTimingState.Running)
        : (this.timingState = TopicTimingState.Warning);

      this.demand = !autoStart;
    } else {
      this.originalTimeFinished = true;

      this.timingState = TopicTimingState.Alarm;

      if (autoStart) {
        this.countupTimerService.startTimer(
          this.setCountUpTime(this.remainingTime)
        );
      } else {
        this.countupTimerService.startTimer(
          this.setCountUpTime(this.remainingTime)
        );
        this.countupTimerService.pauseTimer();
      }
    }
  }

  setCountUpTime(seconds: number) {
    let countDate = new Date();

    if (seconds) {
      switch (true) {
        case seconds < 60:
          countDate.setSeconds(countDate.getSeconds() - seconds);
          break;
        case seconds > 60 && seconds < 3600:
          countDate.setSeconds(countDate.getSeconds() - (seconds % 60));
          countDate.setMinutes(
            countDate.getMinutes() - Math.floor(seconds / 60)
          );
          break;
        case seconds > 3600:
          countDate.setHours(countDate.getHours() + Math.floor(seconds / 3600));
          countDate.setMinutes(
            countDate.getMinutes() -
              Math.floor(seconds / 60) -
              Math.floor(seconds / 3600) * 60
          );
          countDate.setSeconds(countDate.getSeconds() - (seconds % 60));
          break;
        default:
          break;
      }
    }

    return countDate;
  }
  calculateTopicDuration() {
    this.topicDuration =
      (this.topic.topicToDateTime.getTime() -
        this.topic.topicFromDateTime.getTime()) /
      1000;
  }

  countUpConfig() {
    this.config = new countDownTimerConfigModel();
    this.config.timerTexts = new timerTexts();
    this.config.timerTexts.hourText = ':';
    this.config.timerTexts.minuteText = ':';
    this.config.timerTexts.secondsText = ' ';
  }
}
