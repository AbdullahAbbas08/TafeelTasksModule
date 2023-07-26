import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject } from 'rxjs';
import { BrowserStorageService } from './browser-storage.service';
import { Title } from '@angular/platform-browser';
import { en_US, ar_EG, NzI18nService } from 'ng-zorro-antd/i18n';
import {
  CommiteeLocalizationDetailsDTODataSourceResult,
  SwaggerClient,
  CommiteeLocalizationDetailsDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';

@Injectable({
  providedIn: 'root',
})
export class LocalizationService {
  private renderer: Renderer2;

  private isArabic = new BehaviorSubject<boolean>(false);
  isArabic$ = this.isArabic.asObservable();

  constructor(
    private translate: TranslateService,
    private rendererFactory: RendererFactory2,
    private swagger: SwaggerClient,
    private pageTitleService: Title,
    private browserStorageService: BrowserStorageService,
    private i18n: NzI18nService
  ) {
    this.renderer = this.rendererFactory.createRenderer(null, null);
  }

  init() {
    this.translate.setDefaultLang('ar');
    let localization_ar =
      this.browserStorageService.getLocal('localization_ar');
    let localization_en =
      this.browserStorageService.getLocal('localization_en');
    const lastUpdateDate = this.browserStorageService.getLocal(
      'localization_lastUpdateDate'
    );
    // if saved in localStorage use it
    if (localization_ar) {
      this.translate.setTranslation('ar', localization_ar);
    }
    if (localization_en) {
      this.translate.setTranslation('en', localization_en);
    }

    this.swagger
      .apiCommiteeLocalizationsGetLastUpDateTimeGet()
      .subscribe((value) => {
        // check if date changed
        if (lastUpdateDate !== value.toString()) {
          localization_ar = null;
          localization_en = null;
          this.browserStorageService.setLocal(
            'localization_lastUpdateDate',
            value
          );
        }

        if (!localization_ar || !localization_en) {
          this.swagger
            .apiCommiteeLocalizationsGetAllGet(
              undefined,
              undefined,
              undefined,
              undefined,
              undefined,
              undefined,
              undefined,
              undefined,
              undefined
            )
            .subscribe(
              (value: CommiteeLocalizationDetailsDTODataSourceResult) => {
                if (value && value.data.length) {
                  // get Arabic localization
                  if (!localization_ar) {
                    this.storeLanguagesAndSetToTranslate('ar', value.data);
                  }
                  // get English localization
                  if (!localization_en) {
                    this.storeLanguagesAndSetToTranslate('en', value.data);
                  }
                } else {
                  console.error('error in get localizations.');
                }
              }
            );
        }
      });

    const culture = this.browserStorageService.getLocal('culture');
    if (culture) {
      this.translate.use(culture);
      if (culture === 'ar') {
        this.i18n.setLocale(ar_EG);
        this.isArabic.next(true);
        this.renderer.addClass(document.body, 'rtl');
      } else {
        this.i18n.setLocale(en_US);
        this.isArabic.next(false);
        this.renderer.removeClass(document.body, 'rtl');
      }
    } else {
      this.i18n.setLocale(ar_EG);
      this.browserStorageService.setLocal('culture', 'ar');
      this.renderer.addClass(document.body, 'rtl');
      this.isArabic.next(true);
      this.translate.use('ar');
    }
  }

  storeLanguagesAndSetToTranslate(
    lang: string,
    value: CommiteeLocalizationDetailsDTO[]
  ) {
    let local = {};
    value.map((item: CommiteeLocalizationDetailsDTO) => {
      local[item.key] =
        lang === 'ar'
          ? item.commiteeLocalizationAr
          : item.commiteeLocalizationEn;
    });
    this.browserStorageService.setLocal(`localization_${lang}`, local);
    this.translate.setTranslation(`${lang}`, local);
  }

  changeLocal() {
    const currentLang = this.browserStorageService.getLocal('culture');
    if (currentLang === 'en') {
      this.translate.use('ar');
      this.browserStorageService.setLocal('culture', 'ar');
      this.swagger['apiCultureUpdateCultureSessionPost']('ar').subscribe(
        (_) => {
          this.renderer.addClass(document.body, 'rtl');
          this.isArabic.next(true);
        }
      ); // to avoid hijri date in server
    } else {
      this.translate.use('en');
      this.browserStorageService.setLocal('culture', 'en');
      this.swagger['apiCultureUpdateCultureSessionPost']('en').subscribe(
        (_) => {
          this.renderer.removeClass(document.body, 'rtl');
          this.isArabic.next(false);
        }
      ); // to avoid hijri date in server
    }

    setTimeout(() => {
      this.translate
        .get('AppTitle')
        .subscribe((text) => this.pageTitleService.setTitle(text));
    }, 1000);
  }

  // getHjiriDayValueFROMAPI() {
  //   // Get Hijri Value Day [1 - 0 - -1] from API, and store it in a behaviorSubject and localStorage
  //   this.swagger.apiSystemSettingsGetSystemSettingByCodeGet('AdjustHijriDate')
  //     .pipe(map((res) => res.systemSettingValue), take(1))
  //     .subscribe((daysValue) => {
  //       // store this value in localStorage to use it in custom pipes, that deny asynchronus code.
  //       localStorage['hijriDaysValue'] = +daysValue;
  //     });
  // }
}
