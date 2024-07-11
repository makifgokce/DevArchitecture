import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environment';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  user!: User;
  constructor(private route: ActivatedRoute, private u: UserService, private title: Title) {
    this.route.params.subscribe(r => 
      this.u.GetPost(r['account']).subscribe(data => 
        {
          this.user = data
          this.title.setTitle(`${this.user.name} ${this.user.surname} (${this.user.account}) | ${environment.getPageTitle}`)
        })
      )
  }
}
