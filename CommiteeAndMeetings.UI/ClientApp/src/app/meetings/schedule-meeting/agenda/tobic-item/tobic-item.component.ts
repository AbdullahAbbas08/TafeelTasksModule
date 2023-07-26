import { SingleMeetingService } from './../../single-meeting.service';
import { Subscription } from 'rxjs';
import { AgendaService } from './../agenda.service';
import {
  MeetingTopicDTO,
  TopicState,
  TopicType,
} from './../../../../core/_services/swagger/SwaggerClient.service';
import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  Renderer2,
  ViewChild,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MeetingActions } from 'src/app/shared/_enums/AppEnums';
import { SharedModalService } from 'src/app/core/_services/modal.service';

@Component({
  selector: 'app-tobic-item',
  templateUrl: './tobic-item.component.html',
  styleUrls: ['./tobic-item.component.scss'],
})
export class TobicItemComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() topic: MeetingTopicDTO;
  @Input() isCoordinator;
  @Input() isCreator;
  @Input() seqNumber;

  @Output() cancelledTopic: EventEmitter<number> = new EventEmitter();
  @ViewChild('points') pointsElement: ElementRef;
  currentLang: string;
  topicType = TopicType;
  visible = false;
  topicState = TopicState;

  constructor(
    private translateService: TranslateService,
    private modalService: SharedModalService,
    private agendaService: AgendaService,
    private renderer: Renderer2,
    public singleMeeting: SingleMeetingService
  ) {
  }

  ngOnInit(): void {
    this.langChange();
  }

  ngOnDestroy(): void {}

  ngAfterViewInit() {
    this.renderer.setProperty(
      this.pointsElement.nativeElement,
      'innerHTML',
      this.topic?.topicPoints
    );
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }

  addNewTimeLineItem() {
    this.modalService.openDrawerModal(
      MeetingActions.CreateTopicVoting,
      undefined,
      undefined,
      undefined,
      undefined,
      this.topic.id
    );
  }

  cancelTopic() {
    this.agendaService.cancelTopic(this.topic.id).subscribe((res) => {
      if (res) {
        this.topic.topicState = TopicState._5;
        this.cancelledTopic.emit(this.topic.id);
        this.agendaService.cancelledTopic.next(this.topic.id);
      }
    });
  }
}
