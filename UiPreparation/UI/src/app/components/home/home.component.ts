import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Post } from 'src/app/models/post';
import { AuthService } from 'src/app/services/auth.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { environment } from 'src/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public posts!: Post[];
  public messages!: any;
  constructor(private httpClient: HttpClient, private signalR: SignalRService, private authService: AuthService) {

  }
  ngOnInit(): void{
    this.signalR.clients.subscribe((cl) => {
      this.messages = cl
      console.log(cl)
    })
    this.GetPosts()
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
  
  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }

  
}
