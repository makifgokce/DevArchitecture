using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Message : IEntity
    {
        public Message()
        {
            if (Id == 0)
            {
                CreatedDate = DateTime.Now;
            }
        }
        public ulong Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; } = 0;
        public string Content { get; set; }
        public MessageLoc Location { get; set; }
        public ulong ReplyId { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
    public enum MessageLoc
    {
        Private,
        Group
    }
}