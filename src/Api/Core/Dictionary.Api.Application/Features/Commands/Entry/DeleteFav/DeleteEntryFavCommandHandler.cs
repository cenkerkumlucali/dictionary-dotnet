using Common;
using Common.Events.Entry;
using Common.Infrastructure;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.Entry.DeleteFav;

public class DeleteEntryFavCommandHandler:IRequestHandler<DeleteEntryFavCommand,bool>
{
    public async Task<bool> Handle(DeleteEntryFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.FavExchangeName,
            exchangeType:DictionaryConstants.DefaultExchangeType,
            queueName:DictionaryConstants.DeleteEntryFavQueueName,
            obj:new DeleteEntryFavEvent()
            {
                EntryId = request.EntryId,
                CreatedBy = request.UserId
            });
        return await Task.FromResult(true);
    }
}