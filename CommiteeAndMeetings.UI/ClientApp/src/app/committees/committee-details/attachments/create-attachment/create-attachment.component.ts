import { AuthService } from './../../../../auth/auth.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { AttachmentsService } from './../attachments.service';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { BehaviorSubject } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import {
  CommiteeMemberDTO,
  LookUpDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { mimeTypeValidator } from 'src/app/shared/mime-type';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Component({
  selector: 'app-create-attachment',
  templateUrl: './create-attachment.component.html',
  styleUrls: ['./create-attachment.component.scss'],
})
export class CreateAttachmentComponent implements OnInit {
  currentLang;
  postForm: FormGroup;
  imagePreview: string;
  isVisible = false;
  isLoading = false;
  privacy = 'public';
  usersChanged$ = new BehaviorSubject('');
  users: LookUpDTO[] = [];
  lookupTypes = SettingControllers;
  files: File[] = [];
  committeeId: string;
  userName;
  userImage;
  saving = false;
  @ViewChild('fileInput') fileInput: any;

  constructor(
    private translateService: TranslateService,
    public lookupService: LookupService,
    private attachmentsService: AttachmentsService,
    private modalService: SharedModalService,
    private modal: NzModalService,
    private storeService: StoreService,
    private authService: AuthService,
    private browserService:BrowserStorageService
  ) {}

  ngOnInit(): void {
    this.currentLang = this.translateService.currentLang;
    this.initForm();
    this.initUsers();
    this.postForm.controls['privacy'].valueChanges.subscribe((type) => {
      this.privacy = type;
    });
    this.setCreatedByUserDetails();
  }

  initForm() {
    this.postForm = new FormGroup({
      description: new FormControl(null, [
        Validators.required,
        Validators.maxLength(120),
      ]),
      files: new FormControl(null, {
        validators: [Validators.required],
        asyncValidators: [mimeTypeValidator],
      }),
      selectedUsers: new FormControl([]),
      privacy: new FormControl('public'),
    });
  }

  onSelectFile(event: Event) {
    const selectedFiles = (event.target as HTMLInputElement).files;

    for (let i = 0; i < selectedFiles.length; i++) {
      this.files.push(selectedFiles[`${i}`]);
    }

    this.postForm.patchValue({ files: this.files });
    this.fileInput.nativeElement.value = '';
    this.postForm.get('files').updateValueAndValidity();
  }

  removeSelectedFile(index) {
    this.files.splice(index, 1);
    this.postForm.patchValue({ files: this.files });
    this.postForm.get('files').updateValueAndValidity();
  }

  onSave() {
    if (!this.postForm.valid) return;
    this.saving = true;
    const description = this.postForm.value.description;
    const uploadedFiles = this.postForm.value.files;
     const id = this.browserService.decrypteString(this.committeeId)
    this.attachmentsService
      .postAttachment(
        uploadedFiles,
        description,
        this.privacy === 'public' ? true : false,
        this.postForm.value.selectedUsers,
        this.browserService.encrypteString(id)
      )
      .subscribe((res) => {
        if (res) {
          //insert new attachment to the list then reset and close..
          this.storeService.refreshDocuments$.next({
            ...res[0],
            justInserted: true,
          });
          this.storeService.refreshTimelineItems$.next({
            ...res[0],
            type: 'Attachment',
            justInserted: true,
          });
        }
      });
    this.saving = false;
    this.close();
  }

  initUsers() {
    this.usersChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getMembersLookup(
            20,
            0,
            this.committeeId,
            text ? text : undefined,
            []
          )
        )
      )
      .subscribe((res: CommiteeMemberDTO[]) => {
        this.users = res.filter(
          (user) => user.userId !== +this.authService.getUser().userId
        );

        this.isLoading = false;
      });
  }

  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.USERS:
        this.usersChanged$.next(value);
        break;
      default:
        break;
    }
  }

  close() {
    this.postForm.reset();
    this.modalService.destroyModal();
  }

  createTplModal(tplTitle: TemplateRef<{}>, tplContent: TemplateRef<{}>): void {
    this.modal.create({
      nzTitle: tplTitle,
      nzContent: tplContent,
      nzFooter: null,
      nzClassName: 'my-modal',
      nzMaskClosable: true,
      nzClosable: true,
      nzWidth: 450,
    });
  }

  setCreatedByUserDetails() {
    this.userName =
      this.currentLang === 'ar'
        ? this.authService.getUser().fullNameAr
        : this.authService.getUser().fullNameEn;

    this.userImage = this.authService.getUser().userImage;
  }
}
