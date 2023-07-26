import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { CommitteeService } from 'src/app/committees/committee.service';
import { CommiteeDTO,LookUpDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { ExtentedCommiteeMemberDTO, UserService, } from '../../users/user.service';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.scss']
})
export class MembersComponent implements OnInit {
  AllUsers:ExtentedCommiteeMemberDTO[] = [];
  committeDetails: CommiteeDTO;
  committeeId: string;
  delegatedUser:LookUpDTO[]=[]
  skip: number = 0;
  take: number = 9;
  filters: any[] = [];
  filter_Logic: string = 'or';
  count: number;
  headUnit:any;
  constructor(private _users:UserService,private route : ActivatedRoute,public translate: TranslateService,private committeeService: CommitteeService,private browserService: BrowserStorageService) {
   }

  ngOnInit(): void {
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
    this.getAllMembers()
    this.getDelegatedUsers();
    this.headUnit = this.committeeService.CommitteHeadUnit
  }
  getAllMembers(){
    this._users.getCommitteUsers(this.take,this.skip,this.committeeId,this.filters,this.filter_Logic,undefined).subscribe((result) => {
      this.AllUsers = result.data
      this.count = result.count
    })
  }
  getDelegatedUsers(){
    this._users.getAlldelegatedUsers(this.browserService.encryptCommitteId(this.committeeId)).subscribe((res)=> {
      this.delegatedUser = res
    })
  }
}
