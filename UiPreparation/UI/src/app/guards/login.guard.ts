import { AuthService } from "../services/auth.service";
import { CanActivate, Router } from "@angular/router";
import { Injectable } from "@angular/core";
@Injectable({
  providedIn: "root"
})
export class LoginGuard implements CanActivate {
    /**
     *
     */
    constructor(private auth: AuthService, private router: Router) {
    }
    canActivate() {
    if (this.auth.loggedIn()){
    return true;
    }
    this.router.navigateByUrl("/login");
    return false;
  }
}