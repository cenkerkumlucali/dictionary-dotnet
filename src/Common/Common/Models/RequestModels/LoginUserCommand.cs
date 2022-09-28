using Common.Models.Queries;
using MediatR;

namespace Common.Models.RequestModels;

public class LoginUserCommand:IRequest<LoginUserViewModel>
{
    public string Email { get;  set; }
    public string Password { get;  set; }

    public LoginUserCommand()
    {
        
    }   
    public LoginUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}