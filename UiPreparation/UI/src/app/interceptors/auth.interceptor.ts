import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { BehaviorSubject, catchError, filter, finalize, Observable, switchMap, take, throwError } from 'rxjs';
import { TokenService } from '../services/token.service';
import { TranslateService } from '@ngx-translate/core';
import { AlertifyService } from '../services/alertify.service';
import { SharedService } from '../services/shared.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private tokenService:TokenService,
    private translate: TranslateService,
    private alertify: AlertifyService,
    private shared: SharedService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.shared.isLoading.next(true);
    request = this.addToken(request);

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error && error.status === 401 && !request.url.includes('/api/v1/Auth/Login')) {
          return this.handle401Error(request,next);
        }
        else {
          if (typeof error.error == "string"){
            this.alertify.error(error.error)
          }
          if (typeof error.error == "object"){
            this.alertify.error(error.error.message)
          }
          return throwError(() => error);
        }
      }),
      finalize(() => {
        this.shared.isLoading.next(false);
      })
    )
  }
  private addToken(req:HttpRequest<any>){
    var lang = this.translate.currentLang || localStorage.getItem("lang") || "tr-TR"

    //TODO: Refactoring needed

     return req.clone({
       setHeaders: {
         'Accept-Language': lang,
         'Authorization': `Bearer ${localStorage.getItem('token')}`

       },

       responseType: req.method == "DELETE" ? "text" : req.responseType
     });


  }
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  private handle401Error(req: HttpRequest<any>, next: HttpHandler):Observable<HttpEvent<any>> {
    if(!this.isRefreshing)  {
      this.isRefreshing=true;
      this.refreshTokenSubject.next(null);

      return this.tokenService.refreshToken().pipe(
       switchMap((token:any) => {
         console.log("Token Yenilendi.")
         this.isRefreshing = false;
         this.refreshTokenSubject.next(token.data.token);
         return next.handle(this.addToken(req))
       }));
    }
    else{
        return this.refreshTokenSubject.pipe(
          filter(token => token != null),
          take(1),
          switchMap(jwt => {
            return next.handle(this.addToken(req));
          })
        )
    }
  }
}
