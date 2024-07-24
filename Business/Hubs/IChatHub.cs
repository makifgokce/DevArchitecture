using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(string message);
        Task PrivateMessage(string account, string message);
        Task Clients(List<ChatUser> users);
    }
}
