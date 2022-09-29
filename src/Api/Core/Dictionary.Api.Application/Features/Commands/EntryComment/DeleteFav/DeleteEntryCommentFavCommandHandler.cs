using Common;
using Common.Events.Entry;
using Common.Events.EntryComment;
using Common.Infrastructure;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.EntryComment.DeleteFav;

public class DeleteEntryCommentFavCommandHandler:IRequestHandler<DeleteEntryCommentFavCommand,bool>
{
    public async Task<bool> Handle(DeleteEntryCommentFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.FavExchangeName,
            exchangeType:DictionaryConstants.DefaultExchangeType,
            queueName:DictionaryConstants.DeleteEntryCommentFavQueueName,
            obj:new DeleteEntryCommentFavEvent()
            {
                EntryCommentId = request.EntryCommentId,
                CreatedBy = request.UserId
            });
        return await Task.FromResult(true);
    }
}