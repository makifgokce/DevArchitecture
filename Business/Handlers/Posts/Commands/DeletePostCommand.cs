using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using IResult = Core.Utilities.Results.IResult;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Core.CrossCuttingConcerns.Caching;
using System.Collections.Generic;

namespace Business.Handlers.Posts.Commands
{


    public class DeletePostCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, IResult>
        {
            private readonly IPostRepository _postRepository;
            private readonly IHttpContextAccessor _httpContext;
            private readonly ICacheManager _cacheManager;

            public DeletePostCommandHandler(IPostRepository postRepository, IHttpContextAccessor httpContext, ICacheManager cacheManager)
            {
                _postRepository = postRepository;
                _httpContext = httpContext;
                _cacheManager = cacheManager;
            }

            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(false, Priority = 1)]
            public async Task<IResult> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                var userId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                var oprClaims = _cacheManager.Get<IEnumerable<string>>($"{CacheKeys.UserIdForClaim}={userId}");
                var post = await _postRepository.GetAsync(x => x.Id == request.Id && x.AuthorId == Convert.ToInt32(userId));
                if (post == null)
                {
                    return new ErrorResult(Messages.NotFound);
                }
                if (post.AuthorId != Convert.ToInt32(userId) && !oprClaims.Contains("DeletePostCommand"))
                {
                    return new ErrorResult(Messages.AccessDenied);
                }
                post.DeletedDate = DateTime.Now;
                _postRepository.Update(post);
                await _postRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

