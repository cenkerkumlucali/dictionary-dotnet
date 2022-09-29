using AutoMapper;
using Common.Models.RequestModels.EntryComment;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.EntryComment.Create;

public class CreateEntryCommentCommandHandler:IRequestHandler<CreateEntryCommentCommand,Guid>
{
    private IEntryCommentRepository _entryCommentRepository;
    private IMapper _mapper;

    public CreateEntryCommentCommandHandler(IEntryCommentRepository entryCommentRepository, IMapper mapper)
    {
        _entryCommentRepository = entryCommentRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateEntryCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = _mapper.Map<Domain.Models.EntryComment>(request);
        await _entryCommentRepository.AddAsync(comment);
        return comment.Id;
    }
}