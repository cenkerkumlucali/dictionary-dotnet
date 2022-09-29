using Common.Infrastructure.Extensions;
using Common.Models;
using Common.Models.Page;
using Common.Models.Queries.Entry;
using Common.Models.Queries.EntryComment;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Api.Application.Features.Queries.GetEntryComments;

public class GetEntryCommentsQueryHandler:IRequestHandler<GetEntryCommentsQuery,PagedViewModel<GetEntryCommentsViewModel>>
{
    private readonly IEntryCommentRepository _entryCommentRepository;

    public GetEntryCommentsQueryHandler(IEntryCommentRepository entryCommentRepository)
    {
        _entryCommentRepository = entryCommentRepository;
    }

    public async Task<PagedViewModel<GetEntryCommentsViewModel>> Handle(GetEntryCommentsQuery request, CancellationToken cancellationToken)
    {
        var query = _entryCommentRepository.AsQueryable();
        query.Include(c => c.EntryCommentFavorites)
            .Include(c => c.CreatedBy)
            .Include(c => c.EntryCommentVotes)
            .Where(c=>c.EntryId == request.EntryId);
        
        var list = query.Select(c => new GetEntryCommentsViewModel
        {
            Id = c.Id,
            Content = c.Content,
            IsFavorited = request.UserId.HasValue && c.EntryCommentFavorites.Any(i => i.CreatedById == request.UserId),
            FavoritedCount = c.EntryCommentFavorites.Count,
            CreatedDate = c.CreateDate,
            CreatedByUserName = c.CreatedBy.UserName,
            VoteType =
                request.UserId.HasValue && c.EntryCommentVotes.Any(i => i.CreatedById == request.UserId)
                    ? c.EntryCommentVotes.FirstOrDefault(c => c.CreatedById == request.UserId).VoteType
                    : VoteType.None,
        });
        var entries = await list.GetPaged(request.Page, request.PageSize);
        return entries;
    }
}