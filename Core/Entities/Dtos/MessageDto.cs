using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Dtos
{
    public class MessageDto : IDto
    {
        public ulong Id { get; set; }
        public int SenderId { get; set; }
        [NotMapped]
        public User Sender { get; set; }
        public int ReceiverId { get; set; } = 0;
        [NotMapped]
        public User Receiver { get; set; }
        public string Content { get; set; }
        public int Location { get; set; }
        public ulong ReplyId { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}