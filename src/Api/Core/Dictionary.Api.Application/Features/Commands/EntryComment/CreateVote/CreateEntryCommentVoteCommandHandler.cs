using Common;
using Common.Events.EntryComment;
using Common.Infrastructure;
using Common.Models.RequestModels.EntryComment;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.EntryComment.CreateVote;

public class CreateEntryCommentVoteCommandHandler:IRequestHandler<CreateEntryCommentVoteCommand,bool>
{
    public async Task<bool> Handle(CreateEntryCommentVoteCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.VoteExchangeName,
            exchangeType:DictionaryConstants.DefaultExchangeType,
            queueName:DictionaryConstants.CreateEntryCommentVoteQueueName,
            obj:new CreateEntryCommentVoteEvent
            {
                EntryCommentId = request.EntryCommentId,
                VoteType = request.VoteType,
                CreatedBy = request.CreatedBy
            });
        return await Task.FromResult(true);
    }
}