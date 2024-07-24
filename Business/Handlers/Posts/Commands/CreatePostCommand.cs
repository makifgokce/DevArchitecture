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
using Business.Helpers;
using Castle.Core.Internal;

namespace Business.Handlers.Posts.Commands
{


    public class CreatePostCommand : IRequest<IResult>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string? PublishDate { get; set; }
        public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, IResult>
        {
            private readonly IPostRepository _postRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            public CreatePostCommandHandler(IPostRepository postRepository, IHttpContextAccessor httpContext)
            {
                _postRepository = postRepository;
                _contextAccessor = httpContext;
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
                var publish = request.PublishDate.IsNullOrEmpty() ? DateTime.Now : Convert.ToDateTime(request.PublishDate);
                var post = new Post
                {
                    Title = request.Title,
                    Body = request.Body,
                    Slug = String.IsNullOrEmpty(request.Slug.Trim()) ? request.Title.Trim().Slugify() : request.Slug.Trim().Slugify(),
                    Description = request.Description,
                    Keywords = request.Keywords,
                    AuthorId = Convert.ToInt32(userId),
                    PublishDate = publish < DateTime.Now ? DateTime.Now : publish,
                };
                _postRepository.Add(post);
                await _postRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}
