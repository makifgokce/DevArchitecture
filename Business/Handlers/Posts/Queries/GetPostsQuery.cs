using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Logging;
using System.Collections.Generic;
using Core.Entities.Dtos;
using AutoMapper;
using Core.Aspects.Autofac.Performance;
using Business.Helpers;
using Core.Entities.Concrete;
using Core.Utilities.URI;
using Elasticsearch.Net;

namespace Business.Handlers.Posts.Queries
{

    public class GetPostsQuery : IRequest<IDataResult<PaginatedResult<IEnumerable<PostDto>>>>
    {

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public GetPostsQuery()
        {
        }

        public class PostQueryHandler : IRequestHandler<GetPostsQuery, IDataResult<PaginatedResult<IEnumerable<PostDto>>>>
        {
            private readonly IPostRepository _postRepository;
            private readonly IMapper _mapper;
            private readonly IUriService _uriService;

            public PostQueryHandler(IPostRepository postRepository, IMapper mapper, IUriService uriService)
            {
                _postRepository = postRepository;
                _mapper = mapper;
                _uriService = uriService;
            }
            [LogAspect(typeof(FileLogger))]
            [PerformanceAspect(5)]
            public async Task<IDataResult<PaginatedResult<IEnumerable<PostDto>>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
            {
                var posts = await _postRepository.GetPosts();
                var data = PaginationHelper.CreatePaginatedResponse<PostDto>(posts, new PaginationFilter { PageSize = request.PageSize, PageNumber = request.PageNumber }, posts.Count, _uriService, "/Post");
                return new SuccessDataResult<PaginatedResult<IEnumerable<PostDto>>>(data);
            }
        }
    }
}