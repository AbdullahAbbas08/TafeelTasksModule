import { SingleMeetingService } from './../../single-meeting.service';
import { AgendaService } from './../agenda.service';
import {
  TopicType,
  MeetingTopicDTO,
  SwaggerClient,
} from './../../../../core/_services/swagger/SwaggerClient.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import {
  HtmlEditorService,
  RichTextEditorComponent,
  ToolbarService,
} from '@syncfusion/ej2-angular-richtexteditor';
import { RichTextTools } from '../../rich-text.setting';

let startHours: number[] = [];
let startMins: number[] = [];
let endHours: number[] = [];
let endMins: number[] = [];

@Component({
  selector: 'app-add-tobic',
  templateUrl: './add-tobic.component.html',
  styleUrls: ['./add-tobic.component.scss'],
  providers: [ToolbarService, HtmlEditorService],
})
export class AddTobicComponent implements OnInit, AfterViewInit {
  @Input() lastTopic: MeetingTopicDTO;
  @Output() close: EventEmitter<boolean> = new EventEmitter();
  @Output() addTopic: EventEmitter<MeetingTopicDTO> = new EventEmitter();
  @ViewChild('toolsRTE') public rteObj: RichTextEditorComponent;

  topicForm: FormGroup;
  topicType = TopicType;
  meetingDate: Date;
  meetingStartTime: Date;
  meetingEndTime: Date;
  topicStartTime: Date;
  suggestedEndTime: Date;

  meetingId: number;
  isTopic = true;

  showRichTextEditor = false;
  public tools: object = RichTextTools;
  pointsHTML = ``;
  isCollapsed = false;
  disabledEndInput:boolean = false
  constructor(
    private agendaService: AgendaService,
    private singleMeetingService: SingleMeetingService,
    private swagger:SwaggerClient
  ) {
    this.setMeetingData();
    this.initForm();
  }

  ngOnInit(): void {
    this.setTimeLimits();
    this.subscribeToFormChanges();
    this.checkMinutesDisabled()
    this.topicForm.get('endTime').valueChanges.subscribe((val: Date) => {
      val.setDate(this.topicStartTime.getDate());
      if (val.getTime() < this.topicStartTime.getTime()) {
        this.topicForm.patchValue({ endTime: this.suggestedEndTime });
      }
    });
  }

  ngAfterViewInit() {
    this.showRichTextEditor = true;
  }

  initForm() {
    this.topicForm = new FormGroup({
      type: new FormControl(TopicType._1, [Validators.required]),
      title: new FormControl('', [Validators.required]),
      subject: new FormControl(''),
      topicMinutes:new FormControl(''),
      startTime: new FormControl(null, [Validators.required]),
      endTime: new FormControl(null, [Validators.required]),
    });
  }
  checkMinutesDisabled(){
    this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet('TopicMeetingMintues').subscribe((res) => {
         if(res.systemSettingValue === "1"){
             this.disabledEndInput = true;
         } else {
          this.topicForm.controls['topicMinutes'].setValidators([Validators.required]);
          this.disabledEndInput = false
         }
         this.topicForm.controls['topicMinutes'].updateValueAndValidity();
    })
  }
  submitTopic() {
    if (this.topicForm.valid) {
      let fromTime: Date = this.topicForm.value.startTime;
      let toTime: Date = this.topicForm.value.endTime;

      fromTime.setSeconds(0);
      toTime.setSeconds(0);

      let utcDate: Date = new Date(
        Date.UTC(
          this.meetingDate.getFullYear(),
          this.meetingDate.getMonth(),
          this.meetingDate.getDate()
        )
      );

      fromTime.setFullYear(
        this.meetingDate.getFullYear(),
        this.meetingDate.getMonth(),
        this.meetingDate.getDate()
      );

      toTime.setFullYear(
        this.meetingDate.getFullYear(),
        this.meetingDate.getMonth(),
        this.meetingDate.getDate()
      );

      // Assign the value of the rich text to the Topic Points
      this.topicForm.patchValue({ subject: this.pointsHTML });

      let topic: MeetingTopicDTO = new MeetingTopicDTO({
        topicType: this.topicForm.value.type,
        topicTitle: this.topicForm.value.title,
        topicPoints: this.topicForm.value.subject,
        topicFromDateTime: fromTime,
        topicToDateTime: toTime,
        topicDate: utcDate,
        meetingId: this.meetingId,
      });

      this.agendaService.addTopic(topic).subscribe((res) => {
        this.addTopic.emit(res[0]);
        this.closeAddTopic();
      });
    }
  }

  closeAddTopic() {
    this.close.emit(true);
  }

  setMeetingData() {
    this.meetingId = this.singleMeetingService.meeting.id;
    this.meetingDate = this.singleMeetingService.meeting.date;
    this.meetingStartTime = this.singleMeetingService.meeting.meetingFromTime;
    this.meetingEndTime = this.singleMeetingService.meeting.meetingToTime;
  }
  onKeyUp(event){
    if (this.lastTopic) {
      this.topicStartTime = this.lastTopic.topicToDateTime;
    } else {
      this.topicStartTime = this.meetingStartTime;
    }
    this.topicForm.patchValue({ startTime: this.topicStartTime });

    this.suggestedEndTime = new Date(this.topicStartTime);

    const inputValue = event.target.value;
    this.suggestedEndTime.setMinutes(this.suggestedEndTime.getMinutes() + +inputValue);
    this.topicForm.patchValue({
      endTime: this.suggestedEndTime,
    });
  }
  setTimeLimits() {
    if (this.lastTopic) {
      this.topicStartTime = this.lastTopic.topicToDateTime;
    } else {
      this.topicStartTime = this.meetingStartTime;
    }
    this.topicForm.patchValue({ startTime: this.topicStartTime });

    this.suggestedEndTime = new Date(this.topicStartTime);

    this.suggestedEndTime.setMinutes(this.suggestedEndTime.getMinutes() + 1);

    this.topicForm.patchValue({
      endTime: this.suggestedEndTime,
    });

    for (let i = 0; i < this.topicStartTime.getHours(); i++) {
      startHours.push(i);
    }

    startHours = Array(this.topicStartTime.getHours())
      .fill(0)
      .map((x, i) => {
        return i;
      });

    startMins = Array(this.topicStartTime.getMinutes())
      .fill(0)
      .map((x, i) => {
        return i + 1;
      });

    endHours = Array(23 - this.meetingEndTime.getHours())
      .fill(23)
      .map((x, i) => {
        return x - i;
      });

    endMins = Array(59 - this.meetingEndTime.getMinutes())
      .fill(59)
      .map((x, i) => {
        return x - i;
      });
  }

  disabledHours(): number[] {
    return [...startHours, ...endHours];
  }

  disabledMinutes(hour: number): number[] {
    if (hour === startHours.length) {
      return startMins;
    } else if (hour === 23 - endHours.length) {
      return endMins;
    } else if (hour === startHours.length && hour === endHours.length) {
      return [...startMins, ...endMins];
    } else {
      return [];
    }
  }

  subscribeToFormChanges() {
    this.topicForm.get('type').valueChanges.subscribe((val) => {
      if (val === this.topicType._1) {
        this.topicForm.get('title').setValidators(Validators.required);
        this.isTopic = true;
      } else {
        this.topicForm.get('title').clearValidators();
        this.isTopic = false;
      }
      this.topicForm.get('title').updateValueAndValidity();
    });
  }
}
