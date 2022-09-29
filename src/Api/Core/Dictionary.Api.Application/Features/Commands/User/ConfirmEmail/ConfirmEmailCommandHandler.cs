using Common.Infrastructure.Exceptions;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.User.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
{
    private IUserRepository _userRepository;
    private IEmailConfirmationRepository _emailConfirmationRepository;

    public ConfirmEmailCommandHandler(IUserRepository userRepository,
        IEmailConfirmationRepository emailConfirmationRepository)
    {
        _userRepository = userRepository;
        _emailConfirmationRepository = emailConfirmationRepository;
    }

    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var confirmation = await _emailConfirmationRepository.GetByIdAsync(request.ConfirmationId);
        if (confirmation is null)
            throw new DatabaseValidationException("Confirmation not found!");
        var user = await _userRepository.GetSingleAsync(c => c.Email == confirmation.NewEmailAddress);
        if (user is null)
            throw new DatabaseValidationException("User not found with this email!");
        if (user.EmailConfirmed)
            throw new DatabaseValidationException("Email address is already confirmed!");
        user.EmailConfirmed = true;
        await _userRepository.UpdateAsync(user);
        return true;
    }
}