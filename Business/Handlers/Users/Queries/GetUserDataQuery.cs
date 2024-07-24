using AutoMapper;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
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

namespace Business.Handlers.Users.Queries
{
    public class GetUserDataQuery : IRequest<IDataResult<UserDto>>
    {
        public class GetUserDataQueryHandler : IRequestHandler<GetUserDataQuery, IDataResult<UserDto>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContext;

            public GetUserDataQueryHandler(IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContext)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _httpContext = httpContext;
            }
            [SecuredOperation]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<UserDto>> Handle(GetUserDataQuery request, CancellationToken cancellationToken)
            {
                var userId = _httpContext.HttpContext?.User.Claims
                    .FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                var user = await _userRepository.GetAsync(p => p.UserId == Convert.ToInt32(userId));
                var userDto = _mapper.Map<UserDto>(user);
                return new SuccessDataResult<UserDto>(userDto);
            }
        }
    }
}