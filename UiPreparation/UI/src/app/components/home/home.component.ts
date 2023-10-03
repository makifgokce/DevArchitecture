import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Post } from 'src/app/models/post';
import { environment } from 'src/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public posts!: Post[];
  private signalR!: HubConnection;
  public messages!: any[];
  constructor(private httpClient: HttpClient) {
    
    this.signalR = new HubConnectionBuilder()
    .withUrl(`https://localhost:5001/chatHub`)
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Debug)
    .build();
  }
  ngOnInit(): void{
    this.GetPosts()
    this.ConnectSignalR()
    this.signalR.on("Clients", message => {
      this.messages = message
    })
  }

  GetPosts():void{
    let headers = new HttpHeaders();
    headers = headers.append("Content-Type", "application/json")
    this.httpClient.get<Post[]>(environment.getApiUrl + "/Post", { headers: headers }).subscribe(data => {
      if(data != null){
        this.posts = data
      }
    });
  }

  async ConnectSignalR() : Promise<void>{
    try {
      await this.signalR.start().then(() => {
        this.signalR.invoke("SayHello", "Merhaba")
      })
      console.log("Connection success.")
    } catch (error) {
      console.log(error)
      setTimeout(this.ConnectSignalR, 5000)
    }
    
    
  }
}
