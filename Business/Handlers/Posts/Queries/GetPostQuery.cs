
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Logging;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using AutoMapper;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Posts.Queries
{

    public class GetPostQuery : IRequest<IDataResult<PostDto>>
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public class GetPostQueryHandler : IRequestHandler<GetPostQuery, IDataResult<PostDto>>
        {
            private readonly IPostRepository _postRepository;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

            public GetPostQueryHandler(IPostRepository postRepository, IMediator mediator, IMapper mapper)
            {
                _postRepository = postRepository;
                _mediator = mediator;
                _mapper = mapper;
            }
            [LogAspect(typeof(FileLogger))]
            [CacheAspect(10)]
            public async Task<IDataResult<PostDto>> Handle(GetPostQuery request, CancellationToken cancellationToken)
            {
                var postDto = await _postRepository.GetPost(request.Id, request.Slug);
                return new SuccessDataResult<PostDto>(postDto);
            }
        }
    }
}
