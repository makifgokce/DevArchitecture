using AutoMapper;
using Business.BusinessAspects;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Message.Queries
{
    public class GetMessageListQuery : IRequest<IDataResult<IEnumerable<PrivateMessageDto>>>
    {
        public class GetMessageListQueryHandler : IRequestHandler<GetMessageListQuery, IDataResult<IEnumerable<PrivateMessageDto>>>
        {
            private readonly IMessageRepository _messageRepository;
            private readonly IHttpContextAccessor _httpContext;
            public GetMessageListQueryHandler(IMessageRepository messageRepository, IHttpContextAccessor httpContext)
            {
                _messageRepository = messageRepository;
                _httpContext = httpContext;
            }
            [SecuredOperation(false)]
            public async Task<IDataResult<IEnumerable<PrivateMessageDto>>> Handle(GetMessageListQuery request, CancellationToken cancellationToken)
            {
                var userId = Convert.ToInt32(_httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value);
                var messages = await _messageRepository.GetPrivateMessageList(userId);
                return new SuccessDataResult<IEnumerable<PrivateMessageDto>>(messages);
            }
        }
    }
}