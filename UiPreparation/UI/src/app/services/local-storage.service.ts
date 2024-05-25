import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }
  setToken(token: string) {
    localStorage.setItem("token", token);
  }

  removeToken(){
    localStorage.removeItem("token");
  }

  removeItem(itemName:string)
  {
    localStorage.removeItem(itemName);
  }

  getToken():string {
    const token: any = localStorage.getItem("token");
    if (token != null){
      return token
    }
    return ""
  }

  setItem(key:string,data:any){
    localStorage.setItem(key,data);
  }

  getItem(key:string){
    return localStorage.getItem(key);
  }
}
