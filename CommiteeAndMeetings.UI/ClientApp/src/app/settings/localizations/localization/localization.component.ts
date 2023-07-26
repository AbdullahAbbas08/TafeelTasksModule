import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { takeUntil } from 'rxjs/operators';
import {
  SwaggerClient,
  CommiteeLocalizationDetailsDTO
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { DestroyService } from 'src/app/shared/_services/destroy.service';

@Component({
  selector: 'app-localization',
  templateUrl: './localization.component.html',
  styles: [],
  providers: [DestroyService],
})
export class LocalizationComponent implements OnInit {
  localization: CommiteeLocalizationDetailsDTO = new CommiteeLocalizationDetailsDTO();
  localizationCreate = false;
  localizationId: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private swagger: SwaggerClient,
    private translate: TranslateService,
    private notificationService: NzNotificationService,
    private destroyServ: DestroyService
  ) {}

  ngOnInit() {
    this.route.params
      .pipe(takeUntil(this.destroyServ.subDestroyed))
      .subscribe((r) => {
        if (!r.committeeLocalizationId) {
          this.localizationCreate = true;
          this.localization = new CommiteeLocalizationDetailsDTO();
        } else {
          this.localizationId = r.committeeLocalizationId;
          this.getLocalizationsData();
        }
      });
  }

  getLocalizationsData() {
    this.swagger
      .apiCommiteeLocalizationsGetByIdGet(this.localizationId)
      .subscribe((localization) => (this.localization = localization));
  }

  editLocalization() {
    if (
      !this.localization.commiteeLocalizationAr ||
      !this.localization.commiteeLocalizationEn ||
      !this.localization.key
    ) {
      return;
    }
    this.swagger
      .apiCommiteeLocalizationsUpdatePut([this.localization])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('TranslationUpdated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/localizations']);
        } else {
          this.translate
            .get('TranslationAlreadyExist')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }

  insertLocalization() {
    if (
      !this.localization.commiteeLocalizationAr ||
      !this.localization.commiteeLocalizationEn ||
      !this.localization.key
    ) {
      return;
    }
    this.swagger
      .apiCommiteeLocalizationsInsertPost([this.localization])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('TranslationCreated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/localizations']);
        } else {
          this.translate
            .get('TranslationAlreadyExist')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }
}
