import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Observable } from 'rxjs';
import { environment } from 'src/environment';
import { HttpClient } from '@angular/common/http';
import { PrivateMessage } from '../models/private-message';
import { DataTable } from '../models/datatable';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }
  
  GetPost(account:string): Observable<User> {
    return this.http.get<User>(`${environment.getApiUrl}/Users/${account}`);
  }

  GetUserData(): Observable<User> {
    return this.http.get<User>(`${environment.getApiUrl}/Users/Data`);
  }
  GetUsers(): Observable<DataTable> {
    return this.http.get<DataTable>(`${environment.getApiUrl}/Users`);
  }

  DeleteUser(account: string){
    this.http.delete(`${environment.getApiUrl}/Users/${account}`)
  }
  updateUser(user: User){
    return this.http.put(`${environment.getApiUrl}/Users/${user.account}`, user);
  }

  GetPrivateMessageList(){
    return this.http.get<PrivateMessage[]>(`${environment.getApiUrl}/Messages`)
  }
  
  GetPrivateMessage(account: string){
    return this.http.get<DataTable>(`${environment.getApiUrl}/Messages/${account}`)
  }
}
