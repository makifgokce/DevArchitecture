import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '../models/post';
import { Observable } from 'rxjs';
import { HttpEntityRepositoryService } from './http-entity-repository.service';
import { AlertifyService } from './alertify.service';
import { environment } from 'src/environment';
import { DataTable } from '../models/datatable';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  constructor(private http: HttpEntityRepositoryService<Post>, private httpClient: HttpClient, private alertify: AlertifyService) { }
  GetPosts(): Observable<Post[]>{
    return this.http.getAll("/Post");
  }
  GetPost(id:number, slug:string): Observable<Post> {
    return this.http.get("/Post", id, slug);
  }
  AddPost(content: any){
    this.httpClient.post<any>(environment.getApiUrl + "/Post", content).subscribe(data => {
      if(data.success){
        this.alertify.success(data.message)
      }
    })
  }
  
  UpdatePost(content: any){
    this.httpClient.put<any>(environment.getApiUrl + "/Post", content).subscribe(data => {
      if(data.success){
        this.alertify.success(data.message)
      }
    })
  }
  DeletePost(_id: number){
    this.httpClient.delete<any>(`${environment.getApiUrl}/Post/${_id}`).subscribe(data => {
      if(data.success){
        this.alertify.warning(data.message)
      }
    })
  }
}
