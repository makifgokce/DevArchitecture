import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LookUp } from '../models/look-up';
import { Observable } from 'rxjs';
import { environment } from 'src/environment';

@Injectable({
  providedIn: 'root'
})
export class LookUpService {

  constructor(private httpClient: HttpClient) { }
  getGroupLookUp(): Observable<LookUp[]> {

    return this.httpClient.get<LookUp[]>(environment.getApiUrl + "/Groups/getgrouplookup")
  }

  getOperationClaimLookUp(): Observable<LookUp[]> {

    return this.httpClient.get<LookUp[]>(environment.getApiUrl + "/OperationClaims/getoperationclaimlookup")
  }

  getUserLookUp():Observable<LookUp[]>{
    return this.httpClient.get<LookUp[]>(environment.getApiUrl + "/Users/GetUserLookup")
  }

  getLanguageLookup():Observable<LookUp[]>{
    return this.httpClient.get<LookUp[]>(environment.getApiUrl + "/Languages/getlookup")
  }
}
