import {Directive, Input, OnDestroy, OnInit, TemplateRef, ViewContainerRef} from '@angular/core';
import { distinctUntilChanged } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Directive({
  selector: '[appHasAuthUserViewPermission]'
})
export class HasAuthUserViewPermissionDirective implements OnInit, OnDestroy {
  private isVisible = false;
  private requiredPermissions: string[] | null = null;
  private subscription: Subscription | null = null;
  private subscriptionTicket: Subscription | null = null;

  @Input()
  set appHasAuthUserViewPermission(obj: any | null) {
    if (Array.isArray(obj)) {
      this.requiredPermissions = obj;
      // handle enabling the permission with flag
    } else if (obj && obj['enable']) {

    } else this.requiredPermissions = [];
    // handle or options with permissions
    if (obj && obj['orOptions']) {

    }
  }

  // Note, if you don't place the * in front, you won't be able to inject the TemplateRef<any> or ViewContainerRef into your directive.
  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private auth: AuthService
  ) {
  }

  ngOnInit() {
    this.subscriptionTicket = this.auth.currentUserSub().pipe(distinctUntilChanged()).subscribe(() => this.changeVisibility());
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    if (this.subscriptionTicket) {
      this.subscriptionTicket.unsubscribe();
    }
  }

  private changeVisibility() {
    const isHasPermissions = !this.requiredPermissions ? true : this.auth.isAuthUserHasPermissions(this.requiredPermissions);
    if (isHasPermissions) {
      if (!this.isVisible) {
        this.viewContainer.createEmbeddedView(this.templateRef);
        this.isVisible = true;
      }
    } else {
      this.isVisible = false;
      this.viewContainer.clear();
    }
  }
}
