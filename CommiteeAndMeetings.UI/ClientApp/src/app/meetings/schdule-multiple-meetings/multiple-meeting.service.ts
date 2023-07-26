import { UserDetailsDTO } from './../../core/_services/swagger/SwaggerClient.service';
import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class MultipleMeetingService {
  user: UserDetailsDTO;

  constructor(private swagger: SwaggerClient, private router: Router) {}

  getMeetingsList(refId: number) {
    return this.swagger
      .apiMeetingsGetRelatedMeetingsByReferenceNumberPost(refId)
      .pipe(
        tap((res) => {
          if (res && res?.createdByUser) {
            this.userDetails = res.createdByUser;
          } else {
            this.router.navigateByUrl('/meetings');
          }
        })
      );
  }

  get userDetails(): UserDetailsDTO {
    return this.user;
  }

  set userDetails(user: UserDetailsDTO) {
    this.user = user;
  }
}
