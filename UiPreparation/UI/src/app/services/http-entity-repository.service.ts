import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpEntityRepositoryService<T> {
  headers = new HttpHeaders().append("Content-Type", "application/json");
  constructor(private httpClient: HttpClient) { }
  /// /api/[controller] - GET
  getAll(_url: string): Observable<T[]> {
    return this.httpClient.get<T[]>(
      environment.getApiUrl + _url, {headers : this.headers});
  }

  //api/[controller]/:id - GET
  /*
  get(_url: string, id?: number): Observable<T> {
    return this.httpClient.get<T>(
      environment.getApiUrl +  _url + ((id != undefined && id != null) ? + id : ""),
    );
  }
  */
  get(_url: string, ...args: any[]){
    let str = `${environment.getApiUrl + _url}`;
    args.forEach( x => {
      if(x.trim() != '' && x != undefined){
        str += "/" + x;
      }
    })
    return this.httpClient.get<T>(
      str,
      {headers : this.headers}
    );
  }
  /// /api/[controller] - POST
  save<_T>(_url: string, _content: any): Observable<_T> {
    return this.httpClient.post<_T>(
      environment.getApiUrl +  _url,
      _content
    );
  }
  /// /api/[controller] - POST
  add(_url: string, _content: any): Observable<T> {
    return this.httpClient.post<T>(
      environment.getApiUrl +  _url,
      _content
    );
  }

  // /api/[controller] - PUT
  update(_url: string, _content: any): Observable<T> {
    return this.httpClient.put<T>(
      environment.getApiUrl + _url,
      _content
    );
  }

  // /api/[controller]/:id - DELETE
  delete(_url: string, id: number): Observable<T> {
    return this.httpClient.delete<T>(
      environment.getApiUrl + _url  + id
    );
  }
}
