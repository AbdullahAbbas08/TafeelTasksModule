import { Component, OnInit,Input } from '@angular/core';
import { CommiteeTaskDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';

@Component({
  selector: 'app-print-task',
  templateUrl: './print-task.component.html',
  styleUrls: ['./print-task.component.scss'],
})
export class PrintTaskComponent implements OnInit {
  @Input('tasksSummary') tasksSummary:CommiteeTaskDTO[];
  constructor(public translateService: TranslateService,
    public dateService:NgbDateService
  ) {}

  ngOnInit(): void {
  }
}
