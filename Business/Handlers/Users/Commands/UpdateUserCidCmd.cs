using Business.Adapters.PersonService;
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Handlers.Users.Commands
{
    public class UpdateUserCidCmd : IRequest<IResult>
    {
        public long CitizenId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public class UpdateUserCidCommandHandler : IRequestHandler<UpdateUserCidCmd, IResult>
        {
            private readonly IUserRepository _userRepository;
            private readonly IPersonService _personService;
            private readonly IHttpContextAccessor _httpContext;

            public UpdateUserCidCommandHandler(IUserRepository userRepository, IPersonService personService, IHttpContextAccessor httpContext)
            {
                _userRepository = userRepository;
                _personService = personService;
                _httpContext = httpContext;
            }


            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(UpdateUserCidCmd request, CancellationToken cancellationToken)
            {
                var uId = _httpContext.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                var isThereAnyUser = await _userRepository.GetAsync(u => u.UserId == Convert.ToInt32(uId));
                var isThereAnyCid = await _userRepository.GetAsync(u => u.CitizenId == request.CitizenId);
                if (isThereAnyUser == null)
                {
                    return new ErrorResult(Messages.CouldNotBeVerifyCid);
                }
                if (isThereAnyCid != null)
                {
                    return new ErrorResult(Messages.NameAlreadyExist);
                }
                isThereAnyUser.Name = request.Name;
                isThereAnyUser.Surname = request.Surname;
                isThereAnyUser.CitizenId = request.CitizenId;
                isThereAnyUser.BirthDate = request.BirthDate;
                isThereAnyUser.Verified = true;
                var result = await _personService.VerifyCid(new Citizen()
                {
                    BirthYear = request.BirthDate.Year,
                    CitizenId = request.CitizenId,
                    Name = request.Name,
                    Surname = request.Surname
                });
                if (!result)
                {
                    return new ErrorResult(Messages.CouldNotBeVerifyCid);
                }
                _userRepository.Update(isThereAnyUser);
                await _userRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}
