import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StatisticsService } from '../statistics.service';
import { TranslateService } from '@ngx-translate/core';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
@Component({
  selector: 'app-stats-files',
  templateUrl: './stats-files.component.html',
  styleUrls: ['./stats-files.component.scss']
})
export class StatsFilesComponent implements OnInit {
  committeeId:string;
  chartSize = [80, 80];
  typeContainer:any[] =[]
  data=[]
  wordContainer=[];
  excelContainer=[];
  powerpointContainer=[];
  imgContainer=[];
  imageContainer=[];
  totalCount:number;
  wordFile:boolean = false;
  imagesFile:boolean = false;
  excelFile:boolean = false;
  powerPointFiles:boolean = false;
  powerContainer = [];
  spreadContainer=[];
  colorScheme1 = {
    domain: ['#FEE69D' , '#FEC30F']
  };
  colorScheme2 = {
    domain: ['#7087F5', '#38437A']
  };
  colorScheme3 = {
    domain:['#A80034','#F0004A']
  }
  colorScheme4 = {
    domain:['#7CF0D4','#69C9B2']
  }

   
  
  constructor(private router : ActivatedRoute,private _statistics:StatisticsService,public translateService: TranslateService,private browserService:BrowserStorageService) {
   }

  ngOnInit(): void {
    this.router.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
    this.getStatus()
  }

  getStatus(){
    this._statistics.getAttacemntsPerType(this.browserService.encryptCommitteId(this.committeeId)).subscribe((result) => {
      this.data = result;
      this.totalCount = result[0]?.totalCount
      var imgeCount = 0;
      var powerpointCount = 0;
      var excelCount = 0;
      for (let i = 0;i < result.length;i++){
        let mimeType = result[i].type;
        if(mimeType.includes('word')){
          this.wordFile = true
          this.wordContainer.push(
            { name:this.translateService.currentLang==='ar' ? 'عدد الملفات' : 'No Of Files' , value:result[i].count},
            {name:this.translateService.currentLang==='ar' ? 'كٌل الملفات' : 'Total Files', value:this.totalCount}
          )
        } else if(mimeType.includes('image')){
          this.imagesFile = true;
          this.imageContainer.push(result[i])
        } else if (mimeType.includes('spreadsheet')){
          this.excelFile = true;
          this.excelContainer.push(result[i])
        } else if (mimeType.includes('excel')){
          this.excelFile = true;
          this.excelContainer.push(result[i])
        }
        else if (mimeType.includes('present')){
          this.powerPointFiles = true;
          this.powerpointContainer.push(result[i])
        } else if (mimeType.includes('powerpoint')){
          this.powerPointFiles = true;
          this.powerpointContainer.push(result[i])
        }
      }
      for(let i=0; i<this.imageContainer.length;i++){
        imgeCount += this.imageContainer[i].count
      }
      for(let i=0;i<this.powerpointContainer.length;i++){
        powerpointCount += this.powerpointContainer[i].count
      }
      for(let i=0;i<this.excelContainer.length;i++){
        excelCount += this.excelContainer[i].count;
      }
    this.imgContainer = [
        {name:this.translateService.currentLang==='ar' ? 'عدد الصور' : 'No Of Photos',value:imgeCount},
        {name:this.translateService.currentLang==='ar' ? 'كٌل الملفات' : 'Total Files',value:this.totalCount}
    ]
    this.powerContainer = [
          {name:this.translateService.currentLang==='ar' ? 'عدد الملفات' : 'No Of Files', value:powerpointCount},
          {name:this.translateService.currentLang==='ar' ? 'كٌل الملفات' : 'Total Files', value:this.totalCount}
    ]
    this.spreadContainer = [
       {name:this.translateService.currentLang==='ar' ? 'عدد الملفات' : 'No Of Files', value:excelCount},
       {name:this.translateService.currentLang==='ar' ? 'كٌل الملفات' : 'Total Files', value:this.totalCount}
    ]
    })
  }

}
