import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { LocalStorageService } from './local-storage.service';
import { Observable} from 'rxjs';
import { HubConnection } from '@microsoft/signalr';
import { environment } from 'src/environment';
import { ChatUser } from '../models/chat-user';
@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private con!: HubConnection;
  private clients = Observable<ChatUser[]>;
  constructor(private lStorage: LocalStorageService) {
    this.Init()
  }

  Init(){
    this.con = new signalR.HubConnectionBuilder()
    .withUrl(`${environment.getHubsUrl}chatHub`,{
      transport: signalR.HttpTransportType.LongPolling,
      accessTokenFactory:() => { return this.lStorage.getToken()}
    })
    .withAutomaticReconnect()
    .build();

    this.con.on("Clients", cl => {
      this.clients = cl
    })
    this.con.on("ReceiveMessage", msg => {
      console.log("ReceiveMessage", msg)
    })
    this.con.start()
  }

  get Clients(){
    return this.clients
  }

  PrivateMessage(account: string, message: string){
    this.con.send("PrivateMessage", account, message)
  }
}
