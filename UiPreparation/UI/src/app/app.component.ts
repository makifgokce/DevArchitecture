import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from './services/auth.service';
import { LocalStorageService } from './services/local-storage.service';
import { environment } from 'src/environment';
import { Subscription } from 'rxjs';
import { SharedService } from './services/shared.service';
import { SignalRService } from './services/signal-r.service';
export let browserRefresh = false;
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  subscription: Subscription;
  loading: boolean = false;
  constructor(private translate: TranslateService,
    private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private title: Title,
    private local: LocalStorageService,
    public shared: SharedService,
    private signalR: SignalRService) {
      const lang = translate.currentLang || translate.defaultLang || local.getItem("lang") || "tr-TR";
      translate.addLangs(["tr-TR", "en-US"]);
      translate.setDefaultLang("tr-TR");
      if (local.getItem("lang") == null){
        local.setItem("lang", lang)
      }
      if (!this.authService.loggedIn()) {
        this.authService.logOut();
        // this.router.navigateByUrl("/login");
      }
      this.setTitle();
      this.subscription = router.events.subscribe((event) => {
        if (event instanceof NavigationStart) {
          browserRefresh = !router.navigate;
        }
        if (event instanceof NavigationEnd){
          this.setTitle()
        }
      });
      this.shared.isLoading.subscribe((l) => {
        this.loading = l;
      });
  }
  setTitle(): void{
    const rt = this.shared.getChild(this.activatedRoute);
    rt.data.subscribe((data) => {
      this.translate.get('Title.'+ data['title']).subscribe(title => {
        this.title.setTitle(title + " | " + environment.getPageTitle)
      });
    });
  }
}
