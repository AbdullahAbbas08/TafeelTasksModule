import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { InjectionToken, NgModule } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { SharedModule } from '../shared/shared.module';
import { EnvServiceFactory } from '../shared/_services/env.service.provider';
import { HeaderComponent } from './header/header.component';
import { InterceptorService } from './_services/interceptor.service';
import {
  API_BASE_URL,
  SwaggerClient,
} from './_services/swagger/SwaggerClient.service';
import { ChangelangComponent } from './header/changelang/changelang.component';
import { NotificationsComponent } from './header/notifications/notifications.component';

export const DEV_MODE = new InjectionToken<boolean>('DEV_MODE');

@NgModule({
  declarations: [HeaderComponent, ChangelangComponent, NotificationsComponent],
  imports: [SharedModule],
  exports: [HeaderComponent],
  providers: [
    SwaggerClient,
    InterceptorService,
    {
      provide: DEV_MODE,
      useValue: !environment.production,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true,
    },
    {
      provide: API_BASE_URL,
      useValue: EnvServiceFactory().apiUrl,
    },
  ],
})
export class CoreModule {}
