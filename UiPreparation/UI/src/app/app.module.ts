import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateLoader, TranslateModule, TranslatePipe } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslationService } from './services/translation.service';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { LoginGuard } from './guards/login.guard';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { HttpEntityRepositoryService } from './services/http-entity-repository.service';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { AccountComponent } from './components/user/account/account.component';
import { PostComponent } from './components/posts/post/post.component';
import { PostListComponent } from './components/posts/post-list/post-list.component';
import { UpdatePostComponent } from './components/posts/update-post/update-post.component';
import { AddPostComponent } from './components/posts/add-post/add-post.component';
export function layoutHttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http,'/assets/i18n/','.json');
}
@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    AccountComponent,
    PostComponent,
    PostListComponent,
    UpdatePostComponent,
    AddPostComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader:{
        provide: TranslateLoader,
        useFactory: layoutHttpLoaderFactory,
        useClass: TranslationService,
        deps: [HttpClient]
      }
    }),
    AppRoutingModule
  ],
  providers: [
    LoginGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    HttpEntityRepositoryService
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
