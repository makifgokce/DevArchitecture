import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { LocalizeRouterService } from '@gilsdav/ngx-translate-router';
import { Router } from '@angular/router';
import { LoginUser } from '../models/login-user';
import { Token } from '../models/token';
import { RegisterUser } from '../models/register-user';
import { AlertifyService } from './alertify.service';
import { SharedService } from './shared.service';
import { environment } from 'src/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  user: User = new User();
  isLoggin!: boolean;
  decodedToken: any;
  userToken!: string;
  jwtHelper: JwtHelperService = new JwtHelperService();
  claims!: string[];
  constructor(private httpClient: HttpClient, private storageService: LocalStorageService,
    private router: Router, private localize: LocalizeRouterService, private alertifyService: AlertifyService, private sharedService: SharedService) {
      this.setClaims();
  }
  login(loginUser: LoginUser) {
    let headers = new HttpHeaders();
    headers = headers.append("Content-Type", "application/json")
    this.httpClient.post<Token>(environment.getApiUrl + "/Auth/Login", loginUser, { headers: headers }).subscribe(data => {

      if (data.success) {
        this.storageService.setToken(data.data.token);
        this.storageService.setItem("refreshToken", data.data.refreshToken)
        this.claims = data.data.claims;


        var decode = this.jwtHelper.decodeToken(this.storageService.getToken());


        var propUserName = Object.keys(decode).filter(x => x.endsWith("/name"))[0];
        this.user.account = decode[propUserName];
        this.sharedService.sendChangeUserNameEvent();
        var propFirstName = Object.keys(decode).filter(x => x == "Name")[0];
        this.user.name = decode[propFirstName];
        var propLastName = Object.keys(decode).filter(x => x == "Surname")[0];
        this.user.surname = decode[propLastName];
        this.alertifyService.success(data.message);
        this.router.navigateByUrl(this.localize.translateRoute('/').toString());
      }
      else {
        this.alertifyService.warning(data.message);
      }

    }
    );
  }

  register(loginUser: RegisterUser) {
    let headers = new HttpHeaders();
    headers = headers.append("Content-Type", "application/json")
    this.httpClient.post<Token>(environment.getApiUrl + "/Auth/Register", loginUser, { headers: headers }).subscribe(data => {

      if (data.success) {
        this.alertifyService.success(data.message);
        this.router.navigateByUrl(this.localize.translateRoute('/login').toString());
      }
      else {
        this.alertifyService.warning(data.message);
      }

    }
    );
  }

  getUser(): User {
    return this.user;
  }

  setClaims(){
    if ((this.claims == undefined || this.claims.length == 0) && this.storageService.getToken() != null && this.loggedIn()) {

      this.httpClient.get<string[]>(environment.getApiUrl + "/operation-claims/cache").subscribe(data => {
        this.claims = data;
      })


      var token = this.storageService.getToken();
      var decode = this.jwtHelper.decodeToken(token);

      var propUserName = Object.keys(decode).filter(x => x.endsWith("/name"))[0];
      this.user.account = decode[propUserName];
      var propFirstName = Object.keys(decode).filter(x => x == "Name")[0];
      this.user.name = decode[propFirstName];
      var propLastName = Object.keys(decode).filter(x => x == "Surname")[0];
      this.user.surname = decode[propLastName];
    }
  }
  logOut() {
    this.storageService.removeToken();
    this.storageService.removeItem("lang")
    this.storageService.removeItem("refreshToken");
    this.claims = [];
  }

  loggedIn(): boolean {
    let isExpired = this.jwtHelper.isTokenExpired(this.storageService.getToken(), -120);
    return !isExpired;
  }

  getCurrentUserId() {
    this.jwtHelper.decodeToken(this.storageService.getToken()).userId;
  }

  claimGuard(claim: string): boolean {
    var check = this.claims.some(function (item) {
      return item == claim;
    })
    return check;
  }
}
