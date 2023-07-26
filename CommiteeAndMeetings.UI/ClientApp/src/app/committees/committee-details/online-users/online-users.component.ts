import { Component, OnInit } from '@angular/core';
import { ExtentedCommiteeMemberDTO, UserService } from 'src/app/committees/committee-details/users/user.service';
import { ActivatedRoute} from '@angular/router';
import { StoreService } from 'src/app/shared/_services/store.service';
import { TranslateService } from '@ngx-translate/core';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
@Component({
  selector: 'app-online-users',
  templateUrl: './online-users.component.html',
  styleUrls: ['./online-users.component.scss']
})
export class OnlineUsersComponent implements OnInit {
  onlineUsers:ExtentedCommiteeMemberDTO[] = [];
  onlineUsersNumber:ExtentedCommiteeMemberDTO[]=[]
  currentCount = 0;
  count = 0;
  take: number = 10;
  skip: number = 0;
  loadingData: boolean = false;
  filters: any[] = [];
  committeeId: string;
  filter_Logic: string = 'or';
  memberID:number;
  constructor(private UserService:UserService,private route: ActivatedRoute ,private storeService: StoreService,public translateService: TranslateService,private layoutService: LayoutService,private BrowserService:BrowserStorageService ) {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) this.committeeId = id;
    });
   }
  

  ngOnInit(): void {
    this.storeService.refreshUsers$.subscribe((val) => {
      if (val) {
        this.onlineUsers.unshift(val);
      }
    });
  this.UserService.userMemberId.subscribe(x=>{
    if(x){
      this.getAllUsers();
    }
  });
  this.getAllUsers();
  }

  onScroll() {
    if(this.count > this.onlineUsers.length){
      this.skip += this.take;
      this.getAllUsers(true)
    }
  }
  getAllUsers(scroll: boolean = false){
    if (!scroll) {
      this.loadingData = true;
      this.layoutService.toggleSpinner(true);
      this.skip = 0;
    }
    this.UserService.getCommitteUsers(this.take,this.skip,this.committeeId,this.filters,this.filter_Logic,undefined).subscribe((result)=> {
     if(result && result.data){
       this.onlineUsers = scroll ? [...this.onlineUsers,...result.data] : result.data;
       this.count = result.count
     }
     this.onlineUsersNumber = result.data.filter((user)=> {
      return user.active == true
     })
     this.layoutService.toggleSpinner(false);
     this.loadingData = false;
    })
  }
  }

