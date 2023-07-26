import { MinuteOfMeetingDTO } from './../../../../core/_services/swagger/SwaggerClient.service';
import {
  Component,
  Input,
  OnInit,
  Output,
  EventEmitter,
  ViewChild,
  ElementRef,
  Renderer2,
  AfterViewInit,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SingleMeetingService } from '../../single-meeting.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
@Component({
  selector: 'app-minuets-of-meeting-item',
  templateUrl: './minuets-of-meeting-item.component.html',
  styleUrls: ['./minuets-of-meeting-item.component.scss'],
})
export class MinuetsOfMeetingItemComponent implements OnInit, AfterViewInit {
  @Input() minute: MinuteOfMeetingDTO;
  @Input() meetingClosed;
  @Output() deleted: EventEmitter<boolean> = new EventEmitter<boolean>();

  @ViewChild('points') pointsElement: ElementRef;
  currentLang: string;
  visible = false;

  constructor(
    private translateService: TranslateService,
    public singleMeeting: SingleMeetingService,
    private translate: TranslateService,
    private notificationService: NzNotificationService,
    private renderer: Renderer2
  ) {}

  ngOnInit(): void {
    this.langChange();
  }

  ngAfterViewInit() {
    this.renderer.setProperty(
      this.pointsElement.nativeElement,
      'innerHTML',
      this.minute?.description
    );
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }
  onDelete() {
    this.singleMeeting.deleteMOM(this.minute.id).subscribe((res) => {
      if (res) {
        this.deleted.emit(true);
        this.translate
          .get('This point has been successfully deleted')
          .subscribe((translateValue) =>
            this.notificationService.success(translateValue, '')
          );
      } else {
        this.translate
          .get('Canot delete this point')
          .subscribe((translateValue) =>
            this.notificationService.warning(translateValue, '')
          );
      }
    });
  }
}
