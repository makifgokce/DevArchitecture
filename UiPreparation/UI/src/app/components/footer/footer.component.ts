import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
  socials:any = [];
  feign:any = [];
  hobs:any = [];
  studio: any = [];
  constructor() { }

  ngOnInit(): void {
    this.socials = environment.socials
    this.feign = environment.feign
    this.hobs = environment.hobs
    this.studio = environment.studio;
  }

}
