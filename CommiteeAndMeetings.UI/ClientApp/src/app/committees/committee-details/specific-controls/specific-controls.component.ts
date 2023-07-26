import { ThemeService } from './../../../shared/_services/theme.service';
import { Component, OnInit } from '@angular/core';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { CurrentStatusDTODataSourceResult } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { CommitteeActions } from 'src/app/shared/_enums/AppEnums';
import { CommitteeService } from '../../committee.service';
import { TranslateService } from '@ngx-translate/core';
import { SearchService } from 'src/app/shared/_services/search.service';
export interface FilterOptions {
  currentStatusId?: number;
  currentStatusNameAr?: string;
  currentStatusNameEn?: string;
}

@Component({
  selector: 'app-specific-controls',
  templateUrl: './specific-controls.component.html',
  styleUrls: ['./specific-controls.component.scss'],
})
export class SpecificControlsComponent implements OnInit {
  filterOptions: FilterOptions[] = [];
  searchword: string;
  closeFlag: boolean = false;
  take: number = 10;
  count: number = 0;
  constructor(
    public committeeService: CommitteeService,
    public translateService: TranslateService,
    private modalService: SharedModalService,
    private searchService: SearchService,
    public themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this.getStatus();
    this.searchThis();
  }

  showTreeView() {
    this.committeeService.treeView = !this.committeeService.treeView;
  }

  getCommitteesData(filterId?: number) {
    this.committeeService.committeeFilters.next(filterId);
  }

  addNewCommittee() {
    this.modalService.openDrawerModal(CommitteeActions.CreateNewCommittee);
  }

  searchThis() {
    this.searchService.search(this.searchword);
  }

  cancelSearch() {
    this.searchword = undefined;
    this.searchThis();
  }

  getStatus() {
    this.committeeService
      .getCommitteStatus(this.take, this.count)
      .subscribe((result: CurrentStatusDTODataSourceResult) => {
        this.filterOptions = result.data;
      });
  }

  refresh() {
    this.searchService.search(undefined);
  }
}
