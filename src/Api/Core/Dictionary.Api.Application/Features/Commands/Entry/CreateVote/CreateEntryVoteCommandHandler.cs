using AutoMapper;
using Common;
using Common.Events.Entry;
using Common.Infrastructure;
using Common.Models.RequestModels.Entry;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.Entry.CreateVote;

public class CreateEntryVoteCommandHandler:IRequestHandler<CreateEntryVoteCommand,bool>
{
    
    public async Task<bool> Handle(CreateEntryVoteCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.VoteExchangeName,
            exchangeType:DictionaryConstants.DefaultExchangeType,
            queueName:DictionaryConstants.CreateEntryVoteQueueName,
            obj:new CreateEntryVoteEvent()
            {
                EntryId = request.EntryId,
                VoteType = request.VoteType,
                CreatedBy = request.CreatedBy
            });
        return await Task.FromResult(true);
    }
}