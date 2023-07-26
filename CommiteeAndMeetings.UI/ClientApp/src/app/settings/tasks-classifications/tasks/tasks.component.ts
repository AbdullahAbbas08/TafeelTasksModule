import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { takeUntil } from 'rxjs/operators';
import {
  ComiteeTaskCategoryDTO,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { DestroyService } from 'src/app/shared/_services/destroy.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  providers: [DestroyService],
})
export class TasksComponent implements OnInit {
  category: ComiteeTaskCategoryDTO = new ComiteeTaskCategoryDTO();
  categoryCreate = false;
  categoryId: any;

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
        if (!r.comiteeTaskCategoryId) {
          this.categoryCreate = true;
          this.category = new ComiteeTaskCategoryDTO();
        } else {
          this.categoryId = r.comiteeTaskCategoryId;
          this.getCategoryById();
        }
      });
  }

  getCategoryById() {
    this.swagger
      .apiComiteeTaskCategoryGetByIdGet(this.categoryId)
      .subscribe((category) => (this.category = category));
  }

  editCategory() {
    if (!this.category.categoryNameEn || !this.category.categoryNameAr) {
      return;
    }
    this.swagger.apiComiteeTaskCategoryUpdatePut([this.category]).subscribe((value) => {
      if (value) {
        this.translate
          .get('CategoryUpdated')
          .subscribe((translateValue) =>
            this.notificationService.success(translateValue, '')
          );
        this.router.navigate(['/settings/task-classifications']);
      } else {
        this.translate
          .get('CategoryUpdatedError')
          .subscribe((translateValue) =>
            this.notificationService.error(translateValue, '')
          );
      }
    });
  }

  insertCategory() {
    if (!this.category.categoryNameEn || !this.category.categoryNameAr) {
      return;
    }
    this.swagger
      .apiComiteeTaskCategoryInsertPost([this.category])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('CategoryCreated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/task-classifications']);
        } else {
          this.translate
            .get('CategoryCreatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }
}
