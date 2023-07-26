import { Component, EventEmitter, OnInit, Output,Input } from '@angular/core';
import { ActivatedRoute ,Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from 'src/app/auth/auth.service';
import { DestroyService } from 'src/app/shared/_services/destroy.service';
import { CommitteeService } from '../../committee.service';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss'],
  providers: [DestroyService],
})
export class SideMenuComponent implements OnInit {
  isCollapsed = false;
  routeId: string;
  permittedInboxTransactions = false;
  permittedOutboxTransactions = false;
  @Input() enableTransaction: Boolean;
  @Output('menuToglled')
  menuToglled: EventEmitter<boolean> = new EventEmitter<boolean>(false);
  userId: any;
  constructor(
    private route: ActivatedRoute,
    private desService: DestroyService,
    public committeeService: CommitteeService,
    private router: Router,
    private authService:AuthService
  ) {
  }
  isSelected(route: string): boolean {
    return route === this.router.url;
}
  ngOnInit(): void {
    this.userId = this.authService.getUser().userId;
    this.route.paramMap.subscribe((params) => {
      if (params) {
        this.routeId = params.get('id');
      }
    });
    this.checkPermissions();
  }

  toggleCollapsed(): void {
    this.isCollapsed = !this.isCollapsed;
    this.menuToglled.emit(this.isCollapsed);
  }
  checkPermissions(){
    if(this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers.some((el) => el.userId === +this.userId)){
      this.permittedInboxTransactions = this.committeeService.checkPermission('INBOXTRANSACTION');
      this.permittedOutboxTransactions = this.committeeService.checkPermission('OUTBOXTRANSACTION');
    } else {
        this.permittedInboxTransactions = true;
        this.permittedOutboxTransactions = true
      }
  }
  closeFilter(){
     this.committeeService.filterFlag.next(true)
  }

}
