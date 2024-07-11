import { Component, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
  editorConfig = {base_url: '/tinymce', suffix: '.min', plugins: 'lists link image table code help wordcount'}
  public post: Post = new Post;
  public updatePost!:FormGroup;
  constructor(private title: Title, private meta: Meta, private activatedRoute: ActivatedRoute, private postService: PostService,private fb: FormBuilder, private authService: AuthService) {
    this.activatedRoute.params.subscribe(p => {
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

    this.updatePost = new FormGroup({
      postTitle: new FormControl([this.post.title], Validators.required),
      body: new FormControl([this.post.body], [Validators.required]),
      slug: new FormControl([this.post.slug]),
      description: new FormControl([this.post.description],[Validators.required]),
      keywords: new FormControl([this.post.keywords], [Validators.required])
    });
  }
  update(){
    let contents = {
      id: this.post.id,
      title: this.postTitle?.value,
      body: this.body?.value,
      slug: this.slug?.value,
      description: this.description?.value,
      keywords: this.keywords?.value
    }
    this.postService.UpdatePost(contents);
  }
  
  get postTitle(){
    return this.updatePost.get('postTitle')
  }
  get body(){
    return this.updatePost.get('body')
  }
  get slug(){
    return this.updatePost.get('slug')
  }
  get description(){
    return this.updatePost.get('description')
  }
  get keywords(){
    return this.updatePost.get('keywords')
  }
  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }
}
