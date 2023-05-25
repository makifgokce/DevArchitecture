import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthService } from './services/auth.service';
import { LoadingService } from './services/loading.service';
import { LocalStorageService } from './services/local-storage.service';

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
    public _loading: LoadingService)
  {
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
    this._loading.isLoading.subscribe((l) => {
      this.loading = l;
    });
  }
  setTitle(): void{
    const rt = this.getChild(this.activatedRoute);
    rt.data.subscribe((data) => {
      this.translate.get('Title.'+ data['title']).subscribe(title => {
        this.title.setTitle(title + " | " + environment.getPageTitle)
      });
    });
  }
  getChild(activatedRoute: ActivatedRoute): ActivatedRoute {
    if (activatedRoute.firstChild) {
      return this.getChild(activatedRoute.firstChild);
    } else {
      return activatedRoute;
    }
  }
}
