using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Models.Queries.Entry;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Api.Application.Features.Queries.GetEntries;

public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
{
    private readonly IEntryRepository _entryRepository;
    private readonly IMapper _mapper;

    public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper)
    {
        _entryRepository = entryRepository;
        _mapper = mapper;
    }

    public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
    {
        var query = _entryRepository.AsQueryable();
        if (request.TodaysEntries)
        {
            query = query
                .Where(c => c.CreateDate >= DateTime.Now.Date)
                .Where(c => c.CreateDate <= DateTime.Now.AddDays(1).Date);
        }

        query = query.Include(c => c.EntryComments)
            .OrderBy(c => Guid.NewGuid())
            .Take(request.Count);
        return await query.ProjectTo<GetEntriesViewModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}