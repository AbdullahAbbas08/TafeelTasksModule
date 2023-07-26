import { MOMService } from './../mom.service';
import { Component, EventEmitter, OnInit, Output, AfterViewInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  MeetingTopicLookupDTO,
  MinuteOfMeetingDTO,
  MinuteOfMeetingTopicDTO,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SingleMeetingService } from '../../single-meeting.service';
import { ActivatedRoute } from '@angular/router';
import { StoreService } from 'src/app/shared/_services/store.service';
import { RichTextTools } from '../../rich-text.setting';
import { HtmlEditorService, ToolbarService } from '@syncfusion/ej2-angular-richtexteditor';

export enum MomTYPE {
  NEW = 'CreateNew',
  FROMTOPIC = 'FromPreviousTopic',
}

@Component({
  selector: 'app-add-mom-item',
  templateUrl: './add-mom-item.component.html',
  styleUrls: ['./add-mom-item.component.scss'],
  providers: [ToolbarService, HtmlEditorService],
})
export class AddMOMItemComponent implements OnInit, AfterViewInit {
  @Output() close: EventEmitter<boolean> = new EventEmitter();
  @Output() addMOM: EventEmitter<MinuteOfMeetingDTO> = new EventEmitter();

  momForm: FormGroup;
  momType = MomTYPE;
  selectedType = MomTYPE.NEW;
  meetingDate: Date;
  meetingId: number;

  selectedTopicId: number;
  topicList: MeetingTopicLookupDTO[] = [];

  referredTopics = [];

  public tools: object = RichTextTools;
  pointsHTML = ``;
  showRichTextEditor = false;
  hideFromTopic:boolean = false
  constructor(
    private momService: MOMService,
    private singleMeetingService: SingleMeetingService,
    private activatedRoute: ActivatedRoute,
    private storeService: StoreService,
    private swagger:SwaggerClient
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.checkMode();
    this.getTopics();
    this.showFromTopic()
  }

  ngAfterViewInit() {
    this.showRichTextEditor = true;
  }

  initForm() {
    this.momForm = new FormGroup({
      title: new FormControl('', [Validators.required]),
      subject: new FormControl(''),
    });
  }

  submitMOM() {
    if (this.momForm.valid) {
      // Assign the value of the rich text to the MOM Points
      this.momForm.patchValue({ subject: this.pointsHTML });

      let newMom: MinuteOfMeetingDTO = new MinuteOfMeetingDTO({
        title: this.momForm.value.title,
        description: this.momForm.value.subject,
        meetingId: this.meetingId,
        topics: this.referredTopics.map((obj) => {
          return new MinuteOfMeetingTopicDTO({
            meetingTopicId: obj.id,
          });
        }),
      });

      this.momService.addMOM(newMom).subscribe((res) => {
        this.addMOM.emit(res[0]);
        this.storeService.refreshMinutesOfMeeting$.next(res[0]);
        this.closeAddMom();
      });
    }
  }

  closeAddMom() {
    this.close.emit(true);
  }

  setMeetingData() {
    this.meetingDate = this.singleMeetingService.meeting.date;
  }

  checkMode() {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.meetingId = +params.get('id');

      if (this.meetingId) {
        this.setMeetingData();
      }
    });
  }

  getTopics() {
    this.momService.getMeetingTopicsLookup(this.meetingId).subscribe((res) => {
      if (res) this.topicList = res;
    });
  }

  onSelectPreviousTopic() {
    let selectedTopic: MeetingTopicLookupDTO = this.topicList.find(
      (topic) => topic.id === this.selectedTopicId
    );

    if (selectedTopic) {
      this.momForm.patchValue({
        title: selectedTopic.title,
      });

      this.pointsHTML = selectedTopic.points;
    }
  }

  addReferredTopic(topicId: number) {
    this.referredTopics.push({
      id: topicId,
      title: this.topicList.find((topic) => topic.id === topicId).title,
    });
  }

  removeSelectedTopic(index: number) {
    this.referredTopics.splice(index, 1);
  }
  showFromTopic(){
    this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet('fromPerviosTopic').subscribe((res) => {
      if(res.systemSettingValue === "1"){
        this.hideFromTopic = true
      } else {
        this.hideFromTopic = false
      }
    })
  }
}
