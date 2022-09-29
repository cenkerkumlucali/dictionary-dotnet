using Common.Models.Queries.User;
using MediatR;

namespace Dictionary.Api.Application.Features.Queries.GetUserDetail;

public class GetUserDetailQuery:IRequest<UserDetailViewModel>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }

    public GetUserDetailQuery(Guid userId, string userName = null)
    {
        UserId = userId;
        UserName = userName;
    }
}