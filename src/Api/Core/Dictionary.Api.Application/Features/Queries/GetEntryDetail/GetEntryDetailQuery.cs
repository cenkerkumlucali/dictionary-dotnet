using Common.Models.Queries.Entry;
using MediatR;

namespace Dictionary.Api.Application.Features.Queries.GetEntryDetail;

public class GetEntryDetailQuery:IRequest<GetEntryDetailViewModel>
{
    public Guid EntryId { get; set; }
    public Guid? UserId { get; set; }

    public GetEntryDetailQuery(Guid entryId, Guid? userId)
    {
        EntryId = entryId;
        UserId = userId;
    }
}