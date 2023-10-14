import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validator } from '@angular/forms';
import { Title, Meta } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/models/post';
import { AuthService } from 'src/app/services/auth.service';
import { PostService } from 'src/app/services/post.service';
import { environment } from 'src/environment';

@Component({
  selector: 'app-update-post',
  templateUrl: './update-post.component.html',
  styleUrls: ['./update-post.component.scss']
})
export class UpdatePostComponent {
  public post: Post = new Post;
  public updatePost!:FormGroup;
  constructor(private httpClient: HttpClient, private title: Title, private meta: Meta, private activatedRoute: ActivatedRoute, private postService: PostService,private fb: FormBuilder, private authService: AuthService) {
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

    this.updatePost = this.fb.group({

    })
  }
  update(){
    debugger
  }
  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }
}
