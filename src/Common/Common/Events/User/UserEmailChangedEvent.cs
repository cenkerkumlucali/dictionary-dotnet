namespace Common.Events.User;

public class UserEmailChangedEvent
{
    public string  OldEmailAddress { get; set; }
    public string  NewEmailAddress { get; set; }
}