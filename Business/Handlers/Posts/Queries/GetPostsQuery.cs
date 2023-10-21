﻿using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Logging;
using System.Collections.Generic;
using System.Linq;
using Core.Entities.Dtos;
using AutoMapper;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Posts.Queries
{

    public class GetPostsQuery : IRequest<IDataResult<IEnumerable<PostDto>>>
    {

        public class PostQueryHandler : IRequestHandler<GetPostsQuery, IDataResult<IEnumerable<PostDto>>>
        {
            private readonly IPostRepository _postRepository;
            private readonly IMapper _mapper;

            public PostQueryHandler(IPostRepository postRepository, IMapper mapper)
            {
                _postRepository = postRepository;
                _mapper = mapper;
            }
            [LogAspect(typeof(FileLogger))]
            [PerformanceAspect(5)]
            [CacheAspect(10)]
            public async Task<IDataResult<IEnumerable<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
            {
                var posts = await _postRepository.GetListAsync(x => x.DeletedDate == null);
                var list = posts.Select(pt => _mapper.Map<PostDto>(pt)).ToList();

                return new SuccessDataResult<IEnumerable<PostDto>>(list);
            }
        }
    }
}
