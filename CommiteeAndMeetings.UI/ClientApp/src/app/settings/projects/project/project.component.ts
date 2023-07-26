import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { takeUntil } from 'rxjs/operators';
import { DestroyService } from 'src/app/shared/_services/destroy.service';
import {
  ProjectDTO,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  providers: [DestroyService]
})
export class ProjectComponent implements OnInit {
  project: ProjectDTO = new ProjectDTO();
  projectCreate = false;
  projectId: any;

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
        if (!r.projectId) {
          this.projectCreate = true;
          this.project = new ProjectDTO();
        } else {
          this.projectId = r.projectId;
          this.getProjectById();
        }
      });
  }

  getProjectById() {
    this.swagger
      .apiProjectsGetByIdGet(this.projectId)
      .subscribe((project) => (this.project = project));
  }

  editProject() {
    if (!this.project.projectNameEn || !this.project.projectNameAr) {
      return;
    }
    this.swagger.apiProjectsUpdatePut([this.project]).subscribe((value) => {
      if (value) {
        this.translate
          .get('ProjectUpdated')
          .subscribe((translateValue) =>
            this.notificationService.success(translateValue, '')
          );
        this.router.navigate(['/settings/projects']);
      } else {
        this.translate
          .get('ProjectUpdatedError')
          .subscribe((translateValue) =>
            this.notificationService.error(translateValue, '')
          );
      }
    });
  }

  insertProject() {
    if (!this.project.projectNameEn || !this.project.projectNameAr) {
      return;
    }
    this.swagger
      .apiProjectsInsertPost([this.project])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('ProjectCreated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/projects']);
        } else {
          this.translate
            .get('ProjectCreatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }
}
