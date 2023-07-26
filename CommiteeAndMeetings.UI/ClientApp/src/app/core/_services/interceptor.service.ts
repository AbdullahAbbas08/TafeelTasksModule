import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, delay, finalize, mergeMap, retryWhen, take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';

@Injectable()
export class InterceptorService implements HttpInterceptor {
  private delayBetweenRetriesMs = 1000;
  private numberOfRetries = 3;
  private authorizationHeader = 'Authorization';
  private requests: HttpRequest<any>[] = [];
  userData: any;

  constructor(
    private authService: AuthService,
    private router: Router,
    // private layoutService: LayoutService
  ) {
    // this.authService.currentUser.subscribe((user: any) => {
    //   this.userData = user;
    // });
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // this.layoutService.toggleIsLoading(true);
    const accessToken = this.userData ? this.userData.token : '';
    request = request.clone({ withCredentials: false });
    if (accessToken) {
      request = request.clone({
        headers: request.headers.set(this.authorizationHeader, `Bearer ${accessToken}`).set('Cache-Control', 'no-cache')
          .set('Pragma', 'no-cache')
      });
      this.requests.push(request);
      return next.handle(request).pipe(
        retryWhen(errors => errors.pipe(
          mergeMap((error: HttpErrorResponse, retryAttempt: number) => {
            // if in Case UnAuthorize
            if (error.status === 401 || error.status === 403) {
              return this.router.navigate(['/auth/login']);
            }
            if (retryAttempt === this.numberOfRetries - 1) {
              // console.log(
              //   `%cHTTP call '${request.method}' ${request.url} failed after ${this.numberOfRetries} retries.`,
              //   `background: #a55656; color: #ffeded; font-weight: bold; padding: 2px 5px;`
              // );
              return throwError(error); // no retry
            }

            switch (error.status) {
              case 400:
              case 404:
              case 500:
                return throwError(error); // no retry
            }

            return of(error); // retry
          }),
          delay(this.delayBetweenRetriesMs),
          take(this.numberOfRetries)
        )),
        catchError((error: any, caught: Observable<HttpEvent<any>>) => {
          // console.error({ error, caught });
          if (error.status === 400 || error.status === 401 || error.status === 403) {
            this.router.navigate(['/auth/login']);
          }
          return throwError(error);
        }),
        finalize(() => {
          this.requests = this.requests.filter(x => x !== request);
          if (!this.requests.length) {
            // this.layoutService.toggleIsLoading(false);
          }
        })
      );

    } else {
      // login page
      return next.handle(request).pipe(finalize(() => {
        // this.layoutService.toggleIsLoading(false);
      }));
    }
  }
}
