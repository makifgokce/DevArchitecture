import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { PostService } from 'src/app/services/post.service';
import { DataTable } from 'src/app/models/datatable';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent {
  public posts: DataTable = new DataTable;
  public pattern = /\r\n|\n|\r/;
  constructor(private postService: PostService, private authService: AuthService) {
    this.loadMore()
  }
  loadMore(){
    let params = new HttpParams();
    params = params.set('pageNumber', 1);
    params = params.set('pageSize', 10);
    if(typeof this.posts.nextPage != 'undefined')
    {
      const nxt = new URL(this.posts.nextPage)
      params = params.set('pageNumber', nxt.searchParams.get('pageNumber') ?? 1);
      params = params.set('pageSize', nxt.searchParams.get('pageSize') ?? 10);
    }
    
    if (Number(params.get('pageNumber')) < this.posts.pageNumber){
      return;
    }
    this.postService.GetPosts(params).subscribe(p => {
      let cacheData: any[] = this.posts.data
      this.posts = p
      if (typeof cacheData != 'undefined')
      {
        cacheData.push(...p.data)
        this.posts.data = cacheData
      }
    })
  }
  deletePost(id: number){
    this.postService.DeletePost(id);
  }
  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }
}
