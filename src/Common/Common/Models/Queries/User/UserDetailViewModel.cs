namespace Common.Models.Queries.User;

public class UserDetailViewModel
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
}