using MediatR;

namespace Common.Models.RequestModels.User;

public class CreateUserCommand:IRequest<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}