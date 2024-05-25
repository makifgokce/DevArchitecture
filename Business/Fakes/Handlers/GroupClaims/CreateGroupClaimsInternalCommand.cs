using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.UserClaims
{
    /// <summary>
    /// For Internal Use Only,
    /// Registers All Existing Operation Claims To Given User
    /// </summary>
    public class CreateGroupClaimsInternalCommand : IRequest<IResult>
    {
        public int GroupId { get; set; }
        public IEnumerable<OperationClaim> OperationClaims { get; set; }

        public class CreateUserClaimsInternalCommandHandler : IRequestHandler<CreateGroupClaimsInternalCommand, IResult>
        {
            private readonly IGroupClaimRepository _groupClaimsRepository;

            public CreateUserClaimsInternalCommandHandler(IGroupClaimRepository groupClaimsRepository)
            {
                _groupClaimsRepository = groupClaimsRepository;
            }

            public async Task<IResult> Handle(CreateGroupClaimsInternalCommand request, CancellationToken cancellationToken)
            {
                foreach (var claim in request.OperationClaims)
                {
                    if (await DoesClaimExistsForGroup(new GroupClaim { ClaimId = claim.Id, GroupId = request.GroupId }))
                    {
                        continue;
                    }

                    _groupClaimsRepository.Add(new GroupClaim
                    {
                        ClaimId = claim.Id,
                        GroupId = request.GroupId
                    });
                }

                await _groupClaimsRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }

            private async Task<bool> DoesClaimExistsForGroup(GroupClaim groupClaim)
            {
                return (await _groupClaimsRepository.GetAsync(x =>
                    x.GroupId == groupClaim.GroupId && x.ClaimId == groupClaim.ClaimId)) is { };
            }
        }
    }
}