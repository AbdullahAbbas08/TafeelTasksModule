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
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthguardService implements CanActivate, CanActivateChild {
  ssokey: string;
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    this.authService.autoAuthUser();
    const isAuthenticated = this.authService.getAuthStatus();

    if (!isAuthenticated) {
      this.router.navigate(['/auth/login']);
    }

    return isAuthenticated;
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    this.authService.autoAuthUser();
    const isAuthenticated = this.authService.getAuthStatus();

    if (!isAuthenticated) {
      this.router.navigate(['/auth/login']);
    }

    return isAuthenticated;
  }
}
