import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalizeRouterService } from '@gilsdav/ngx-translate-router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
  account!: string;
  name!: string;
  surname!: string;
  constructor(private auth: AuthService, private http: HttpClient, private router: Router, private localizeService: LocalizeRouterService) { }

  ngOnInit(): void {
    this.account = this.auth.getUserName()
    this.name = this.auth.getName()
    this.surname = this.auth.getSurname()
  }
  navigate(str:string){
    this.router.navigateByUrl(this.localizeService.translateRoute(str).toString())
  }
}
