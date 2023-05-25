import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { LoginUser } from 'src/app/models/login-user';
import { AuthService } from 'src/app/services/auth.service';
import { LoadingService } from 'src/app/services/loading.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loading: boolean = false;
  username:string="";
  loginUser!: FormGroup;
  constructor(private auth:AuthService,
    private storageService:LocalStorageService,
    public translate: TranslateService,
    private fb: FormBuilder,
    private _loading: LoadingService) { }

  ngOnInit(): void {
    this._loading.isLoading.subscribe((l) => {
      this.loading = l;
    })
    this.loginUser = this.fb.group({
      account: ['', [Validators.required, Validators.pattern(new RegExp("^[\\w](?!.*?\\.{2})[\\w.]{1,18}[\\w]$"))]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(new RegExp("[A-Z]")), Validators.pattern(new RegExp("[a-z]")), Validators.pattern(new RegExp("[0-9]"))]]
    })
    this.username=this.auth.getUserName();
  }

  get account(){
    return this.loginUser.get('account')
  }

  get password(){
    return this.loginUser.get('password')
  }

  getTitle(){
    return environment.getPageTitle
  }

  getUserName(){
    return this.username;
  }

  login(){
    if(!this.loginUser.valid){
      return;
    }
    const user = new LoginUser()
    user.account = this.loginUser.get('account')?.value
    user.password = this.loginUser.get('password')?.value
    this.auth.login(user);
  }

  logOut(){
    this.auth.logOut();
  }
}
