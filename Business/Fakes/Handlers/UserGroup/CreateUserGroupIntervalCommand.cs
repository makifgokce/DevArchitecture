using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.UserGroup
{
    public class CreateUserGroupIntervalCommand : IRequest<IResult>
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }

        public class CreateUserGroupIntervalCommandHandler : IRequestHandler<CreateUserGroupIntervalCommand, IResult>
        {
            private readonly IUserGroupRepository _userGroupRepository;

            public CreateUserGroupIntervalCommandHandler(IUserGroupRepository userGroupRepository)
            {
                _userGroupRepository = userGroupRepository;
            }

            public async Task<IResult> Handle(CreateUserGroupIntervalCommand request, CancellationToken cancellationToken)
            {
                var _userGroup = _userGroupRepository.Get(x => x.UserId == request.UserId && x.GroupId == request.UserId);
                if (_userGroup == null)
                {
                    var userGroup = new Core.Entities.Concrete.UserGroup
                    {
                        GroupId = request.GroupId,
                        UserId = request.UserId
                    };

                    _userGroupRepository.Add(userGroup);
                    await _userGroupRepository.SaveChangesAsync();
                }
                return new SuccessResult(Messages.Added);
            }
        }
    }
}