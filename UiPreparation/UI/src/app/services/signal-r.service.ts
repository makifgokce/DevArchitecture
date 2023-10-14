import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { LocalStorageService } from './local-storage.service';
import { Subject } from 'rxjs';
import { HubConnection } from '@microsoft/signalr';
@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private con!: HubConnection;
  public clients = new Subject();
  constructor(private lStorage: LocalStorageService) {
    this.Init()
    this.con.on("Clients", clients => {
      this.clients.next(clients)
    })
  }

  Init(){
    this.con = new signalR.HubConnectionBuilder()
    .withUrl(`https://localhost:5001/hubs/chatHub`,{
      accessTokenFactory:() => { return this.lStorage.getToken()}
    })
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Debug)
    .build();

    this.con.start()
  }
}
