using Common;
using Common.Events.EntryComment;
using Common.Infrastructure;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.EntryComment.CreateFav;

public class CreateEntryCommentFavCommandHandler:IRequestHandler<CreateEntryCommentFavCommand,bool>
{
    public async Task<bool> Handle(CreateEntryCommentFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.FavExchangeName,
            exchangeType:DictionaryConstants.DefaultExchangeType,
            queueName:DictionaryConstants.CreateEntryCommentFavQueueName,
            obj:new CreateEntryCommentFavEvent
            {
                EntryCommentId = request.EntryCommentId,
                CreatedBy = request.UserId
            });
        return await Task.FromResult(true);
    }
}