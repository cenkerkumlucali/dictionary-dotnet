using Common.Models.Queries.Entry;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Api.Application.Features.Queries.SearchBySubject;

public class SearchEntryQueryHandler : IRequestHandler<SearchEntryQuery, List<SearchEntryViewModel>>
{
    private readonly IEntryRepository _entryRepository;

    public SearchEntryQueryHandler(IEntryRepository entryRepository)
    {
        _entryRepository = entryRepository;
    }

    public async Task<List<SearchEntryViewModel>> Handle(SearchEntryQuery request, CancellationToken cancellationToken)
    {
        var result = _entryRepository
            .Get(c => EF.Functions.Like(c.Subject, $"{request.SearchText}%")).Select(c =>
                new SearchEntryViewModel
                {
                    Id = c.Id,
                    Subject = c.Subject
                });
        
        return await result.ToListAsync(cancellationToken);
    }
}