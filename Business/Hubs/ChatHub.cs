﻿using AutoMapper.Internal;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Hubs
{
    [AllowAnonymous]
    public class ChatHub : Hub<IChatHub>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private static List<ChatUser> chatUsers = new List<ChatUser>();

        public ChatHub()
        {
            _contextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }
        public void SayHello(string message)
        {
            var cId = chatUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine(Clients.All.ToString());
            var item = chatUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            var uId = Convert.ToInt32(_contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value);
            var uAcc = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type.EndsWith("Name"))?.Value;
            if (item == null)
            {
                chatUsers.TryAdd(new ChatUser
                {
                    ConnectionId = Context.ConnectionId,
                    UId = uId,
                    Account = uAcc != null ? uAcc : "Guest",
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
