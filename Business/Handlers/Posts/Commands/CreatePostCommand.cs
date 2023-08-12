
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using IResult = Core.Utilities.Results.IResult;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Core.Utilities.Results;
using System.Linq;
using Core.Entities.Concrete;
using System;
using Slugify;

namespace Business.Handlers.Posts.Commands
{


    public class CreatePostCommand : IRequest<IResult>
    {
        public string Title {  get; set; }
        public string Body { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, IResult>
        {
            private readonly IPostRepository _postRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly ISlugHelper _slugHelper;
            public CreatePostCommandHandler(IPostRepository postRepository, IHttpContextAccessor httpContext, ISlugHelper slugHelper)
            {
                _postRepository = postRepository;
                _contextAccessor = httpContext;
                _slugHelper = slugHelper;
            }

            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
            {
                var userId = _contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                if (userId == null)
                {
                    return new ErrorResult(Messages.AuthorizationsDenied);
                }
                var post = new Post
                {
                    Title = request.Title,
                    Body = request.Body,
                    Slug = String.IsNullOrEmpty(request.Slug.Trim()) ? _slugHelper.GenerateSlug(request.Title) : _slugHelper.GenerateSlug(request.Slug),
                    Description = request.Description,
                    Keywords = request.Keywords,
                    AuthorId = Convert.ToInt32(userId)
                };
                _postRepository.Add(post);
                await _postRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}

