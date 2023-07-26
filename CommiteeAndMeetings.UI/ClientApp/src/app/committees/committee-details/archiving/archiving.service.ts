import { CommitteeService } from './../../committee.service';
import { Injectable } from '@angular/core';
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';

@Injectable({ providedIn: 'root' })
export class ValididyResolverService implements Resolve<any> {
  constructor(private committeeService: CommitteeService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const validityPeriod = this.committeeService.getValidityPeriod();

    if (!validityPeriod) {
      return this.committeeService.getCommitteeDetails(
        route.parent.paramMap.get('id')
      );
    } else {
      return validityPeriod;
    }
  }
}
