import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalizeRouterService } from '@gilsdav/ngx-translate-router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  userName!: string;
  clickEventSubscription: Subscription;

  constructor(private authService: AuthService, private router: Router,
    private sharedService:SharedService,
		private translateService: TranslateService, public localizeService: LocalizeRouterService, private local: LocalStorageService) {
      this.clickEventSubscription= this.sharedService.getChangeUserNameClickEvent().subscribe(()=>{
        this.setUserName();
      })
    }

  ngOnInit(): void {
    this.setUserName();
  }
  isLoggedIn(): boolean {

		return this.authService.loggedIn();
	}

	logOut() {
		this.authService.logOut();
		this.router.navigate(["login"]);
	}

	setUserName(){
		this.userName = this.authService.getUserName();
	}
	changeLanguage(lang:string){
		this.translateService.use(lang)
    this.localizeService.changeLanguage(lang)
    this.local.setItem("lang", lang)
	}
}