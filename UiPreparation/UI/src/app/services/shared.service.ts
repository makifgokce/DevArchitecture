import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs'

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  subject = new Subject<any>();
  constructor() { }
  sendChangeUserNameEvent(){
    this.subject.next(1);
  }
  getChangeUserNameClickEvent():Observable<any>{
    return this.subject.asObservable();
  }
}
