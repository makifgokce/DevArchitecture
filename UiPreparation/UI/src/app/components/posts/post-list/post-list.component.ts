import { Component } from '@angular/core';
import { Post } from 'src/app/models/post';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent {
    public posts!: Post[];
    constructor(private postService: PostService) {
      postService.GetPosts().subscribe(p => { this.posts = p})
    }
}
