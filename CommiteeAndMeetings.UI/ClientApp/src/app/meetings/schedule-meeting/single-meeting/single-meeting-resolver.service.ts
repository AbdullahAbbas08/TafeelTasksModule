import { SingleMeetingService } from './../single-meeting.service';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  RouterStateSnapshot,
} from '@angular/router';
import { MeetingDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';

@Injectable({
  providedIn: 'root',
})
export class SingleMeetingResolverService implements Resolve<any> {
  constructor(private singleMeetingService: SingleMeetingService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const meeting: MeetingDTO = this.singleMeetingService.meeting;

    if (!meeting) {
      return this.singleMeetingService.getMeetingDetails(
        +route.paramMap.get('id')
      );
    } else {
      return meeting;
    }
  }
}
