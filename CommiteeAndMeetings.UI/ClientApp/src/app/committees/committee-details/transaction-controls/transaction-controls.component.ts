import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { CommitteeActions } from 'src/app/shared/_enums/AppEnums';
import { SearchService } from 'src/app/shared/_services/search.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { CommitteeService } from '../../committee.service';

@Component({
  selector: 'app-transaction-controls',
  templateUrl: './transaction-controls.component.html',
  styleUrls: ['./transaction-controls.component.scss'],
})
export class TransactionControlsComponent implements OnInit {
  actionTypes = CommitteeActions;
  committeeId: number;
  permittedToAddTransaction = false;

  searchword: string;
  closeFlag: boolean = false;
  constructor(
    private storeService: StoreService,
    private route: ActivatedRoute,
    private modalService: SharedModalService,
    private searchService: SearchService,
    private committeeService: CommitteeService
  ) {
    this.route.params.subscribe((params) => {
      this.committeeId = params.id;
      this.storeService.setCommitteeId(this.committeeId);
    });
  }
  ngOnInit(): void {
    this.checkPermissions();
    this.searchThis();
  }

  addNewTimeLineItem(type: number) {
    this.modalService.openDrawerModal(type, this.committeeId);
  }
  searchThis() {
    this.searchService.search(this.searchword);
  }
  cancelSearch() {
    this.searchword = undefined;
    this.searchThis();
  }

  checkPermissions() {
    this.permittedToAddTransaction = this.committeeService.checkPermission(
      'ADDTRANSACTIO'
    );
  }
  refresh() {
    this.searchService.search(undefined);
  }

}
