using MediatR;

namespace Common.Models.RequestModels.User;

public class ChangeUserPasswordCommand:IRequest<bool>
{
    public Guid? UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    public ChangeUserPasswordCommand(Guid? userId, string oldPassword, string newPassword)
    {
        UserId = userId;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}