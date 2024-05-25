using Core.DataAccess;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Migrations.Ms;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IMessageRepository : IEntityRepository<Message>
    {
        Task<List<MessageDto>> GetPrivateMessage(int id, string account);
        Task<List<PrivateMessageDto>> GetPrivateMessageList(int id);
    }
}
