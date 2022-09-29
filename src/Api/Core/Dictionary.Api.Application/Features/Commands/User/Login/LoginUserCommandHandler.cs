using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Common.Infrastructure;
using Common.Infrastructure.Exceptions;
using Common.Models.Queries;
using Common.Models.RequestModels.User;
using Dictionary.Api.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Dictionary.Api.Application.Features.Commands.User.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
{
    private IUserRepository _userRepository;
    private IMapper _mapper;
    private IConfiguration _configuration;

    public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var dbUser = await _userRepository.GetSingleAsync(c => c.Email == request.Email);
        if (dbUser == null)
            throw new DatabaseValidationException("User not found!");

        var checkPassword = PasswordEncryptor.Encrpt(request.Password);
        if (dbUser.Password != checkPassword)
            throw new DatabaseValidationException("Password is wrong!");

        if (!dbUser.EmailConfirmed)
            throw new DatabaseValidationException("Email address is not confirmed yet!");
        var result = _mapper.Map<LoginUserViewModel>(dbUser);
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
            new(ClaimTypes.Email, dbUser.Email),
            new(ClaimTypes.Name, dbUser.UserName),
            new(ClaimTypes.GivenName, dbUser.FirstName),
            new(ClaimTypes.Surname, dbUser.LastName),
        };
        result.Token = GenerateToken(claims);
        return result;
    }

    private string GenerateToken(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthConfig:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(10);

        var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds,
            notBefore: DateTime.Now);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}