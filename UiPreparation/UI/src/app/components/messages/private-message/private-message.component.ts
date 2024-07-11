import { Component } from '@angular/core';
import { DataTable } from 'src/app/models/datatable';
import { Message } from 'src/app/models/message';
import { PrivateMessage } from 'src/app/models/private-message';
import { PrivateMessageList } from 'src/app/models/private-message-list';
import { User } from 'src/app/models/user';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-private-message',
  templateUrl: './private-message.component.html',
  styleUrls: ['./private-message.component.scss']
})
export class PrivateMessageComponent {
  messagerList: PrivateMessage[] = [];
  targetUser: User = new User()
  messages: Message[] = [];
  dataSource: DataTable = new DataTable();
  constructor(private signalR: SignalRService, private user: UserService) {
    this.user.GetPrivateMessageList().subscribe(data => {
      this.messagerList = data
    })
  }
  GetMessages(account:string){
    this.user.GetPrivateMessage(account).subscribe(data => {
      this.dataSource = data;
      this.messages = data.data
      data.data.forEach(x => {
        if(x.receiver.account == account) {
          this.targetUser = x.receiver
        }
        if(x.sender.account == account) {
          this.targetUser = x.sender
        }
      })
    })
  }
  GetReplyMessage(replyId:number){
    return this.messages.find(x => x.id == replyId)?.content;
  }
}
