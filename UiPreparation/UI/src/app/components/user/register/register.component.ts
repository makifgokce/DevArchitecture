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
      citizenId: ['', Validators.pattern(new RegExp("^[1-9]{1}[0-9]{9}[02468]{1}$"))],
      name: ['', [Validators.required, Validators.pattern(new RegExp("^(?=.{2,50}$)[A-Za-zÇçÖöÜüİıŞş]+(?:\\s[A-Za-zÇçÖöÜüİıŞşĞğ]+)*$"))]],
      surname: ['', [Validators.required, Validators.pattern(new RegExp("^(?=.{2,50}$)[A-Za-zÇçÖöÜüİıŞş]+(?:\\s[A-Za-zÇçÖöÜüİıŞşĞğ]+)*$"))]],
      account: ['', [Validators.required, Validators.pattern(new RegExp("^[\\w](?!.*?\\.{2})[\\w.]{1,18}[\\w]$"))]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(new RegExp("[A-Z]")), Validators.pattern(new RegExp("[a-z]")), Validators.pattern(new RegExp("[0-9]")), Validators.pattern(new RegExp("[^a-zA-Z0-9]"))]],
      birthDate: ['', Validators.required],
      gender: ['', Validators.required, Validators.pattern(new RegExp("[0-2]{1}"))]
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
  get citizenId(){
    return this.registerUser.get('citizenId')
  }
  get birthDate(){
    return this.registerUser.get('birthDate')
  }
  get gender(){
    return this.registerUser.get('gender')
  }
  get address(){
    return this.registerUser.get('address')
  }
  get notes(){
    return this.registerUser.get('notes')
  }
  getTitle(){
    return environment.getPageTitle
  }

  register(){
    if (!this.registerUser.valid){
      return;
    }
    const user = new RegisterUser();
    user.citizenId = this.citizenId?.value
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