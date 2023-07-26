import { BehaviorSubject } from 'rxjs';
import { API_BASE_URL } from './../../core/_services/swagger/SwaggerClient.service';
import { AuthService } from './../../auth/auth.service';
import { Injectable, Inject } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

class SignalRMethod {
  methodName: string;
  method: (...args: any[]) => void;
}

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: HubConnection | undefined;
  private methods: SignalRMethod[] = [];
  public onConnected: BehaviorSubject<boolean> = new BehaviorSubject(null);

  constructor(
    private authService: AuthService,
    @Inject(API_BASE_URL) private baseUrl: string
  ) {}

  public initialize(): void {
    this.stopConnection();

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/api/SignalR`, {
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => this.authService.getToken(),
      })
      .configureLogging(signalR.LogLevel.None)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.onConnected.next(true);
      })
      .catch((error: any) => {
        setTimeout(() => this.initialize(), 3000);
      });
  }

  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
      this.hubConnection = null;
    }
  }

  checkIfClosed() {
    this.hubConnection.onclose(() => {
      setTimeout(() => {
        this.initialize();
      }, 3000);
    });
  }

  on(methodName: string, method: (...args: any[]) => void) {
    this.methods = this.methods.filter(
      (item) => item.methodName !== methodName
    );
    this.methods.push({ methodName: methodName, method: method });
    if (this.hubConnection) {
      this.hubConnection.on(methodName, method);
    }
  }
}
