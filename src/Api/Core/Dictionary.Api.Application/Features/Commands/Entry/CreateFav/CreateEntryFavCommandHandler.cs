using Common;
using Common.Events.Entry;
using Common.Events.EntryComment;
using Common.Infrastructure;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.Entry.CreateFav;

public class CreateEntryFavCommandHandler:IRequestHandler<CreateEntryFavCommand,bool>
{
    public async Task<bool> Handle(CreateEntryFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.FavExchangeName,
            exchangeType:DictionaryConstants.DefaultExchangeType,
            queueName:DictionaryConstants.CreateEntryFavQueueName,
            obj:new CreateEntryFavEvent()
            {
                EntryId = request.EntryId.Value,
                CreatedBy = request.UserId.Value
            });
        return await Task.FromResult(true);
    }
}