using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.Authorizations.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using MediatR;
using static Core.Entities.Concrete.User;

namespace Business.Fakes.Handlers.Authorizations
{
    public class RegisterUserInternalCommand : IRequest<IResult>
    {
        public string Account { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public class RegisterUserInternalCommandHandler : IRequestHandler<RegisterUserInternalCommand, IResult>
        {
            private readonly IUserRepository _userRepository;


            public RegisterUserInternalCommandHandler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }


            [ValidationAspect(typeof(RegisterUserValidator), Priority = 2)]
            [CacheRemoveAspect()]
            public async Task<IResult> Handle(RegisterUserInternalCommand request, CancellationToken cancellationToken)
            {
                var isThereAnyUser = await _userRepository.GetAsync(u => u.Email == request.Email);
                var isThereAnyAccount = await _userRepository.GetAsync(u => u.Account == request.Account);

                if (isThereAnyUser != null)
                {
                    return new ErrorResult(Messages.NameAlreadyExist);
                }
                if (isThereAnyAccount != null)
                {
                    return new ErrorResult(Messages.AccountAlreadyExist);
                }

                HashingHelper.CreatePasswordHash(request.Password, out var passwordSalt, out var passwordHash);
                var user = new User
                {
                    Email = request.Email,
                    Account = request.Account,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = UserStatus.Activated
                };

                _userRepository.Add(user);
                await _userRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}