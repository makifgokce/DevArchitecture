import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '../models/post';
import { Observable } from 'rxjs';
import { HttpEntityRepositoryService } from './http-entity-repository.service';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  constructor(private http: HttpEntityRepositoryService<Post>) { }
  GetPosts(): Observable<Post[]>{
    return this.http.getAll("/Post");
  }
  GetPost(id:number, slug:string): Observable<Post> {
    return this.http.get("/Post", id, slug);
  }
  AddPost(content: any){
    this.http.add("/Post", content).subscribe(data => {
      console.log("AddPost", data)
    })
  }
}
