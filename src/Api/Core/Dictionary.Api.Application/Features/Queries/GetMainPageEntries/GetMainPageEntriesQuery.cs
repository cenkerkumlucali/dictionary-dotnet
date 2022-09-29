using Common.Models.Page;
using Common.Models.Queries.Entry;
using MediatR;

namespace Dictionary.Api.Application.Features.Queries.GetMainPageEntries;

public class GetMainPageEntriesQuery : BasePagedQuery, IRequest<PagedViewModel<GetEntryDetailViewModel>>
{
    public Guid? UserId { get; set; }

    public GetMainPageEntriesQuery(Guid? userId, int page, int pageSize) : base(page, pageSize)
    {
        UserId = userId;
    }
}
