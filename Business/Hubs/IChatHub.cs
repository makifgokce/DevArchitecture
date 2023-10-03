using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(string user, string message);
        Task Clients(List<ChatUser> users);
    }
}
