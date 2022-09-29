using Common.Infrastructure.Extensions;
using Common.Models;
using Common.Models.Queries.Entry;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Api.Application.Features.Queries.GetEntryDetail;

public class GetEntryDetailQueryHandler : IRequestHandler<GetEntryDetailQuery, GetEntryDetailViewModel>
{
    private readonly IEntryRepository _entryRepository;

    public GetEntryDetailQueryHandler(IEntryRepository entryRepository)
    {
        _entryRepository = entryRepository;
    }

    public async Task<GetEntryDetailViewModel> Handle(GetEntryDetailQuery request, CancellationToken cancellationToken)
    {
        var query = _entryRepository.AsQueryable();
        query.Include(c => c.EntryFavorites)
            .Include(c => c.CreatedBy)
            .Include(c => c.EntryVotes)
            .Where(c => c.Id == request.EntryId);
        var list = query.Select(c => new GetEntryDetailViewModel
        {
            Id = c.Id,
            Subject = c.Subject,
            Content = c.Content,
            IsFavorited = request.UserId.HasValue && c.EntryFavorites.Any(i => i.CreatedById == request.UserId),
            FavoritedCount = c.EntryFavorites.Count,
            CreatedDate = c.CreateDate,
            CreatedByUserName = c.CreatedBy.UserName,
            VoteType =
                request.UserId.HasValue && c.EntryVotes.Any(i => i.CreatedById == request.UserId)
                    ? c.EntryVotes.FirstOrDefault(c => c.CreatedById == request.UserId).VoteType
                    : VoteType.None,
        });
        return await list.FirstOrDefaultAsync(cancellationToken);
    }
}