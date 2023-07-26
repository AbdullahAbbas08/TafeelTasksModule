/// <reference types="dwt" />
/// <reference types="dwt/addon.pdf" />

import { ScannerService } from './../../_services/scanner.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

export interface SelectedArea {
  left: number;
  top: number;
  right: number;
  bottom: number;
}
export interface TwainSource {
  idx: number;
  name: string;
}
@Component({
  selector: 'app-scanner',
  templateUrl: './scanner.component.html',
  styleUrls: ['./scanner.component.scss'],
})
export class ScannerComponent implements OnInit {
  @Output() fileScanned: EventEmitter<File> = new EventEmitter();

  DWObject: WebTwain = null;
  selectSources: HTMLSelectElement;
  fileName = '';
  containerId = 'dwtcontrolContainer';
  bWASM = false;
  resolutions = ['100', '150', '200', '300'];
  selectedTwainSourceIdx: number = null;
  selectedArea: SelectedArea = { left: 0, top: 0, right: 0, bottom: 0 };
  twainSources: TwainSource[] = [];
  constructor(private scannerService: ScannerService) {}

  ngOnInit(): void {
    this.initScan();
    this.scannerService.getDefaultScanName().subscribe((data) => {
      this.fileName = data.systemSettingValue;
    });
  }

  initScan() {
    Dynamsoft.WebTwainEnv.Containers = [
      {
        ContainerId: this.containerId,
        Width: '153px',
        Height: '200px',
      },
    ];

    Dynamsoft.WebTwainEnv.Load();

    Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', () => {
      this.Dynamsoft_OnReady();
    });

    Dynamsoft.WebTwainEnv.ProductKey =
      'f0068WQAAAJBKMX24ciX9rMVkoVXmpVQYP6hvf6Yt40n5keLu2JGgWQseNJP5445gGeNYhMtAu2W2zdtjZ9E9chObrZOTP48=';

    Dynamsoft.WebTwainEnv.Trial = false;
  }

  Dynamsoft_OnReady(): void {
    this.DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');
    const twainSources = localStorage.getItem("twainSources");
    if (twainSources) {
      this.twainSources = JSON.parse(twainSources);
    } else {
      this.refreshTwainSources();
    }
    if (this.twainSources.length > 0) {
      const selectedTwainSourceIdx = localStorage.getItem(
        "selectedTwainSourceIdx"
      );
      if (selectedTwainSourceIdx) {
        this.selectedTwainSourceIdx = parseInt(selectedTwainSourceIdx, 10);
      } else {
        this.selectedTwainSourceIdx = 0;
      }
    }
  }
  refreshTwainSources() {
    this.twainSources = [];
    for (let i = 0; i < this.DWObject.SourceCount; i++) {
      this.twainSources.push({
        idx: i,
        name: this.DWObject.GetSourceNameItems(i),
      });
    }
    localStorage.setItem("twainSources", JSON.stringify(this.twainSources));
  }
  selectedTwainSourceChanged(newValue): void {
    if (newValue) {
      localStorage.setItem("selectedTwainSourceIdx", newValue);
    } else {
      localStorage.removeItem("selectedTwainSourceIdx");
    }
  }
  acquireImage(): void {
    if (this.DWObject == null)
      this.DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');
    this.DWObject.IfDisableSourceAfterAcquire = true;

    this.DWObject.RemoveAllImages();

    if (
      this.DWObject.SourceCount > 0 &&
      this.DWObject.SelectSourceByIndex(this.selectedTwainSourceIdx)
    ) {
      const onAcquireImageSuccess = () => {
        this.DWObject.CloseSource();

        const count = this.DWObject.HowManyImagesInBuffer;

        this.emitScannedFile(Array.from(Array(count).keys()), 2);
      };

      const onAcquireImageFailure = onAcquireImageSuccess;
      this.DWObject.OpenSource();
      this.DWObject.AcquireImage(
        {},
        onAcquireImageSuccess,
        onAcquireImageFailure
      );
    } else {
      alert('No Source Available!');
    }
  }

  emitScannedFile(indices, type) {
    this.DWObject.ConvertToBlob(
      indices,
      type,
      (result) => {
        let fileName = this.fileName + this.getExtension(type);

        const file: File = new File([result], fileName, { type: 'image/tiff' });
        this.fileScanned.emit(file);
        this.fileName = '';
      },
      (errorCode, errorString) => {
      }
    );
  }

  getExtension(type) {
    switch (type) {
      case 0:
        return '.bmp';
      case 1:
        return '.jpg';
      case 2:
        return '.tif';
      case 3:
        return '.png';
      case 4:
        return '.pdf';
      case 7:
      case 8:
      default:
        return '.unknown';
    }
  }
}
