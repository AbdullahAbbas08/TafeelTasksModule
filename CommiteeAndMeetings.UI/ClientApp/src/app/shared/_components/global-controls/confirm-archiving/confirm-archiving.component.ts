import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CommitteeService } from 'src/app/committees/committee.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import {
  SwaggerClient,
  ValidityPeriod,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { LocalizationService } from 'src/app/shared/_services/localization.service';

@Component({
  selector: 'app-confirm-archiving',
  templateUrl: './confirm-archiving.component.html',
  styleUrls: ['./confirm-archiving.component.scss'],
})
export class ConfirmArchivingComponent implements OnInit {
  @Input() committeeId: string;
  @Input() validityPeriodId: number;
  @Input() validityPeriodFrom: Date;
  @Input() createdOn:Date;
  constructor(
    public translateService: TranslateService,
    private modalService: SharedModalService,
    private swagger: SwaggerClient,
    private committeeService: CommitteeService,
    private bs:BrowserStorageService
  ) {}

  ngOnInit(): void {}

  close() {
    this.modalService.destroyModal();
  }

  archivePeriod() {
    this.modalService.destroyModal();
    this.swagger.apiCommiteesArchivePost(this.bs.encryptCommitteId(this.committeeId)).subscribe((res) => {
      var archivedPeriod;
      if (res) {
        this.modalService.createNotification('success', 'ArchiveSuccess');
       if(this.validityPeriodFrom === undefined){
        archivedPeriod = new ValidityPeriod({
          periodState: 2,
          validityPeriodId: this.validityPeriodId,
          validityPeriodFrom: this.createdOn,
          validityPeriodTo: new Date(),
        });
       } else {
        archivedPeriod = new ValidityPeriod({
          periodState: 2,
          validityPeriodId: this.validityPeriodId,
          validityPeriodFrom: this.validityPeriodFrom,
          validityPeriodTo: new Date(),
        });
       }
        this.committeeService.committeePeriodChange$.next(archivedPeriod);
        this.committeeService.archiveCurrentPeriod$.next(true);
      }
    });
  }
}
