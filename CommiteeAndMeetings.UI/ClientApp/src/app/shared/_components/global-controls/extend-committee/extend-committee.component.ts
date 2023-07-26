import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DateFormatterService, DateType } from 'ngx-hijri-gregorian-datepicker';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { FormGroup } from '@angular/forms';
import { CommitteeService } from 'src/app/committees/committee.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Component({
  selector: 'app-extend-committee',
  templateUrl: './extend-committee.component.html',
  styleUrls: ['./extend-committee.component.scss'],
})
export class ExtendCommitteeComponent implements OnInit {
  committeeId;
  committeeEndDate: Date;
  newDate = undefined;
  selectedDateType = DateType.Hijri;
  currentLang;
  extendForm: FormGroup;
  minHijri: NgbDateStruct;
  minGreg: NgbDateStruct;

  constructor(
    private dateFormatterService: DateFormatterService,
    private committeeService: CommitteeService,
    private translateService: TranslateService,
    private modalService: SharedModalService,
    private swagger: SwaggerClient,
    private browserService:BrowserStorageService
  ) {}

  ngOnInit(): void {
    this.currentLang = this.translateService.currentLang;
    this.setMinAllowed();
  }

  dateSelected(selectedDate: NgbDateStruct) {
    if (!selectedDate) {
      this.newDate = undefined;
      return;
    }

    if (selectedDate.year < 1900) {
      selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
    }

    this.newDate = new Date(
      Date.UTC(selectedDate.year, selectedDate.month - 1, selectedDate.day)
    );
  }

  submitNewExtendDate() {
    this.swagger
      .apiCommiteesExtendPost(this.browserService.encryptCommitteId(`${this.committeeId}_${this.browserService.getUserRoleId()}`), this.newDate)
      .subscribe((res) => {
        if (res) {
          this.modalService.createNotification('success', 'ExtendSuccess');
          this.committeeService.extendCommittee$.next(this.newDate);
          this.close();
        }
      });
  }

  close() {
    this.modalService.destroyModal();
  }

  setMinAllowed() {
    this.minGreg = {
      year: this.committeeEndDate.getFullYear(),
      month: this.committeeEndDate.getMonth() + 1,
      day: this.committeeEndDate.getDate() + 1,
    };
    this.minHijri = this.dateFormatterService.ToHijri(this.minGreg);
  }
}
