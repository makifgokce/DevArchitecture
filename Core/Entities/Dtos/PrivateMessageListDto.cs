using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Dtos
{
    public class PrivateMessageListDto : IDto
    {
        public string Account { get; set; }
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
    }
}
