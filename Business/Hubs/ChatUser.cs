using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Hubs
{
    public class ChatUser
    {
        public int UId { get; set; }
        public string ConnectionId { get; set; }
        public string Account { get; set; }
        public DateTime LastOnline { get; set; } = DateTime.Now;
    }
}
