using AutoMapper;
using Common;
using Common.Events.User;
using Common.Infrastructure;
using Common.Infrastructure.Exceptions;
using Common.Models.RequestModels;
using Common.Models.RequestModels.User;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.User.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dbUser = await _userRepository.GetByIdAsync(request.Id);
        if (dbUser is null)
            throw new DatabaseValidationException("User not found!");
        var dbEmailAddress = dbUser.Email;
        var emailChanged = string.CompareOrdinal(dbEmailAddress, request.Email) != 0;
        _mapper.Map(request, dbUser);
        var rows = await _userRepository.UpdateAsync(dbUser);
        //Check if email changed
        if (emailChanged && rows > 0)
        {
            var @event = new UserEmailChangedEvent()
            {
                OldEmailAddress = null,
                NewEmailAddress = dbUser.Email
            };
            QueueFactory.SendMessageToExchange(exchangeName: DictionaryConstants.UserExchangeName,
                exchangeType: DictionaryConstants.DefaultExchangeType,
                queueName: DictionaryConstants.UserEmailChangedQueueName,
                obj: @event);
            dbUser.EmailConfirmed = false;
            await _userRepository.UpdateAsync(dbUser);
        }

        return dbUser.Id;
    }
}