import { AuthService } from "../services/auth.service";
import { CanActivate, Router } from "@angular/router";
import { Injectable } from "@angular/core";
import { LocalizeRouterService } from "@gilsdav/ngx-translate-router";
@Injectable({
  providedIn: "root"
})
export class NotLoginGuard implements CanActivate{
    /**
     *
     */
    constructor(private auth: AuthService, private router: Router, private localize: LocalizeRouterService) {
        
    }
    canActivate(){
    if (!this.auth.loggedIn()){
    return true;
    }
    this.router.navigateByUrl(this.localize.translateRoute('/login').toString());
    return false;
  }
}