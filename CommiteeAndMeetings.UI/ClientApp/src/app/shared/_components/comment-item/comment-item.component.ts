import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DateFormatterService } from 'ngx-hijri-gregorian-datepicker';
import { AuthService } from 'src/app/auth/auth.service';
import {
  Component,
  Input,
  OnInit,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-comment-item',
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.scss'],
})
export class CommentItemComponent implements OnInit, OnChanges {
  @Input() item: any;
  currentLang: string;
  createdUserName: string;
  createdUserTitle: string;
  createdUserImage: string;
  createdOnDate;
  createdOnTime:Date;
  offset:number
  constructor(
    public translateService: TranslateService,
    private authService: AuthService,
    private dateService: DateFormatterService,
    public dateFormatService:NgbDateService
  ) {}

  ngOnInit(): void {
    this.offset = new Date().getTimezoneOffset();
    this.langChange();
    this.setCreatedByUserDetails();
    this.getCreatedOnDate();
    this.createdOnTime =  new Date(
      Date.UTC(this.item?.createdOn.getFullYear(),
      this.item?.createdOn.getMonth(),
      this.item?.createdOn.getDate(),
      this.item?.createdOn.getHours(),
      this.item?.createdOn.getMinutes(),
      0,
      0
    ))
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.setCreatedByUserDetails();
  }
  setCreatedByUserDetails() {
    if (this.item.comment?.createdByUser || this.item?.createdByUser) {
      this.createdUserName =
        (this.currentLang === 'ar'
          ? this.item.comment.createdByUser?.fullNameAr
          : this.item.comment.createdByUser?.fullNameEn) ||
        (this.currentLang === 'ar'
          ? this.item.createdByUser?.fullNameAr
          : this.item.createdByUser?.fullNameEn);

      this.createdUserTitle = '';
      this.createdUserImage =
        this.item.comment.createdByUser?.profileImage ||
        this.item.createdByUser?.profileImage;
      return;
    } else if (this.item.justInserted) {
      this.createdUserName =
        this.currentLang === 'ar'
          ? this.authService.getUser().fullNameAr
          : this.authService.getUser().fullNameEn;
      this.createdUserTitle = '';
      this.createdUserImage = this.authService.getUser().userImage;
    }
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setCreatedByUserDetails();
    });
  }

  getCreatedOnDate() {
    let ngbHijri: NgbDateStruct = this.dateService.ToHijri({
      year: this.item?.createdOn?.getFullYear(),
      month: this.item?.createdOn?.getMonth() + 1,
      day: this.item?.createdOn?.getDate(),
    });

    let hijriDateString = `${ngbHijri.day}/${ngbHijri.month}/${ngbHijri.year}`;
    let gregDateString = `${this.item?.createdOn?.getDate()}/${this.item?.createdOn?.getMonth()}/${this.item?.createdOn?.getFullYear()}`;

    this.createdOnDate =
      this.translateService.currentLang == 'ar'
        ? hijriDateString
        : gregDateString;
  }

  getCreatedOnTime() {
    const hours = +this.item?.createdOn?.getHours();
    const minutes = +this.item?.createdOn?.getMinutes();
    const seconds = +this.item?.createdOn?.getSeconds();

    // this.createdOnTime = `${hours}:${minutes}:${seconds}`;
  }
}
