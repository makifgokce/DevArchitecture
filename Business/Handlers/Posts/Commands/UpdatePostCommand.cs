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
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using Core.CrossCuttingConcerns.Caching;
using System.Collections.Generic;
using Business.Helpers;

namespace Business.Handlers.Posts.Commands
{


    public class UpdatePostCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Slug { get; set; }
        public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, IResult>
        {
            private readonly IPostRepository _postRepository;
            private readonly ICacheManager _cacheManager;
            private readonly IHttpContextAccessor _httpContext;

            public UpdatePostCommandHandler(IPostRepository postRepository, ICacheManager cacheManager, IHttpContextAccessor httpContext)
            {
                _postRepository = postRepository;
                _cacheManager = cacheManager;
                _httpContext = httpContext;
            }

            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(false, Priority = 1)]
            public async Task<IResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                var userId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                var oprClaims = _cacheManager.Get<IEnumerable<string>>($"{CacheKeys.UserIdForClaim}={userId}");
                var post = await _postRepository.GetAsync(x => x.Id == request.Id);
                if (post == null)
                {
                    return new ErrorResult(Messages.NotFound);
                }
                if (post.AuthorId != Convert.ToInt32(userId) && !oprClaims.Contains("UpdatePostCommand"))
                {
                    return new ErrorResult(Messages.AccessDenied);
                }
                post.Slug = String.IsNullOrEmpty(request.Slug.Trim()) ? request.Title.Trim().Slugify() : request.Slug.Trim().Slugify();
                post.Title = request.Title;
                post.Description = request.Description;
                post.Keywords = request.Keywords;
                post.Body = request.Body;
                post.UpdatedDate = DateTime.Now;
                _postRepository.Update(post);
                await _postRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

