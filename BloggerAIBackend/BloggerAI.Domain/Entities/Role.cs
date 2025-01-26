namespace BloggerAI.Domain.Entities;

public class Role
{
    public Role()
    {
        IdentityUsers = [];
    }

    public int Id { get; }
    public required string Name { get; set; }
    public virtual List<IdentityUser> IdentityUsers { get; set; }
}