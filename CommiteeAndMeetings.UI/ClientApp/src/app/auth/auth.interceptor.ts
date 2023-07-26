import { SharedModalService } from 'src/app/core/_services/modal.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, of, pipe, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { delay, mergeMap, retryWhen, take,finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/internal/operators/catchError';
import { LayoutService } from '../shared/_services/layout.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private delayBetweenRetriesMs = 1000;
  private numberOfRetries = 3;
  constructor(
    private authService: AuthService,
    private router: Router,
    private layoutService: LayoutService,
    private modal: SharedModalService
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const token = this.authService.getToken();
   if(token){
    const authRequest = request.clone({
      headers: request.headers.set('Authorization', 'Bearer ' + token)
    });
    return next.handle(authRequest).pipe(
      retryWhen((errors) =>
        errors.pipe(
          mergeMap((error: HttpErrorResponse, retryAttempt: number) => {
            // if in Case UnAuthorize
            if (error.status === 401 || error.status === 403) {
              return this.router.navigate(['/auth/login']);
            }
            if (retryAttempt === this.numberOfRetries - 1) {
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
        )
      ),
      catchError((error: any, caught: Observable<HttpEvent<any>>) => {
        if (
          error.status === 400 ||
          error.status === 401 ||
          error.status === 403 ||
          error.status === 500
        ) {
          this.layoutService.toggleSpinner(false);
           this.modal.createMessage('error', 'LoginError');

          if (error.status === 401) {
            this.router.navigate(['/auth/login']);
          }
        }
        return throwError(error);
      })
    );
   } else {
    this.layoutService.toggleIsLoading(false);
    return next.handle(request).pipe(finalize(() => {
      this.layoutService.toggleIsLoading(false);
    }));
   }

  
  }
}
