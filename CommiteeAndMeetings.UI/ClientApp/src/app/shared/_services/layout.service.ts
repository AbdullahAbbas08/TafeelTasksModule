import { Subject, Subscription } from 'rxjs';
import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { BrowserStorageService } from './browser-storage.service';

@Injectable({
  providedIn: 'root',
})
export class LayoutService {
  private renderer: Renderer2;
  private spinnerSub = new Subject<boolean>();

  constructor(
    private browserStorageService: BrowserStorageService,
    private rendererFactory: RendererFactory2
  ) {
    this.renderer = rendererFactory.createRenderer(null, null);
  }

  listenToLoading() {
    return this.spinnerSub.asObservable();
  }

  setTheme(themeName: string) {
    const body = document.body;
    const className = body.className;
    body.className = className
      .split(' ')
      .filter((x) => !x.includes('-theme'))
      .join(' ');
    body.classList.add(themeName);
    this.browserStorageService.setLocal('theme', themeName);
  }

  toggleIsLoading(loading: boolean) {
    if (loading) {
      if (
        !document
          .getElementsByTagName('body')[0]
          .classList.contains('isLoading')
      )
        this.renderer.addClass(document.body, 'isLoading');
      else this.renderer.removeClass(document.body, 'isLoading');
    } else {
      this.renderer.removeClass(document.body, 'isLoading');
    }
  }

  toggleIsLoadingBlockUI(loading: boolean) {
    if (loading) {
      this.renderer.addClass(document.body, 'isLoading-blockUI');
    } else {
      this.renderer.removeClass(document.body, 'isLoading-blockUI');
    }
  }

  toggleSpinner(isLoading: boolean) {
    this.spinnerSub.next(isLoading);
  }
}
