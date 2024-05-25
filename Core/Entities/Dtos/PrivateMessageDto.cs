using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Dtos
{
    public class PrivateMessageDto : IDto
    {
        public int UserId { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Message Message { get; set; } = new Message();
        public int UnreadCount { get; set; }
    }
}
