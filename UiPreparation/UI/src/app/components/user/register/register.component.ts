import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { RegisterUser } from 'src/app/models/register-user';
import { AuthService } from 'src/app/services/auth.service';
import { SharedService } from 'src/app/services/shared.service';
import { environment } from 'src/environment';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  loading: boolean = false;
  registerUser!: FormGroup;
  constructor(private auth:AuthService,
    public translate: TranslateService,
    private fb: FormBuilder,
    private shared: SharedService) { }

  ngOnInit(): void {
    this.shared.isLoading.subscribe((l)=>{
      this.loading = l;
    })
    this.registerUser = this.fb.group({
      name: ['', [Validators.required, Validators.pattern(new RegExp("^(?=.{2,50}$)[A-Za-zÇçÖöÜüİıŞş]+(?:\\s[A-Za-zÇçÖöÜüİıŞşĞğ]+)*$"))]],
      surname: ['', [Validators.required, Validators.pattern(new RegExp("^(?=.{2,50}$)[A-Za-zÇçÖöÜüİıŞş]+(?:\\s[A-Za-zÇçÖöÜüİıŞşĞğ]+)*$"))]],
      account: ['', [Validators.required, Validators.pattern(new RegExp("^[\\w](?!.*?\\.{2})[\\w.]{1,18}[\\w]$"))]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(new RegExp("[A-Z]")), Validators.pattern(new RegExp("[a-z]")), Validators.pattern(new RegExp("[0-9]")), Validators.pattern(new RegExp("[^a-zA-Z0-9]"))]]
    })
  }
  get name(){
    return this.registerUser.get('name')
  }
  get surname(){
    return this.registerUser.get('surname')
  }
  get account(){
    return this.registerUser.get('account')
  }
  get email(){
    return this.registerUser.get('email')
  }
  get password(){
    return this.registerUser.get('password')
  }
  getTitle(){
    return environment.getPageTitle
  }

  register(){
    if (!this.registerUser.valid){
      return;
    }
    const user = new RegisterUser();
    user.name = this.name?.value
    user.surname = this.surname?.value
    user.account = this.account?.value
    user.email = this.email?.value
    user.password = this.password?.value
    this.auth.register(user)
  }

  clear(){
    this.registerUser.reset();
  }
}