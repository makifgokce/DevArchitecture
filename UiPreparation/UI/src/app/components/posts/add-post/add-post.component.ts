import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-add-post',
  templateUrl: './add-post.component.html',
  styleUrls: ['./add-post.component.scss']
})
export class AddPostComponent {
  editorConfig = {base_url: '/tinymce', suffix: '.min', plugins: 'lists link image table code help wordcount'}
  addPost = new FormGroup({
    postTitle: new FormControl('', Validators.required),
    body: new FormControl('', [Validators.required]),
    slug: new FormControl(''),
    description: new FormControl('',[Validators.required]),
    keywords: new FormControl('', [Validators.required]),
    publishDate: new FormControl('')
  });

  constructor(private postService: PostService) {
  }
  save(){
    let contents = {
      title: this.postTitle?.value,
      body: this.body?.value,
      slug: this.slug?.value,
      description: this.description?.value,
      keywords: this.keywords?.value,
      publishDate: this.publishDate?.value
    }
    this.postService.AddPost(contents);
  }
  get postTitle(){
    return this.addPost.get('postTitle')
  }
  get body(){
    return this.addPost.get('body')
  }
  get slug(){
    return this.addPost.get('slug')
  }
  get description(){
    return this.addPost.get('description')
  }
  get keywords(){
    return this.addPost.get('keywords')
  }
  get publishDate(){
    return this.addPost.get('publishDate')
  }
}

