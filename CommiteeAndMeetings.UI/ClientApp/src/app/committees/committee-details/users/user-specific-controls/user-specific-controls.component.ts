import { CommitteeService } from './../../../committee.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { CommitteeActions } from 'src/app/shared/_enums/AppEnums';
import { SearchService } from 'src/app/shared/_services/search.service';
@Component({
  selector: 'app-user-specific-controls',
  templateUrl: './user-specific-controls.component.html',
  styleUrls: ['./user-specific-controls.component.scss'],
})
export class UserSpecificControlsComponent implements OnInit {
  committeeId: string;
  searchword: string;
  permittedToAddMember = false;
  userId: number;
  constructor(
    private modalService: SharedModalService,
    private searchService: SearchService,
    private route: ActivatedRoute,
    private committeeService: CommitteeService,
    private authService: AuthService,
  ) {
  }

  ngOnInit(): void {
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
    this.userId = this.authService.getUser().userId;
    this.searchThis();
    // this.checkPermissions();
    this.checkIfUserExistInCommitte()
  }
  addNewUser() {
    this.modalService.openDrawerModal(
      CommitteeActions.CreateNewUser,
      this.committeeId
    );
  }
  searchThis() {
    this.searchService.search(this.searchword);
  }
  cancelSearch() {
    this.searchword = undefined;
    this.searchThis();
  }

  checkPermissions() {
    this.permittedToAddMember = this.committeeService.checkPermission(
      'ADDMEMBER'
    );
  }
  checkIfUserExistInCommitte(){
     if(!this.committeeService.committeMembers.some((el) => el.userId === +this.userId)){
      if(this.authService.isAuthUserHasPermissions(['ForAllCommitte'])){
        this.permittedToAddMember = true
      }
     } else {
       this.checkPermissions()
     }
  }
}
