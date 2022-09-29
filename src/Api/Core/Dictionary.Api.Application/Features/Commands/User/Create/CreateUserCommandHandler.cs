using AutoMapper;
using Common;
using Common.Events.User;
using Common.Infrastructure;
using Common.Infrastructure.Exceptions;
using Common.Models.RequestModels;
using Common.Models.RequestModels.User;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.User.Create;

public class CreateUserCommandHandler:IRequestHandler<CreateUserCommand,Guid>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existsUser = await _userRepository.GetSingleAsync(c => c.Email == request.Email);
        if (existsUser is not null)
            throw new DatabaseValidationException("User already exists!");
        var dbUser = _mapper.Map<Domain.Models.User>(request);
        var rows = await _userRepository.AddAsync(dbUser);
        //Email Changed/Created
        if (rows > 0)
        {
            var @event = new UserEmailChangedEvent()
            {
                OldEmailAddress = null,
                NewEmailAddress = dbUser.Email
            };
            QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.UserExchangeName,
                exchangeType:DictionaryConstants.DefaultExchangeType,
                queueName:DictionaryConstants.UserEmailChangedQueueName,
                obj:@event);
        }

        return dbUser.Id;
    }
}