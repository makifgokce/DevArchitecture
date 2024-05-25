using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class MessageRepository : EfEntityRepositoryBase<Message, ProjectDbContext>, IMessageRepository
    {
        public MessageRepository(ProjectDbContext context) : base(context)
        {
            
        }

        public async Task<List<MessageDto>> GetPrivateMessage(int id, string account)
        {
            var data = await (from m in Context.Messages join s in Context.Users on m.SenderId equals s.UserId
                        join r in Context.Users on m.ReceiverId equals r.UserId
                    where (s.Account == account && m.ReceiverId == id) || (r.Account == account && m.SenderId == id)
                    select new MessageDto
                    {
                        Id = m.Id,
                        SenderId = m.SenderId,
                        Sender = s,
                        ReceiverId = m.ReceiverId,
                        Receiver = r,
                        Content = m.Content,
                        ReplyId = m.ReplyId,
                        Location = (int) m.Location,
                        CreatedDate = m.CreatedDate,
                        UpdatedDate = m.UpdatedDate,
                    }).OrderBy(x => x.CreatedDate).ToListAsync();
            return data;
        }

        public async Task<List<PrivateMessageDto>> GetPrivateMessageList(int id)
        {
            var messages = await Context.Messages.Join(Context.Users, m => (m.SenderId == id) ? m.ReceiverId : m.SenderId, u => u.UserId, (message, user) => new {
                UserId = user.UserId,
                Account = user.Account,
                Name = user.Name,
                Surname = user.Surname,
                Message = message
            }).Where(x => x.Message.SenderId == id || x.Message.ReceiverId == id).GroupBy(x => (x.Message.SenderId == id) ? x.Message.ReceiverId : x.Message.SenderId, (key, m) => new { Message = m.OrderByDescending(x => x.Message.CreatedDate).FirstOrDefault(), Unread = m.Count() }).ToListAsync();
            var data = messages.Select(x => new PrivateMessageDto
            {
                UserId = x.Message.UserId,
                Account = x.Message.Account,
                Name = x.Message.Name,
                Surname = x.Message.Surname,
                Message = x.Message.Message,
                UnreadCount = x.Unread
            }).ToList();
            return data; 
        }
    }
}
