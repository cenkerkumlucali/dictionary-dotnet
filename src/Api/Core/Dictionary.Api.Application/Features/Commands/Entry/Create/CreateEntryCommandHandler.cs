using AutoMapper;
using Common.Models.RequestModels.Entry;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.Entry.Create;

public class CreateEntryCommandHandler:IRequestHandler<CreateEntryCommand,Guid>
{
    private IEntryRepository _entryRepository;
    private IMapper _mapper;

    public CreateEntryCommandHandler(IEntryRepository entryRepository, IMapper mapper)
    {
        _entryRepository = entryRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateEntryCommand request, CancellationToken cancellationToken)
    {
        var entry = _mapper.Map<Domain.Models.Entry>(request);
        await _entryRepository.AddAsync(entry);
        return entry.Id;
    }
}