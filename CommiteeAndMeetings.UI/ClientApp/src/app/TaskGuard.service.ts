import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth/auth.service';


@Injectable({
  providedIn: 'root',
})
export class TaskguardService implements CanActivate, CanActivateChild {
  constructor(private authService:AuthService,private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {

    const isAuthenticated = this.authService.checkModule();
    if(isAuthenticated){
       return this.router.parseUrl('/tasks')
    }
    return true
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
        const isAuthenticated = this.authService.checkModule();
    if(isAuthenticated){
      return this.router.parseUrl('/tasks')
    }
    return true
  }
}
