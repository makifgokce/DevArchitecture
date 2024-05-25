using AutoMapper.Internal;
using Business.BusinessAspects;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Hubs
{
    [AllowAnonymous]
    public class ChatHub : Hub<IChatHub>
    {
        private static List<ChatUser> chatUsers = new List<ChatUser>();
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;

        public ChatHub(IMessageRepository messageRepository, IHttpContextAccessor httpContext, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _httpContext = httpContext;
            _userRepository = userRepository;

        }
        [SecuredOperation(false)]
        public async Task PrivateMessage(string account, string message)
        {

            var userId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
            var u = chatUsers.FirstOrDefault(x => x.Account == account);
            var sender = await _userRepository.GetAsync(x => x.UserId == Convert.ToInt32(userId));
            var receiver = await _userRepository.GetAsync(x => x.Account == account);
            if (sender == null || receiver == null)
            {
                return;
            }

            _messageRepository.Add(new Message
            {
                ReceiverId = receiver.UserId,
                Content = message,
                SenderId = sender.UserId,
                Location = MessageLoc.Private
            });
            var res = await _messageRepository.SaveChangesAsync();
            if (u != null && res != 0)
            {
                await Clients.Client(u.ConnectionId).ReceiveMessage(message);
            }
        }

        public static List<ChatUser> GetUsers()
        {
            return chatUsers;
        }

        public override Task OnConnectedAsync()
        {
            var item = chatUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            var uId = Convert.ToInt32(Context.GetHttpContext().User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value);
            var uAcc = Context.GetHttpContext().User.Claims.FirstOrDefault(x => x.Type.EndsWith("/name"))?.Value;
            var userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            var ip = Context.GetHttpContext().Connection.RemoteIpAddress;
            if (item == null)
            {
                chatUsers.TryAdd(new ChatUser
                {
                    ConnectionId = Context.ConnectionId,
                    UId = uId,
                    Account = uAcc != null ? uAcc : "",
                    UserAgent = userAgent,
                    Ip = ip.ToString(),
                    LastOnline = DateTime.Now
                });
                Clients.All.Clients(chatUsers);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var item = chatUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                chatUsers.Remove(item);
            }
            Clients.All.Clients(chatUsers);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
