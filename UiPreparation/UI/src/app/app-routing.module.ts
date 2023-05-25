import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LocalizeParser, LocalizeRouterModule, LocalizeRouterSettings, ManualParserLoader } from '@gilsdav/ngx-translate-router';
import { TranslateService } from '@ngx-translate/core';
import { HomeComponent } from './components/home/home.component';
import { AccountComponent } from './components/user/account/account.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { LoginGuard } from './guards/login.guard';
import { NotLoginGuard } from './guards/not-login.guard';

const routes: Routes = [
  {
    path: "",
    component: HomeComponent,
    pathMatch: 'full',
    data: {
      title: 'home'
    }
  },
  {
    path: "login",
    component: LoginComponent,
    canActivate: [NotLoginGuard],
    data: {
      title: 'login'
    }
  },
  {
    path: "register",
    component: RegisterComponent,
    canActivate: [NotLoginGuard],
    data: {
      title: 'register'
    }
  },
  {
    path: "account",
    component: AccountComponent,
    canActivate: [LoginGuard],
    data: {
      title: 'account'
    }
  }
];

@NgModule({
  imports: [
    LocalizeRouterModule.forRoot(routes, {
      parser: {
        provide: LocalizeParser,
        useFactory: (translate: TranslateService, location: Location, settings: LocalizeRouterSettings, http: HttpClient) =>
            new ManualParserLoader (translate, location, settings, ["tr-TR", "en-US"]),
        deps: [TranslateService, Location, LocalizeRouterSettings, HttpClient]
      },
      initialNavigation: true,
      alwaysSetPrefix: true
    }),
    RouterModule.forRoot(routes, {
      useHash: true,
      initialNavigation: 'disabled'
    })
  ],
  exports: [RouterModule, LocalizeRouterModule]
})
export class AppRoutingModule { }
