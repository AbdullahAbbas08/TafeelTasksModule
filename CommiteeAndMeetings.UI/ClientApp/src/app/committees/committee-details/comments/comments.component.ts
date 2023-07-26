import { Component, Input, OnInit } from '@angular/core';
import { SavedAttachmentDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss'],
})
export class CommentsComponent implements OnInit {
  @Input() comments: any;
  @Input() count;
  @Input() checkComponent:boolean
  showPrevious = false;

  constructor() {}

  ngOnInit(): void {
  }

  toggleDisplay(e: boolean): void {
    this.showPrevious = !this.showPrevious;
  }
}
