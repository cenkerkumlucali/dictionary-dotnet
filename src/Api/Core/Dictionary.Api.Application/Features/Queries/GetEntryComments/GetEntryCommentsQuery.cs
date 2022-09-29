using Common.Models.Page;
using Common.Models.Queries.EntryComment;
using MediatR;

namespace Dictionary.Api.Application.Features.Queries.GetEntryComments;

public class GetEntryCommentsQuery:BasePagedQuery,IRequest<PagedViewModel<GetEntryCommentsViewModel>>
{
    public GetEntryCommentsQuery(int page, int pageSize, Guid entryId, Guid? userId) : base(page, pageSize)
    {
        EntryId = entryId;
        UserId = userId;
    }

    public Guid EntryId { get; set; }
    public Guid? UserId { get; set; }
}