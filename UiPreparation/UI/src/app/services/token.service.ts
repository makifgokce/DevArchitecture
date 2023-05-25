import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs/internal/operators/tap'
import { environment } from 'src/environments/environment';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private httpClient:HttpClient, private storageService:LocalStorageService) { }
  refreshToken():any{
    if(this.storageService.getItem("refreshToken") !== null)
    return this.httpClient
        .post<any>(environment.getApiUrl + "/Auth/RefreshToken",{refreshToken:this.storageService.getItem("refreshToken")})
        .pipe(tap(res => {
          if(res.success){
            this.storageService.setToken(res.data.token);
            this.storageService.setItem("refreshToken",res.data.refreshToken);
          }
      }));
  }
}
