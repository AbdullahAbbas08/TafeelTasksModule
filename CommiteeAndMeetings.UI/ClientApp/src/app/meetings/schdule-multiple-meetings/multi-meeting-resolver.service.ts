import { MultipleMeetingService } from './multiple-meeting.service';
import { Observable } from 'rxjs';
import { ReletedMeetingListDTO } from './../../core/_services/swagger/SwaggerClient.service';
import {
  ActivatedRouteSnapshot,
  Resolve,
  RouterStateSnapshot,
} from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable()
export class MultiMeetingResolverService
  implements Resolve<ReletedMeetingListDTO>
{
  constructor(private multiMeetingService: MultipleMeetingService) {}
  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<ReletedMeetingListDTO>
    | Promise<ReletedMeetingListDTO>
    | ReletedMeetingListDTO {
    return this.multiMeetingService.getMeetingsList(+route.params['refId']);
  }
}
