namespace BloggerAI.Domain.Entities;

public class Blogger
{
    public Blogger()
    {
        Posts = [];
    }

    public Guid Id { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public Guid IdentityUserId { get; set; }
    public virtual IdentityUser? IdentityUser { get; set; }
    public virtual List<Post> Posts { get; set; }
}