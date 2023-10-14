using Business.Adapters.PersonService;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.Authorizations.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;
using ServiceStack;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Authorizations.Commands
{
    public class RegisterUserCommand : IRequest<IResult>
    {

        public long CitizenId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string Password { get; set; }


        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IResult>
        {
            private readonly IUserRepository _userRepository;
            private readonly IPersonService _personService;


            public RegisterUserCommandHandler(IUserRepository userRepository, IPersonService personService)
            {
                _userRepository = userRepository;
                _personService = personService;
            }


            //[SecuredOperation(Priority = 1)]
            [ValidationAspect(typeof(RegisterUserValidator), Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                var isThereAnyUser = await _userRepository.GetAsync(u => u.Email == request.Email);
                var isThereAnyAccount = await _userRepository.GetAsync(u => u.Account == request.Account);
                var isThereAnyCid = await _userRepository.GetAsync(u => u.CitizenId == request.CitizenId);
                if (isThereAnyUser != null)
                {
                    return new ErrorResult(Messages.NameAlreadyExist);
                }
                if (isThereAnyAccount != null)
                {
                    return new ErrorResult(Messages.AccountAlreadyExist);
                }
                if (isThereAnyCid != null)
                {
                    return new ErrorResult(Messages.NameAlreadyExist);
                }
                HashingHelper.CreatePasswordHash(request.Password, out var passwordSalt, out var passwordHash);
                var user = new User
                {
                    CitizenId = request.CitizenId,
                    Email = request.Email,
                    Account = request.Account,
                    Name = request.Name,
                    Surname = request.Surname,
                    BirthDate = Convert.ToDateTime(request.BirthDate),
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Verified = true,
                    Status = User.UserStatus.NotActivated,
                    Gender = request.Gender,
                    Address = request.Address,
                    Notes = request.Notes,
                };
                var res = await _personService.VerifyCid(new Citizen()
                {
                    CitizenId = user.CitizenId,
                    Name = user.Name,
                    Surname = user.Surname,
                    BirthYear = user.BirthDate.Year
                });
                if (!res)
                {
                    return new ErrorResult(Messages.CouldNotBeVerifyCid);
                }
                _userRepository.Add(user);
                await _userRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}