import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { LocalStorageService } from './local-storage.service';
import { Subject } from 'rxjs';
import { HubConnection } from '@microsoft/signalr';
import { environment } from 'src/environment';
@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private con!: HubConnection;
  public clients = new Subject();
  constructor(private lStorage: LocalStorageService) {
    this.Init()
  }

  Init(){
    this.con = new signalR.HubConnectionBuilder()
    .withUrl(`${environment.getHubsUrl}chatHub`,{
      accessTokenFactory:() => { return this.lStorage.getToken()}
    })
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Debug)
    .build();
    this.con.on("Clients", cl => {
      this.clients = cl
    })
    this.con.start()
  }

}
