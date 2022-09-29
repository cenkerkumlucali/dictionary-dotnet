using Common;
using Common.Events.Entry;
using Common.Events.EntryComment;
using Common.Infrastructure;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.EntryComment.DeleteVote;

public class DeleteEntryCommentVoteCommandHandler:IRequestHandler<DeleteEntryCommentVoteCommand,bool>
{
    public async Task<bool> Handle(DeleteEntryCommentVoteCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.VoteExchangeName,
            exchangeType:DictionaryConstants.DefaultExchangeType,
            queueName:DictionaryConstants.DeleteEntryCommentVoteQueueName,
            obj:new DeleteEntryCommentVoteEvent()
            {
                EntryCommentId= request.EntryCommentId,
                CreatedBy = request.UserId
            });
        return await Task.FromResult(true);
    }
}