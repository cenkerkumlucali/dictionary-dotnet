using Common;
using Common.Events.Entry;
using Common.Infrastructure;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.Entry.DeleteVote;

public class DeleteEntryVoteCommandHandler:IRequestHandler<DeleteEntryVoteCommand,bool>
{
    public async Task<bool> Handle(DeleteEntryVoteCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.VoteExchangeName,
            exchangeType:DictionaryConstants.DefaultExchangeType,
            queueName:DictionaryConstants.DeleteEntryVoteQueueName,
            obj:new DeleteEntryVoteEvent()
            {
                EntryId = request.EntryId,
                CreatedBy = request.UserId
            });
        return await Task.FromResult(true);
    }
}