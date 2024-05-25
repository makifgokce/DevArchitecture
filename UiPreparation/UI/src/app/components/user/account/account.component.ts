import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DataTable } from 'src/app/models/datatable';
import { User } from 'src/app/models/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UserService } from 'src/app/services/user.service';

declare var jQuery: any;

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent {
  user: User = new User;
  target: User = new User;
  userList: DataTable = new DataTable;
  userAddForm!: FormGroup;
  dataSource!: MatTableDataSource<User>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  displayedColumns = ["userId", "account", "name", "surname", "email", "mobilePhones", "gender", "status", "update", "delete"];
  constructor(private auth: AuthService, private u: UserService, private signalR: SignalRService, private alertify: AlertifyService) {
    var account = auth.getUser().account
    this.createUserForm()
    u.GetUserData().subscribe(data => this.user = data);
    u.GetUsers().subscribe(data => {
      this.userList = data;
      this.dataSource = new MatTableDataSource(this.userList.data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }
  createUserForm(user: User = new User){
    this.userAddForm = new FormGroup({
      name: new FormControl([user.name], [Validators.required]),
      surname: new FormControl([user.surname], [Validators.required]),
      email: new FormControl([user.email], [Validators.required]),
      mobilePhones: new FormControl([user.mobilePhones]),
      address: new FormControl([user.address]),
      notes: new FormControl([user.notes]),
      gender: new FormControl([user.gender], [Validators.required]),
      status: new FormControl([user.status], [Validators.required]),
    })
  }
  checkClaim(claim:string){
    return this.auth.claimGuard(claim)
  }
  deleteUser(account:string){
    if(confirm("Are you sure?")){
      this.u.DeleteUser(account);
    }
  }
  updateUser() {
    this.u.updateUser(this.target).subscribe((data) => {
      var index = this.userList.data.findIndex((x) => x.userId == this.user.userId);
      this.userList.data[index] = this.target;
      this.dataSource = new MatTableDataSource(this.userList.data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.target = new User();
      jQuery("#user").modal("hide")
      //this.alertify.success(data.message);
      console.log(data)
      this.clearFormGroup(this.userAddForm);
    });
  }
  getUserByAcc(account:string){
    this.userList.data.forEach((x) => {
      if(x.account == account){
        {
          this.target = x;
          this.clearFormGroup(this.userAddForm)
          this.createUserForm(x)
          this.userAddForm.patchValue(x)
        }
      }
    })
  }
  
  clearFormGroup(group: FormGroup) {
    group.markAsUntouched();
    group.reset();

    Object.keys(group.controls).forEach((key) => {
      group.get(key)?.setErrors(null);
    });
  }

}

