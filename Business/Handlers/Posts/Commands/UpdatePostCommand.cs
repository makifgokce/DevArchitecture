
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
using Slugify;

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
            private readonly IMediator _mediator;
            private readonly IHttpContextAccessor _httpContext;
            private readonly ISlugHelper _slugHelper;

            public UpdatePostCommandHandler(IPostRepository postRepository, IMediator mediator, IHttpContextAccessor httpContext, ISlugHelper slugHelper)
            {
                _postRepository = postRepository;
                _mediator = mediator;
                _httpContext = httpContext;
                _slugHelper = slugHelper;
            }

            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                var userId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                var post = await _postRepository.GetAsync(x => x.Id == request.Id && x.AuthorId == Convert.ToInt32(userId));
                if (post == null)
                {
                    return new ErrorResult(Messages.Unknown);
                }
                post.Slug = String.IsNullOrEmpty(request.Slug.Trim()) ? _slugHelper.GenerateSlug(request.Title.Trim()) : _slugHelper.GenerateSlug(request.Slug.Trim());
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

