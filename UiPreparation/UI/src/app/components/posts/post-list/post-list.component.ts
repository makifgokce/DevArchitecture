import { Component, ViewChild } from '@angular/core';
import { Post } from 'src/app/models/post';
import { AuthService } from 'src/app/services/auth.service';
import { PostService } from 'src/app/services/post.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent {
  public posts!: Post[];
  dataSource!: MatTableDataSource<Post>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  displayedColumns: string[] = [
    "title",
    "authorName",
    "publishDate",
    "update",
    "delete"
  ];
  constructor(private postService: PostService, private authService: AuthService) {
    postService.GetPosts().subscribe(p => { 
      this.posts = p;
      this.dataSource = new MatTableDataSource(p);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    })
  }

  deletePost(id: number){
    this.postService.DeletePost(id);
  }
  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }
}
