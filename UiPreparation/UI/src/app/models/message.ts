import { User } from "./user";

export class Message {
    id: number = 0;
    senderId: number = 0;
    sender: User = new User();
    receiverId: number = 0;
    receiver: User = new User();
    content: string = "";
    replyId: number = 0;
    location: number = 0;
    createdDate: string = "";
    updatedDate: string = "";
}