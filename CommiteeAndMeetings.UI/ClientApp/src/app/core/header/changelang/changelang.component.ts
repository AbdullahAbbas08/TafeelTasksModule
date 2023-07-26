import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LocalizationService } from 'src/app/shared/_services/localization.service';
import { SharedModalService } from '../../_services/modal.service';

@Component({
  selector: 'app-changelang',
  templateUrl: './changelang.component.html',
  styleUrls: ['./changelang.component.scss']
})
export class ChangelangComponent implements OnInit {

  constructor(public translateService: TranslateService ,private modalService: SharedModalService,private localizationService: LocalizationService) { }

  ngOnInit(): void {
  }

  close(){
    this.modalService.destroyModal();
   }

   changeLang(){
    this.localizationService.changeLocal();
    this.close();
   }
}
