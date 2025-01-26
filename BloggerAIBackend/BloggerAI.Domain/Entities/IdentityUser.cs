namespace BloggerAI.Domain.Entities;

public class IdentityUser
{
    public IdentityUser()
    {
        Roles = [];
    }

    public Guid Id { get; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }

    public virtual List<Role> Roles { get; set; }
}