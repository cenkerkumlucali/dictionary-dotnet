using Common.Infrastructure.Extensions;
using Common.Models.Page;
using Common.Models.Queries.User;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Api.Application.Features.Queries.GetUserEntries;

public class GetUserEntriesQueryHandler:IRequestHandler<GetUserEntriesQuery,PagedViewModel<GetUserEntriesDetailViewModel>>
{
    private readonly IEntryRepository _entryRepository;

    public GetUserEntriesQueryHandler(IEntryRepository entryRepository)
    {
        _entryRepository = entryRepository;
    }

    public async Task<PagedViewModel<GetUserEntriesDetailViewModel>> Handle(GetUserEntriesQuery request, CancellationToken cancellationToken)
    {
        var query = _entryRepository.AsQueryable();
        if (request.UserId != null && request.UserId.HasValue && request.UserId != Guid.Empty)
        {
            query = query.Where(c => c.CreatedById == request.UserId);
        }
        else if (!string.IsNullOrEmpty(request.UserName))
        {
            query = query.Where(c => c.CreatedBy.UserName == request.UserName);
        }
        else return null;

        query = query.Include(c => c.EntryFavorites)
                     .Include(c => c.CreatedBy);
        
        var list = query.Select(c => new GetUserEntriesDetailViewModel
        {
            Id = c.Id,
            Subject = c.Subject,
            Content = c.Content,
            IsFavorited = false,
            FavoritedCount = c.EntryFavorites.Count,
            CreatedDate = c.CreateDate,
            CreatedByUserName = c.CreatedBy.UserName,
        });
        var entries = await list.GetPaged(request.Page, request.PageSize);
        return entries;
    }
}