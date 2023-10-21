import { Component } from '@angular/core';
import { Post } from 'src/app/models/post';
import { AuthService } from 'src/app/services/auth.service';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent {
  public posts!: Post[];
  constructor(private postService: PostService, private authService: AuthService) {
    postService.GetPosts().subscribe(p => { this.posts = p})
  }


  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }
}
