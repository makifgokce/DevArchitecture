using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.Group
{
    public class CreateGroupIntervalCommand : IRequest<IResult>
    {
        public string GroupName { get; set; }

        public class CreateGroupIntervalCommandHandler : IRequestHandler<CreateGroupIntervalCommand, IResult>
        {
            private readonly IGroupRepository _groupRepository;


            public CreateGroupIntervalCommandHandler(IGroupRepository groupRepository)
            {
                _groupRepository = groupRepository;
            }

            public async Task<IResult> Handle(CreateGroupIntervalCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var groupExists = _groupRepository.Get(x => x.GroupName == request.GroupName);
                    if (groupExists == null)
                    {
                        var group = new Core.Entities.Concrete.Group
                        {
                            GroupName = request.GroupName
                        };
                        _groupRepository.Add(group);
                        await _groupRepository.SaveChangesAsync();
                    }

                    return new SuccessResult(Messages.Added);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}