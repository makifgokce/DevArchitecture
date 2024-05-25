using AutoMapper;
using Business.BusinessAspects;
using Business.Helpers;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using Core.Utilities.Uri;
using Core.Utilities.URI;
using DataAccess.Abstract;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Users.Queries
{
    public class GetUsersQuery : IRequest<IDataResult<PaginatedResult<IEnumerable<UserDto>>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IDataResult<PaginatedResult<IEnumerable<UserDto>>>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly IUriService _uriService;

            public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper, IUriService uriService)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _uriService = uriService;
            }

            [SecuredOperation(Priority = 1)]
            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<PaginatedResult<IEnumerable<UserDto>>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var userList = await _userRepository.GetListAsync();
                var userDtoList = userList.Select(user => _mapper.Map<UserDto>(user)).ToList();
                var data = PaginationHelper.CreatePaginatedResponse<UserDto>(userDtoList, new PaginationFilter { PageSize = request.PageSize, PageNumber = request.PageNumber }, userDtoList.Count, _uriService, "/Users");
                return new SuccessDataResult<PaginatedResult<IEnumerable<UserDto>>>(data);
            }
        }
    }
}