import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment';
import { Post } from '../models/post';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  constructor(private httpClient: HttpClient) { }
  GetPosts(): Observable<Post[]>{
    let headers = new HttpHeaders();
    headers = headers.append("Content-Type", "application/json")
    return this.httpClient.get<Post[]>(environment.getApiUrl + `/Post`, { headers: headers });
  }
  GetPost(id:number, slug:string): Observable<Post> {
    let headers = new HttpHeaders();
    headers = headers.append("Content-Type", "application/json")
    return this.httpClient.get<Post>(environment.getApiUrl + `/Post/${id}/${slug}`, { headers: headers });
  }
}
