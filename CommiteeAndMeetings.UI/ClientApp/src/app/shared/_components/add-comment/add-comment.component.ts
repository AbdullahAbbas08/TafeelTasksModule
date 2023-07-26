import { AuthService } from 'src/app/auth/auth.service';
import { Component, Input, OnInit, Output, EventEmitter,  ViewChild } from '@angular/core';
import { CommentDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { NavigationEnd, Router } from '@angular/router';
import { AttachmentsService } from 'src/app/committees/committee-details/attachments/attachments.service';
@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.scss'],
})
export class AddCommentComponent implements OnInit {
  commentText = '';
  @Input() id: number;
  @Input() checkAttachmentIcon:boolean
  @Output() commentAdded = new EventEmitter<any>();
  @Input() count;
  @Input() recommendation: boolean = false;
  @Input() isMeetingCanceled
  userImage: string;
  currentUrl: string;
  hideAttachmentIcon:boolean = false
  @ViewChild('fileInput') fileInput: any;
  files: File[] = [];
  constructor(private authService: AuthService,private router: Router,private AttachmentService:AttachmentsService) {}

  ngOnInit(): void {
    this.userImage = this.authService.getUser().userImage;
    this.checkCurrentComponent();
  }

  addComment() {
      if (this.commentText.length || this.files.length){
        let commentObj = {
          comment: new CommentDTO({ text: this.commentText }),
          id: this.id,
          attachmentFiles:this.files
        };
        this.commentAdded.emit(commentObj);

        this.commentText = '';
        this.files = []
      } 
  }
  onSelectFile(event: Event) {
    const selectedFiles = (event.target as HTMLInputElement).files;
    for (let i = 0; i < selectedFiles.length; i++) {
      this.files.push(selectedFiles[`${i}`]);
    }
    this.fileInput.nativeElement.value = '';
  }
  removeSelectedFile(index) {
    this.files.splice(index, 1);
  }
  checkCurrentComponent(){
    this.currentUrl = this.router.routerState.snapshot.url;
    if (
      this.currentUrl.includes('tasks')
    ) {
      this.hideAttachmentIcon = true;
    } else {
      this.hideAttachmentIcon = false;
    }
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentUrl = this.router.routerState.snapshot.url;
        if (
          this.currentUrl.includes('tasks')
        ) {
          this.hideAttachmentIcon = true;
        } else {
          this.hideAttachmentIcon = false;
        }
      }
    });
  }
}
