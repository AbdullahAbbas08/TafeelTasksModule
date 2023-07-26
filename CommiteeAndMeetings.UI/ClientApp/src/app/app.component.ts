import { LayoutService } from './shared/_services/layout.service';
import { AuthTicketDTO, SwaggerClient } from './core/_services/swagger/SwaggerClient.service';
import {
  AfterViewChecked,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AuthService } from './auth/auth.service';
import { LocalizationService } from './shared/_services/localization.service';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { ThemeService } from './shared/_services/theme.service';
import { TasksService } from './tasks/tasks.service';
export interface BreadCrumb {
  label: string;
  url: string;
}
@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
})
export class AppComponent implements OnInit, OnDestroy, AfterViewChecked {
  userIsAuthenticated = false;
  spinnerSub: Subscription;
  currentUser: AuthTicketDTO;
  isloading;
  showHeader = false;
  checkComp:boolean;
  routeSubscription: Subscription;
  breadcrumbs$: BreadCrumb[];
  subscription: Subscription;
  showBreadcrumb: boolean;
  checkTaskModule:boolean = false
  constructor(
    private titleService: Title,
    private translateService: TranslateService,
    private authService: AuthService,
    private layoutService: LayoutService,
    private localizationService: LocalizationService,
    private changeDetector: ChangeDetectorRef,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public themeService: ThemeService,
    private swagger:SwaggerClient
  ) {
    this.authService
      .listenToAuthStatus()
      .subscribe((value) => {this.showHeader = value});
    this.onListenToSpinner();
    this.routeSubscription = this.router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        this.breadcrumbs$ = this.buildBreadCrumb(this.activatedRoute.root);
      }
    });
    //  const isAuth = this.authService.getAuthStatus();
    this.checkComponent()
  }

  ngOnInit() {
    this.localizationService.init();
    this.authService.autoAuthUser();

   

    setTimeout(() => {
      this.translateService
        .get('AppTitle')
        .subscribe((title) => this.titleService.setTitle(title)); //CommitteesAndMeetings
    }, 1000);
  }
  ngAfterViewChecked() {
    this.changeDetector.detectChanges();
  }

  ngOnDestroy() {
    this.spinnerSub.unsubscribe();
    this.subscription.unsubscribe();
  }
  checkComponent(){
    this.router.events.subscribe((env) =>{
      if(env instanceof NavigationEnd){
            if((env.url.includes('/tasks')) && !localStorage.getItem('token')){
              this.router.navigate(['/auth/login']);
            } else {
              let currentModule = localStorage.getItem('defaultModule');
              if(currentModule == '0'){
                if((env.url.includes('/committees') || env.url.includes('/meetings') || env.urlAfterRedirects.includes('/committees') || env.urlAfterRedirects.includes('/meetings'))){
                  this.router.navigate(['/tasks']);
                 }
              }
              if(!this.authService.isAuthUserHasPermissions(['DisplayEmployeeStatistics'])){
                if((env.url.includes('/taskStatistics') || env.urlAfterRedirects.includes('/taskStatistics'))){
                  this.router.navigate(['/tasks']);
                 }
              }
            }
      }
    })
  }
  onListenToSpinner() {
    this.layoutService.listenToLoading().subscribe((value) => {
      this.isloading = value;
    });
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
