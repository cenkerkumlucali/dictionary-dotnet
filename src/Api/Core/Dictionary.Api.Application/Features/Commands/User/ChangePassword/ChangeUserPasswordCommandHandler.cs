using AutoMapper;
using Common.Infrastructure;
using Common.Infrastructure.Exceptions;
using Common.Models.RequestModels.User;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;

namespace Dictionary.Api.Application.Features.Commands.User.ChangePassword;

public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, bool>
{
    private IUserRepository _userRepository;
    private IMapper _mapper;

    public ChangeUserPasswordCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        if (!request.UserId.HasValue)
            throw new ArgumentNullException(nameof(request.UserId));

        var dbUser = await _userRepository.GetByIdAsync(request.UserId.Value);
        if (dbUser is null)
            throw new DatabaseValidationException("User not found!");
        var encrptPassword = PasswordEncryptor.Encrpt(request.OldPassword);
        if (dbUser.Password == encrptPassword)
            throw new DatabaseValidationException("Old password wrong");
        dbUser.Password = PasswordEncryptor.Encrpt(request.NewPassword);
        await _userRepository.UpdateAsync(dbUser);
        return true;
    }
}