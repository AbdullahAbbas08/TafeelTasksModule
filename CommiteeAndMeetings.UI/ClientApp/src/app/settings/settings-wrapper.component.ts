import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';

export interface BreadCrumb {
  label: string;
  url: string;
}

@Component({
  selector: 'app-settings',
  template: `
    <div style="min-height: calc(100vh - 61px);">
    <router-outlet></router-outlet>
    </div>
  `,
})
export class SettingsWrapperComponent implements OnInit, OnDestroy {
  routeSubscription: Subscription;
  breadcrumbs$: BreadCrumb[];

  showBreadcrumb: boolean;

  constructor(private activatedRoute: ActivatedRoute, private router: Router) {
    // Build your breadcrumb starting with the root route of your current activated route
    this.routeSubscription = this.router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        this.breadcrumbs$ = this.buildBreadCrumb(this.activatedRoute.root);
      }
    });
  }

  ngOnInit() {}

  ngOnDestroy() {
    this.routeSubscription.unsubscribe();
  }

  buildBreadCrumb(
    route: ActivatedRoute,
    url: string = '',
    breadcrumbs: Array<BreadCrumb> = []
  ): Array<BreadCrumb> {
    // If no routeConfig is avalailable we are on the root path
    let label =
      route.routeConfig &&
      route.routeConfig.data &&
      route.routeConfig.data['breadcrumb']
        ? route.routeConfig.data['breadcrumb']
        : route.routeConfig
        ? route.routeConfig.path
        : 'Home';
    if (label === 'Settings') {
      this.showBreadcrumb = false;
    } else if (label !== '') {
      this.showBreadcrumb = true;
    }
    if (label.indexOf(' ') > -1) {
      let words = label.split(' ');
      words = words.map((x) => {
        return x.charAt(0).toUpperCase() + x.slice(1).toLowerCase();
      });
      label = words.join('');
    }
    if (label.indexOf(':') > -1) {
      route.params
        .subscribe(
          (params) =>
            (label = params[label.substr(label.indexOf(':') + 1, label.length)])
        )
        .unsubscribe();
    }
    const path = route.routeConfig ? route.routeConfig.path : '';
    // In the routeConfig the complete path is not available,
    // so we rebuild it each time
    const nextUrl = `${url}${path}/`;
    const breadcrumb = {
      label: label,
      url: nextUrl,
    };
    let newBreadcrumbs;
    if (path === '') {
      if (!breadcrumbs.length) {
        newBreadcrumbs = [...breadcrumbs, breadcrumb];
      } else {
        newBreadcrumbs = breadcrumbs;
      }
    } else {
      newBreadcrumbs = [...breadcrumbs, breadcrumb];
    }
    if (route.firstChild) {
      // If we are not on our current path yet,
      // there will be more children to look after, to build our breadcumb
      return this.buildBreadCrumb(route.firstChild, nextUrl, newBreadcrumbs);
    }
    return newBreadcrumbs;
  }
}
