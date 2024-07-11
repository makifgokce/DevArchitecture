import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Post } from 'src/app/models/post';
import { AuthService } from 'src/app/services/auth.service';
import { PostService } from 'src/app/services/post.service';
import { environment } from 'src/environment';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent {
  public post: Post = new Post;
  constructor(private httpClient: HttpClient, private title: Title, private meta: Meta, private activatedRoute: ActivatedRoute, private postService: PostService, public auth: AuthService) {
    this.activatedRoute.params.subscribe(p => {
      //this.post = postService.GetPost(p['id'], p['slug'])
      postService.GetPost(p['id'], p['slug']).subscribe(x => {
        this.post = x
        this.title.setTitle(x.title + " | " + environment.getPageTitle)
        this.meta.addTags([
          {
            name: 'keywords',
            content: x.keywords,
          },
          { name: 'robots', content: 'index, follow' },
          { name: 'title', content: x.title },
          { name: 'description', content: x.body },
          { name: 'viewport', content: 'width=device-width, initial-scale=1' },
          { name: 'date', content: x.createdDate, scheme: 'YYYY-MM-DD' },
        ])
      })
    })
  }
  keywords(){
    let k = this.post.keywords?.split(',')
    return k
  }
  checkClaim(claim: string){
    return this.auth.claimGuard(claim)
  }
  
  deletePost(id: number){
    this.postService.DeletePost(id);
  }
}
