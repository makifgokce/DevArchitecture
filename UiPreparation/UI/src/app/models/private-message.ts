import { Message } from "./message";

export class PrivateMessage {
    userId:number = 0;
    account:string = "";
    name:string = "";
    surname:string = "";
    message:Message = new Message();
    unreadCount:number = 0;
}