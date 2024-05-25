using Core.Utilities.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DataAccess.Abstract;
using Core.Entities.Dtos;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Business.BusinessAspects;
using Business.Helpers;
using Core.Entities.Concrete;
using Core.Utilities.URI;

namespace Business.Handlers.Message.Queries
{
    public class GetMessageQuery : IRequest<IDataResult<PaginatedResult<IEnumerable<MessageDto>>>>
    {
        public string Account {  get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public class GetMessageQueryHandler : IRequestHandler<GetMessageQuery, IDataResult<PaginatedResult<IEnumerable<MessageDto>>>>
        {
            private readonly IMessageRepository _messageRepository;
            private readonly IHttpContextAccessor _httpContext;
            private readonly IMapper _mapper;
            private readonly IUriService _uriService;
            public GetMessageQueryHandler(IMessageRepository messageRepository, IHttpContextAccessor httpContext, IMapper mapper, IUriService uriService)
            {
                _messageRepository = messageRepository;
                _httpContext = httpContext;
                _mapper = mapper;
                _uriService = uriService;

            }
            [SecuredOperation(false)]
            public async Task<IDataResult<PaginatedResult<IEnumerable<MessageDto>>>> Handle(GetMessageQuery request, CancellationToken cancellationToken)
            {
                var userId = _httpContext.HttpContext?.User.Claims
                    .FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                var messages = await _messageRepository.GetPrivateMessage(Convert.ToInt32(userId), request.Account);

                var data = PaginationHelper.CreatePaginatedResponse(messages, new PaginationFilter { PageSize = request.PageSize, PageNumber = request.PageNumber }, messages.Count, _uriService, "/Users");
                return new SuccessDataResult<PaginatedResult<IEnumerable<MessageDto>>>(data);
            }
        }
    }
}
